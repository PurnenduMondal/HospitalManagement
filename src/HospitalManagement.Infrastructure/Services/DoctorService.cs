using HospitalManagement.Application.Common;
using HospitalManagement.Application.DTOs.Doctor;
using HospitalManagement.Application.Interfaces;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Interfaces;

namespace HospitalManagement.Infrastructure.Services;

public class DoctorService : IDoctorService
{
    private readonly IUnitOfWork _unitOfWork;

    public DoctorService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<BaseResponse<IEnumerable<DoctorDto>>> GetAllAsync()
    {
        var doctors = await _unitOfWork.Repository<Doctor>().GetAllAsync();
        var result  = doctors.Where(d => !d.IsDeleted).Select(MapToDto);
        return BaseResponse<IEnumerable<DoctorDto>>.Ok(result);
    }

    public async Task<BaseResponse<DoctorDto>> GetByIdAsync(Guid id)
    {
        var doctor = await _unitOfWork.Repository<Doctor>().GetByIdAsync(id);
        if (doctor == null || doctor.IsDeleted)
            return BaseResponse<DoctorDto>.Fail("Doctor not found.");

        return BaseResponse<DoctorDto>.Ok(MapToDto(doctor));
    }

    public async Task<BaseResponse<DoctorDto>> CreateAsync(CreateDoctorDto dto)
    {
        var emailExists = await _unitOfWork.Repository<Doctor>()
            .ExistsAsync(d => d.Email == dto.Email && !d.IsDeleted);
        if (emailExists)
            return BaseResponse<DoctorDto>.Fail("A doctor with this email already exists.");

        var licenseExists = await _unitOfWork.Repository<Doctor>()
            .ExistsAsync(d => d.LicenseNumber == dto.LicenseNumber && !d.IsDeleted);
        if (licenseExists)
            return BaseResponse<DoctorDto>.Fail("A doctor with this license number already exists.");

        var doctor = new Doctor
        {
            FirstName       = dto.FirstName,
            LastName        = dto.LastName,
            Email           = dto.Email,
            Phone           = dto.Phone,
            Specialization  = dto.Specialization,
            Qualification   = dto.Qualification,
            LicenseNumber   = dto.LicenseNumber,
            ExperienceYears = dto.ExperienceYears,
            ConsultationFee = dto.ConsultationFee,
            Gender          = dto.Gender,
            Address         = dto.Address,
            IsAvailable     = true
        };

        await _unitOfWork.Repository<Doctor>().AddAsync(doctor);
        await _unitOfWork.SaveChangesAsync();

        return BaseResponse<DoctorDto>.Ok(MapToDto(doctor), "Doctor created successfully.");
    }

    public async Task<BaseResponse<DoctorDto>> UpdateAsync(Guid id, UpdateDoctorDto dto)
    {
        var doctor = await _unitOfWork.Repository<Doctor>().GetByIdAsync(id);
        if (doctor == null || doctor.IsDeleted)
            return BaseResponse<DoctorDto>.Fail("Doctor not found.");

        doctor.FirstName       = dto.FirstName;
        doctor.LastName        = dto.LastName;
        doctor.Email           = dto.Email;
        doctor.Phone           = dto.Phone;
        doctor.Specialization  = dto.Specialization;
        doctor.Qualification   = dto.Qualification;
        doctor.LicenseNumber   = dto.LicenseNumber;
        doctor.ExperienceYears = dto.ExperienceYears;
        doctor.ConsultationFee = dto.ConsultationFee;
        doctor.Gender          = dto.Gender;
        doctor.Address         = dto.Address;
        doctor.IsAvailable     = dto.IsAvailable;
        doctor.UpdatedAt       = DateTime.UtcNow;

        _unitOfWork.Repository<Doctor>().Update(doctor);
        await _unitOfWork.SaveChangesAsync();

        return BaseResponse<DoctorDto>.Ok(MapToDto(doctor), "Doctor updated successfully.");
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id)
    {
        var doctor = await _unitOfWork.Repository<Doctor>().GetByIdAsync(id);
        if (doctor == null || doctor.IsDeleted)
            return BaseResponse<bool>.Fail("Doctor not found.");

        doctor.IsDeleted = true;
        doctor.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Repository<Doctor>().Update(doctor);
        await _unitOfWork.SaveChangesAsync();

        return BaseResponse<bool>.Ok(true, "Doctor deleted successfully.");
    }

    public async Task<BaseResponse<IEnumerable<DoctorDto>>> SearchAsync(string keyword)
    {
        var kw = keyword.ToLower();
        var doctors = await _unitOfWork.Repository<Doctor>()
            .FindAsync(d => !d.IsDeleted && (
                d.FirstName.ToLower().Contains(kw) ||
                d.LastName.ToLower().Contains(kw)  ||
                d.Email.ToLower().Contains(kw)     ||
                d.Specialization.ToLower().Contains(kw)));

        return BaseResponse<IEnumerable<DoctorDto>>.Ok(doctors.Select(MapToDto));
    }

    public async Task<BaseResponse<IEnumerable<DoctorDto>>> GetBySpecializationAsync(string specialization)
    {
        var doctors = await _unitOfWork.Repository<Doctor>()
            .FindAsync(d => !d.IsDeleted &&
                d.Specialization.ToLower() == specialization.ToLower() &&
                d.IsAvailable);

        return BaseResponse<IEnumerable<DoctorDto>>.Ok(doctors.Select(MapToDto));
    }

    private static DoctorDto MapToDto(Doctor d) => new()
    {
        Id              = d.Id,
        FirstName       = d.FirstName,
        LastName        = d.LastName,
        FullName        = d.FullName,
        Email           = d.Email,
        Phone           = d.Phone,
        Specialization  = d.Specialization,
        Qualification   = d.Qualification,
        LicenseNumber   = d.LicenseNumber,
        ExperienceYears = d.ExperienceYears,
        ConsultationFee = d.ConsultationFee,
        Gender          = d.Gender,
        Address         = d.Address,
        IsAvailable     = d.IsAvailable,
        CreatedAt       = d.CreatedAt
    };
}
