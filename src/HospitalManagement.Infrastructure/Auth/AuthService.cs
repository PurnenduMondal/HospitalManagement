using HospitalManagement.Application.Common;
using HospitalManagement.Application.DTOs.Auth;
using HospitalManagement.Application.Interfaces;
using HospitalManagement.Domain.Enums;
using HospitalManagement.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace HospitalManagement.Infrastructure.Auth;

public class AuthService : IAuthService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly JwtTokenService _jwtTokenService;

    public AuthService(
        UserManager<AppUser> userManager,
        RoleManager<IdentityRole> roleManager,
        JwtTokenService jwtTokenService)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<BaseResponse<string>> RegisterAsync(RegisterRequest request)
    {
        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser != null)
            return BaseResponse<string>.Fail("Email is already registered.");

        var validRoles = new[] { UserRole.Admin, UserRole.Doctor, UserRole.Nurse, UserRole.Patient };
        if (!validRoles.Contains(request.Role))
            return BaseResponse<string>.Fail($"Invalid role. Valid roles: {string.Join(", ", validRoles)}");

        var user = new AppUser
        {
            FullName = request.FullName,
            Email    = request.Email,
            UserName = request.Email,
            Role     = request.Role
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
            return BaseResponse<string>.Fail("Registration failed.",
                result.Errors.Select(e => e.Description).ToList());

        if (!await _roleManager.RoleExistsAsync(request.Role))
            await _roleManager.CreateAsync(new IdentityRole(request.Role));

        await _userManager.AddToRoleAsync(user, request.Role);

        return BaseResponse<string>.Ok(user.Id, "User registered successfully.");
    }

    public async Task<BaseResponse<LoginResponse>> LoginAsync(LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
            return BaseResponse<LoginResponse>.Fail("Invalid email or password.");

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);
        if (!isPasswordValid)
            return BaseResponse<LoginResponse>.Fail("Invalid email or password.");

        var roles = await _userManager.GetRolesAsync(user);
        var role  = roles.FirstOrDefault() ?? string.Empty;

        var (token, expiresAt) = _jwtTokenService.GenerateToken(user, role);

        var response = new LoginResponse
        {
            Token     = token,
            FullName  = user.FullName,
            Email     = user.Email!,
            Role      = role,
            ExpiresAt = expiresAt
        };

        return BaseResponse<LoginResponse>.Ok(response, "Login successful.");
    }
}