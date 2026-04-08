using HospitalManagement.Application.DTOs.Patient;
using HospitalManagement.Application.Interfaces;
using HospitalManagement.Application.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PatientController : ControllerBase
{
    private readonly IPatientService _patientService;

    public PatientController(IPatientService patientService)
    {
        _patientService = patientService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _patientService.GetAllAsync();
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _patientService.GetByIdAsync(id);
        return result.Success ? Ok(result) : NotFound(result);
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string keyword)
    {
        var result = await _patientService.SearchAsync(keyword);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Doctor,Nurse")]
    public async Task<IActionResult> Create([FromBody] CreatePatientDto dto)
    {
        var validator  = new CreatePatientValidator();
        var validation = await validator.ValidateAsync(dto);
        if (!validation.IsValid)
            return BadRequest(validation.Errors.Select(e => e.ErrorMessage));

        var result = await _patientService.CreateAsync(dto);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin,Doctor,Nurse")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePatientDto dto)
    {
        var result = await _patientService.UpdateAsync(id, dto);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _patientService.DeleteAsync(id);
        return result.Success ? Ok(result) : BadRequest(result);
    }
}
