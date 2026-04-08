using HospitalManagement.Application.Common;
using HospitalManagement.Application.DTOs.Doctor;

namespace HospitalManagement.Application.Interfaces;

public interface IDoctorService
{
    Task<BaseResponse<IEnumerable<DoctorDto>>> GetAllAsync();
    Task<BaseResponse<DoctorDto>>              GetByIdAsync(Guid id);
    Task<BaseResponse<DoctorDto>>              CreateAsync(CreateDoctorDto dto);
    Task<BaseResponse<DoctorDto>>              UpdateAsync(Guid id, UpdateDoctorDto dto);
    Task<BaseResponse<bool>>                   DeleteAsync(Guid id);
    Task<BaseResponse<IEnumerable<DoctorDto>>> SearchAsync(string keyword);
    Task<BaseResponse<IEnumerable<DoctorDto>>> GetBySpecializationAsync(string specialization);
}
