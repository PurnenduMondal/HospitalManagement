using HospitalManagement.Application.Common;
using HospitalManagement.Application.DTOs.Appointment;
using HospitalManagement.Application.Interfaces;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Enums;
using HospitalManagement.Domain.Interfaces;
using HospitalManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Infrastructure.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IUnitOfWork  _unitOfWork;
    private readonly AppDbContext _context;

    public AppointmentService(IUnitOfWork unitOfWork, AppDbContext context)
    {
        _unitOfWork = unitOfWork;
        _context    = context;
    }

    public async Task<BaseResponse<IEnumerable<AppointmentDto>>> GetAllAsync()
    {
        var appointments = await _context.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Doctor)
            .Where(a => !a.IsDeleted)
            .OrderByDescending(a => a.AppointmentDate)
            .ToListAsync();

        return BaseResponse<IEnumerable<AppointmentDto>>.Ok(appointments.Select(MapToDto));
    }

    public async Task<BaseResponse<AppointmentDto>> GetByIdAsync(Guid id)
    {
        var appointment = await _context.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Doctor)
            .FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted);

        if (appointment == null)
            return BaseResponse<AppointmentDto>.Fail("Appointment not found.");

        return BaseResponse<AppointmentDto>.Ok(MapToDto(appointment));
    }

    public async Task<BaseResponse<IEnumerable<AppointmentDto>>> GetByPatientIdAsync(Guid patientId)
    {
        var appointments = await _context.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Doctor)
            .Where(a => a.PatientId == patientId && !a.IsDeleted)
            .OrderByDescending(a => a.AppointmentDate)
            .ToListAsync();

        return BaseResponse<IEnumerable<AppointmentDto>>.Ok(appointments.Select(MapToDto));
    }

    public async Task<BaseResponse<IEnumerable<AppointmentDto>>> GetByDoctorIdAsync(Guid doctorId)
    {
        var appointments = await _context.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Doctor)
            .Where(a => a.DoctorId == doctorId && !a.IsDeleted)
            .OrderByDescending(a => a.AppointmentDate)
            .ToListAsync();

        return BaseResponse<IEnumerable<AppointmentDto>>.Ok(appointments.Select(MapToDto));
    }

    public async Task<BaseResponse<IEnumerable<AppointmentDto>>> GetByDateAsync(DateTime date)
    {
        var appointments = await _context.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Doctor)
            .Where(a => a.AppointmentDate.Date == date.Date && !a.IsDeleted)
            .OrderBy(a => a.StartTime)
            .ToListAsync();

        return BaseResponse<IEnumerable<AppointmentDto>>.Ok(appointments.Select(MapToDto));
    }

    public async Task<BaseResponse<AppointmentDto>> CreateAsync(CreateAppointmentDto dto)
    {
        var patient = await _unitOfWork.Repository<Patient>().GetByIdAsync(dto.PatientId);
        if (patient == null || patient.IsDeleted)
            return BaseResponse<AppointmentDto>.Fail("Patient not found.");

        var doctor = await _unitOfWork.Repository<Doctor>().GetByIdAsync(dto.DoctorId);
        if (doctor == null || doctor.IsDeleted)
            return BaseResponse<AppointmentDto>.Fail("Doctor not found.");
        if (!doctor.IsAvailable)
            return BaseResponse<AppointmentDto>.Fail("Doctor is not available.");

        var hasConflict = await _context.Appointments
            .AnyAsync(a =>
                a.DoctorId              == dto.DoctorId &&
                a.AppointmentDate.Date  == dto.AppointmentDate.Date &&
                a.Status                != AppointmentStatus.Cancelled &&
                !a.IsDeleted            &&
                a.StartTime             <  dto.EndTime &&
                a.EndTime               >  dto.StartTime);

        if (hasConflict)
            return BaseResponse<AppointmentDto>.Fail(
                "Doctor already has an appointment in this time slot.");

        var appointment = new Appointment
        {
            PatientId       = dto.PatientId,
            DoctorId        = dto.DoctorId,
            AppointmentDate = dto.AppointmentDate,
            StartTime       = dto.StartTime,
            EndTime         = dto.EndTime,
            Notes           = dto.Notes,
            Status          = AppointmentStatus.Scheduled,
            ConsultationFee = doctor.ConsultationFee
        };

        await _unitOfWork.Repository<Appointment>().AddAsync(appointment);
        await _unitOfWork.SaveChangesAsync();

        return await GetByIdAsync(appointment.Id);
    }

    public async Task<BaseResponse<AppointmentDto>> UpdateAsync(Guid id, UpdateAppointmentDto dto)
    {
        var appointment = await _unitOfWork.Repository<Appointment>().GetByIdAsync(id);
        if (appointment == null || appointment.IsDeleted)
            return BaseResponse<AppointmentDto>.Fail("Appointment not found.");

        if (appointment.Status == AppointmentStatus.Cancelled ||
            appointment.Status == AppointmentStatus.Completed)
            return BaseResponse<AppointmentDto>.Fail(
                $"Cannot update a {appointment.Status} appointment.");

        var hasConflict = await _context.Appointments
            .AnyAsync(a =>
                a.Id                    != id &&
                a.DoctorId              == appointment.DoctorId &&
                a.AppointmentDate.Date  == dto.AppointmentDate.Date &&
                a.Status                != AppointmentStatus.Cancelled &&
                !a.IsDeleted            &&
                a.StartTime             <  dto.EndTime &&
                a.EndTime               >  dto.StartTime);

        if (hasConflict)
            return BaseResponse<AppointmentDto>.Fail(
                "Doctor already has an appointment in this time slot.");

        appointment.AppointmentDate = dto.AppointmentDate;
        appointment.StartTime       = dto.StartTime;
        appointment.EndTime         = dto.EndTime;
        appointment.Notes           = dto.Notes;
        appointment.UpdatedAt       = DateTime.UtcNow;

        _unitOfWork.Repository<Appointment>().Update(appointment);
        await _unitOfWork.SaveChangesAsync();

        return await GetByIdAsync(id);
    }

    public async Task<BaseResponse<AppointmentDto>> CancelAsync(Guid id, CancelAppointmentDto dto)
    {
        var appointment = await _unitOfWork.Repository<Appointment>().GetByIdAsync(id);
        if (appointment == null || appointment.IsDeleted)
            return BaseResponse<AppointmentDto>.Fail("Appointment not found.");

        if (appointment.Status == AppointmentStatus.Completed)
            return BaseResponse<AppointmentDto>.Fail("Cannot cancel a completed appointment.");

        if (appointment.Status == AppointmentStatus.Cancelled)
            return BaseResponse<AppointmentDto>.Fail("Appointment is already cancelled.");

        appointment.Status       = AppointmentStatus.Cancelled;
        appointment.CancelReason = dto.Reason;
        appointment.UpdatedAt    = DateTime.UtcNow;

        _unitOfWork.Repository<Appointment>().Update(appointment);
        await _unitOfWork.SaveChangesAsync();

        return await GetByIdAsync(id);
    }

    public async Task<BaseResponse<AppointmentDto>> ConfirmAsync(Guid id)
    {
        var appointment = await _unitOfWork.Repository<Appointment>().GetByIdAsync(id);
        if (appointment == null || appointment.IsDeleted)
            return BaseResponse<AppointmentDto>.Fail("Appointment not found.");

        if (appointment.Status != AppointmentStatus.Scheduled)
            return BaseResponse<AppointmentDto>.Fail(
                "Only scheduled appointments can be confirmed.");

        appointment.Status    = AppointmentStatus.Confirmed;
        appointment.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Repository<Appointment>().Update(appointment);
        await _unitOfWork.SaveChangesAsync();

        return await GetByIdAsync(id);
    }

    public async Task<BaseResponse<AppointmentDto>> CompleteAsync(Guid id)
    {
        var appointment = await _unitOfWork.Repository<Appointment>().GetByIdAsync(id);
        if (appointment == null || appointment.IsDeleted)
            return BaseResponse<AppointmentDto>.Fail("Appointment not found.");

        if (appointment.Status == AppointmentStatus.Cancelled)
            return BaseResponse<AppointmentDto>.Fail("Cannot complete a cancelled appointment.");

        if (appointment.Status == AppointmentStatus.Completed)
            return BaseResponse<AppointmentDto>.Fail("Appointment is already completed.");

        appointment.Status    = AppointmentStatus.Completed;
        appointment.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Repository<Appointment>().Update(appointment);
        await _unitOfWork.SaveChangesAsync();

        return await GetByIdAsync(id);
    }

    private static AppointmentDto MapToDto(Appointment a) => new()
    {
        Id              = a.Id,
        PatientId       = a.PatientId,
        PatientName     = a.Patient?.FullName     ?? string.Empty,
        DoctorId        = a.DoctorId,
        DoctorName      = a.Doctor?.FullName       ?? string.Empty,
        Specialization  = a.Doctor?.Specialization ?? string.Empty,
        AppointmentDate = a.AppointmentDate,
        StartTime       = a.StartTime,
        EndTime         = a.EndTime,
        Status          = a.Status,
        Notes           = a.Notes,
        CancelReason    = a.CancelReason,
        ConsultationFee = a.ConsultationFee,
        CreatedAt       = a.CreatedAt
    };
}
