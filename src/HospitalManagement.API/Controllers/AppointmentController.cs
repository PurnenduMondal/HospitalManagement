using HospitalManagement.Application.DTOs.Appointment;
using HospitalManagement.Application.Interfaces;
using HospitalManagement.Application.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AppointmentController : ControllerBase
{
    private readonly IAppointmentService _appointmentService;

    public AppointmentController(IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _appointmentService.GetAllAsync();
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _appointmentService.GetByIdAsync(id);
        return result.Success ? Ok(result) : NotFound(result);
    }

    [HttpGet("patient/{patientId:guid}")]
    public async Task<IActionResult> GetByPatient(Guid patientId)
    {
        var result = await _appointmentService.GetByPatientIdAsync(patientId);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpGet("doctor/{doctorId:guid}")]
    public async Task<IActionResult> GetByDoctor(Guid doctorId)
    {
        var result = await _appointmentService.GetByDoctorIdAsync(doctorId);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpGet("date/{date}")]
    public async Task<IActionResult> GetByDate(DateTime date)
    {
        var result = await _appointmentService.GetByDateAsync(date);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Doctor,Nurse")]
    public async Task<IActionResult> Create([FromBody] CreateAppointmentDto dto)
    {
        var validator  = new CreateAppointmentValidator();
        var validation = await validator.ValidateAsync(dto);
        if (!validation.IsValid)
            return BadRequest(validation.Errors.Select(e => e.ErrorMessage));

        var result = await _appointmentService.CreateAsync(dto);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin,Doctor,Nurse")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateAppointmentDto dto)
    {
        var result = await _appointmentService.UpdateAsync(id, dto);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPatch("{id:guid}/confirm")]
    [Authorize(Roles = "Admin,Doctor,Nurse")]
    public async Task<IActionResult> Confirm(Guid id)
    {
        var result = await _appointmentService.ConfirmAsync(id);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPatch("{id:guid}/complete")]
    [Authorize(Roles = "Admin,Doctor")]
    public async Task<IActionResult> Complete(Guid id)
    {
        var result = await _appointmentService.CompleteAsync(id);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPatch("{id:guid}/cancel")]
    public async Task<IActionResult> Cancel(Guid id, [FromBody] CancelAppointmentDto dto)
    {
        var result = await _appointmentService.CancelAsync(id, dto);
        return result.Success ? Ok(result) : BadRequest(result);
    }
}
