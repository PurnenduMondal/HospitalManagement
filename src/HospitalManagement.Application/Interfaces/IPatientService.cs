using HospitalManagement.Application.Common;
using HospitalManagement.Application.DTOs.Patient;

namespace HospitalManagement.Application.Interfaces;

public interface IPatientService
{
    Task<BaseResponse<IEnumerable<PatientDto>>> GetAllAsync();
    Task<BaseResponse<PatientDto>>              GetByIdAsync(Guid id);
    Task<BaseResponse<PatientDto>>              CreateAsync(CreatePatientDto dto);
    Task<BaseResponse<PatientDto>>              UpdateAsync(Guid id, UpdatePatientDto dto);
    Task<BaseResponse<bool>>                    DeleteAsync(Guid id);
    Task<BaseResponse<IEnumerable<PatientDto>>> SearchAsync(string keyword);
}
