using HospitalManagement.Application.Common;
using HospitalManagement.Application.DTOs.MedicalRecord;
using HospitalManagement.Application.Interfaces;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Interfaces;
using HospitalManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Infrastructure.Services;

public class MedicalRecordService : IMedicalRecordService
{
    private readonly IUnitOfWork  _unitOfWork;
    private readonly AppDbContext _context;

    public MedicalRecordService(IUnitOfWork unitOfWork, AppDbContext context)
    {
        _unitOfWork = unitOfWork;
        _context    = context;
    }

    public async Task<BaseResponse<IEnumerable<MedicalRecordDto>>> GetAllAsync()
    {
        var records = await _context.MedicalRecords
            .Include(r => r.Patient)
            .Include(r => r.Doctor)
            .Where(r => !r.IsDeleted)
            .OrderByDescending(r => r.RecordDate)
            .ToListAsync();

        return BaseResponse<IEnumerable<MedicalRecordDto>>.Ok(records.Select(MapToDto));
    }

    public async Task<BaseResponse<MedicalRecordDto>> GetByIdAsync(Guid id)
    {
        var record = await _context.MedicalRecords
            .Include(r => r.Patient)
            .Include(r => r.Doctor)
            .FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted);

        if (record == null)
            return BaseResponse<MedicalRecordDto>.Fail("Medical record not found.");

        return BaseResponse<MedicalRecordDto>.Ok(MapToDto(record));
    }

    public async Task<BaseResponse<IEnumerable<MedicalRecordDto>>> GetByPatientIdAsync(Guid patientId)
    {
        var records = await _context.MedicalRecords
            .Include(r => r.Patient)
            .Include(r => r.Doctor)
            .Where(r => r.PatientId == patientId && !r.IsDeleted)
            .OrderByDescending(r => r.RecordDate)
            .ToListAsync();

        return BaseResponse<IEnumerable<MedicalRecordDto>>.Ok(records.Select(MapToDto));
    }

    public async Task<BaseResponse<IEnumerable<MedicalRecordDto>>> GetByDoctorIdAsync(Guid doctorId)
    {
        var records = await _context.MedicalRecords
            .Include(r => r.Patient)
            .Include(r => r.Doctor)
            .Where(r => r.DoctorId == doctorId && !r.IsDeleted)
            .OrderByDescending(r => r.RecordDate)
            .ToListAsync();

        return BaseResponse<IEnumerable<MedicalRecordDto>>.Ok(records.Select(MapToDto));
    }

    public async Task<BaseResponse<IEnumerable<MedicalRecordDto>>> GetByAppointmentIdAsync(Guid appointmentId)
    {
        var records = await _context.MedicalRecords
            .Include(r => r.Patient)
            .Include(r => r.Doctor)
            .Where(r => r.AppointmentId == appointmentId && !r.IsDeleted)
            .OrderByDescending(r => r.RecordDate)
            .ToListAsync();

        return BaseResponse<IEnumerable<MedicalRecordDto>>.Ok(records.Select(MapToDto));
    }

    public async Task<BaseResponse<MedicalRecordDto>> CreateAsync(CreateMedicalRecordDto dto)
    {
        var patient = await _unitOfWork.Repository<Patient>().GetByIdAsync(dto.PatientId);
        if (patient == null || patient.IsDeleted)
            return BaseResponse<MedicalRecordDto>.Fail("Patient not found.");

        var doctor = await _unitOfWork.Repository<Doctor>().GetByIdAsync(dto.DoctorId);
        if (doctor == null || doctor.IsDeleted)
            return BaseResponse<MedicalRecordDto>.Fail("Doctor not found.");

        if (dto.AppointmentId.HasValue)
        {
            var appointment = await _unitOfWork.Repository<Appointment>()
                .GetByIdAsync(dto.AppointmentId.Value);
            if (appointment == null || appointment.IsDeleted)
                return BaseResponse<MedicalRecordDto>.Fail("Appointment not found.");
        }

        var record = new MedicalRecord
        {
            PatientId     = dto.PatientId,
            DoctorId      = dto.DoctorId,
            AppointmentId = dto.AppointmentId,
            RecordDate    = dto.RecordDate,
            Diagnosis     = dto.Diagnosis,
            Symptoms      = dto.Symptoms,
            Treatment     = dto.Treatment,
            Prescription  = dto.Prescription,
            LabResults    = dto.LabResults,
            Notes         = dto.Notes,
            BloodPressure = dto.BloodPressure,
            Temperature   = dto.Temperature,
            Weight        = dto.Weight,
            Height        = dto.Height
        };

        await _unitOfWork.Repository<MedicalRecord>().AddAsync(record);
        await _unitOfWork.SaveChangesAsync();

        return await GetByIdAsync(record.Id);
    }

    public async Task<BaseResponse<MedicalRecordDto>> UpdateAsync(Guid id, UpdateMedicalRecordDto dto)
    {
        var record = await _unitOfWork.Repository<MedicalRecord>().GetByIdAsync(id);
        if (record == null || record.IsDeleted)
            return BaseResponse<MedicalRecordDto>.Fail("Medical record not found.");

        record.RecordDate    = dto.RecordDate;
        record.Diagnosis     = dto.Diagnosis;
        record.Symptoms      = dto.Symptoms;
        record.Treatment     = dto.Treatment;
        record.Prescription  = dto.Prescription;
        record.LabResults    = dto.LabResults;
        record.Notes         = dto.Notes;
        record.BloodPressure = dto.BloodPressure;
        record.Temperature   = dto.Temperature;
        record.Weight        = dto.Weight;
        record.Height        = dto.Height;
        record.UpdatedAt     = DateTime.UtcNow;

        _unitOfWork.Repository<MedicalRecord>().Update(record);
        await _unitOfWork.SaveChangesAsync();

        return await GetByIdAsync(id);
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id)
    {
        var record = await _unitOfWork.Repository<MedicalRecord>().GetByIdAsync(id);
        if (record == null || record.IsDeleted)
            return BaseResponse<bool>.Fail("Medical record not found.");

        record.IsDeleted = true;
        record.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Repository<MedicalRecord>().Update(record);
        await _unitOfWork.SaveChangesAsync();

        return BaseResponse<bool>.Ok(true, "Medical record deleted successfully.");
    }

    private static MedicalRecordDto MapToDto(MedicalRecord r) => new()
    {
        Id             = r.Id,
        PatientId      = r.PatientId,
        PatientName    = r.Patient?.FullName      ?? string.Empty,
        DoctorId       = r.DoctorId,
        DoctorName     = r.Doctor?.FullName        ?? string.Empty,
        Specialization = r.Doctor?.Specialization  ?? string.Empty,
        AppointmentId  = r.AppointmentId,
        RecordDate     = r.RecordDate,
        Diagnosis      = r.Diagnosis,
        Symptoms       = r.Symptoms,
        Treatment      = r.Treatment,
        Prescription   = r.Prescription,
        LabResults     = r.LabResults,
        Notes          = r.Notes,
        BloodPressure  = r.BloodPressure,
        Temperature    = r.Temperature,
        Weight         = r.Weight,
        Height         = r.Height,
        CreatedAt      = r.CreatedAt,
        UpdatedAt      = r.UpdatedAt
    };
}
