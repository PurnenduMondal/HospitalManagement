using HospitalManagement.Application.Common;
using HospitalManagement.Application.DTOs.Patient;
using HospitalManagement.Application.Interfaces;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Interfaces;

namespace HospitalManagement.Infrastructure.Services;

public class PatientService : IPatientService
{
    private readonly IUnitOfWork _unitOfWork;

    public PatientService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<BaseResponse<IEnumerable<PatientDto>>> GetAllAsync()
    {
        var patients = await _unitOfWork.Repository<Patient>().GetAllAsync();
        var result   = patients.Where(p => !p.IsDeleted).Select(MapToDto);
        return BaseResponse<IEnumerable<PatientDto>>.Ok(result);
    }

    public async Task<BaseResponse<PatientDto>> GetByIdAsync(int id)
    {
        var patient = await _unitOfWork.Repository<Patient>().GetByIdAsync(id);
        if (patient == null || patient.IsDeleted)
            return BaseResponse<PatientDto>.Fail("Patient not found.");

        return BaseResponse<PatientDto>.Ok(MapToDto(patient));
    }

    public async Task<BaseResponse<PatientDto>> CreateAsync(CreatePatientDto dto)
    {
        // Check duplicate email
        var exists = await _unitOfWork.Repository<Patient>()
            .ExistsAsync(p => p.Email == dto.Email && !p.IsDeleted);
        if (exists)
            return BaseResponse<PatientDto>.Fail("A patient with this email already exists.");

        var patient = new Patient
        {
            FirstName            = dto.FirstName,
            LastName             = dto.LastName,
            Email                = dto.Email,
            Phone                = dto.Phone,
            DateOfBirth          = dto.DateOfBirth,
            Gender               = dto.Gender,
            Address              = dto.Address,
            BloodGroup           = dto.BloodGroup,
            EmergencyContactName = dto.EmergencyContactName,
            EmergencyContactPhone= dto.EmergencyContactPhone
        };

        await _unitOfWork.Repository<Patient>().AddAsync(patient);
        await _unitOfWork.SaveChangesAsync();

        return BaseResponse<PatientDto>.Ok(MapToDto(patient), "Patient created successfully.");
    }

    public async Task<BaseResponse<PatientDto>> UpdateAsync(int id, UpdatePatientDto dto)
    {
        var patient = await _unitOfWork.Repository<Patient>().GetByIdAsync(id);
        if (patient == null || patient.IsDeleted)
            return BaseResponse<PatientDto>.Fail("Patient not found.");

        patient.FirstName             = dto.FirstName;
        patient.LastName              = dto.LastName;
        patient.Email                 = dto.Email;
        patient.Phone                 = dto.Phone;
        patient.Gender                = dto.Gender;
        patient.Address               = dto.Address;
        patient.BloodGroup            = dto.BloodGroup;
        patient.EmergencyContactName  = dto.EmergencyContactName;
        patient.EmergencyContactPhone = dto.EmergencyContactPhone;
        patient.UpdatedAt             = DateTime.UtcNow;

        _unitOfWork.Repository<Patient>().Update(patient);
        await _unitOfWork.SaveChangesAsync();

        return BaseResponse<PatientDto>.Ok(MapToDto(patient), "Patient updated successfully.");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(int id)
    {
        var patient = await _unitOfWork.Repository<Patient>().GetByIdAsync(id);
        if (patient == null || patient.IsDeleted)
            return BaseResponse<bool>.Fail("Patient not found.");

        // Soft delete
        patient.IsDeleted = true;
        patient.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Repository<Patient>().Update(patient);
        await _unitOfWork.SaveChangesAsync();

        return BaseResponse<bool>.Ok(true, "Patient deleted successfully.");
    }

    public async Task<BaseResponse<IEnumerable<PatientDto>>> SearchAsync(string keyword)
    {
        var keyword_lower = keyword.ToLower();
        var patients = await _unitOfWork.Repository<Patient>()
            .FindAsync(p => !p.IsDeleted && (
                p.FirstName.ToLower().Contains(keyword_lower) ||
                p.LastName.ToLower().Contains(keyword_lower)  ||
                p.Email.ToLower().Contains(keyword_lower)     ||
                p.Phone.Contains(keyword)));

        return BaseResponse<IEnumerable<PatientDto>>.Ok(patients.Select(MapToDto));
    }

    private static PatientDto MapToDto(Patient p) => new()
    {
        Id                   = p.Id,
        FirstName            = p.FirstName,
        LastName             = p.LastName,
        FullName             = p.FullName,
        Email                = p.Email,
        Phone                = p.Phone,
        DateOfBirth          = p.DateOfBirth,
        Gender               = p.Gender,
        Address              = p.Address,
        BloodGroup           = p.BloodGroup,
        EmergencyContactName = p.EmergencyContactName,
        EmergencyContactPhone= p.EmergencyContactPhone,
        CreatedAt            = p.CreatedAt
    };
}
