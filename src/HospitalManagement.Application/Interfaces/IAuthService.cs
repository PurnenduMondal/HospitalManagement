using HospitalManagement.Application.Common;
using HospitalManagement.Application.DTOs.Auth;

namespace HospitalManagement.Application.Interfaces;

public interface IAuthService
{
    Task<BaseResponse<string>> RegisterAsync(RegisterRequest request);
    Task<BaseResponse<LoginResponse>> LoginAsync(LoginRequest request);
}