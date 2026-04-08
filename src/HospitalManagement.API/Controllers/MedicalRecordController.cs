using HospitalManagement.Application.DTOs.MedicalRecord;
using HospitalManagement.Application.Interfaces;
using HospitalManagement.Application.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MedicalRecordController : ControllerBase
{
    private readonly IMedicalRecordService _medicalRecordService;

    public MedicalRecordController(IMedicalRecordService medicalRecordService)
    {
        _medicalRecordService = medicalRecordService;
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Doctor")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _medicalRecordService.GetAllAsync();
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _medicalRecordService.GetByIdAsync(id);
        return result.Success ? Ok(result) : NotFound(result);
    }

    [HttpGet("patient/{patientId:guid}")]
    public async Task<IActionResult> GetByPatient(Guid patientId)
    {
        var result = await _medicalRecordService.GetByPatientIdAsync(patientId);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpGet("doctor/{doctorId:guid}")]
    [Authorize(Roles = "Admin,Doctor")]
    public async Task<IActionResult> GetByDoctor(Guid doctorId)
    {
        var result = await _medicalRecordService.GetByDoctorIdAsync(doctorId);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpGet("appointment/{appointmentId:guid}")]
    public async Task<IActionResult> GetByAppointment(Guid appointmentId)
    {
        var result = await _medicalRecordService.GetByAppointmentIdAsync(appointmentId);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Doctor")]
    public async Task<IActionResult> Create([FromBody] CreateMedicalRecordDto dto)
    {
        var validator  = new CreateMedicalRecordValidator();
        var validation = await validator.ValidateAsync(dto);
        if (!validation.IsValid)
            return BadRequest(validation.Errors.Select(e => e.ErrorMessage));

        var result = await _medicalRecordService.CreateAsync(dto);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin,Doctor")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateMedicalRecordDto dto)
    {
        var result = await _medicalRecordService.UpdateAsync(id, dto);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _medicalRecordService.DeleteAsync(id);
        return result.Success ? Ok(result) : BadRequest(result);
    }
}
