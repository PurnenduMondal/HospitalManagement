using HospitalManagement.Application.DTOs.Doctor;
using HospitalManagement.Application.Interfaces;
using HospitalManagement.Application.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DoctorController : ControllerBase
{
    private readonly IDoctorService _doctorService;

    public DoctorController(IDoctorService doctorService)
    {
        _doctorService = doctorService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _doctorService.GetAllAsync();
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _doctorService.GetByIdAsync(id);
        return result.Success ? Ok(result) : NotFound(result);
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string keyword)
    {
        var result = await _doctorService.SearchAsync(keyword);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpGet("specialization/{specialization}")]
    public async Task<IActionResult> GetBySpecialization(string specialization)
    {
        var result = await _doctorService.GetBySpecializationAsync(specialization);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateDoctorDto dto)
    {
        var validator  = new CreateDoctorValidator();
        var validation = await validator.ValidateAsync(dto);
        if (!validation.IsValid)
            return BadRequest(validation.Errors.Select(e => e.ErrorMessage));

        var result = await _doctorService.CreateAsync(dto);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateDoctorDto dto)
    {
        var result = await _doctorService.UpdateAsync(id, dto);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _doctorService.DeleteAsync(id);
        return result.Success ? Ok(result) : BadRequest(result);
    }
}
