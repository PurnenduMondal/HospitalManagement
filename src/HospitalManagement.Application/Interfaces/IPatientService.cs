using HospitalManagement.Application.Common;
using HospitalManagement.Application.DTOs.Patient;

namespace HospitalManagement.Application.Interfaces;

public interface IPatientService
{
    Task<BaseResponse<IEnumerable<PatientDto>>> GetAllAsync();
    Task<BaseResponse<PatientDto>> GetByIdAsync(int id);
    Task<BaseResponse<PatientDto>> CreateAsync(CreatePatientDto dto);
    Task<BaseResponse<PatientDto>> UpdateAsync(int id, UpdatePatientDto dto);
    Task<BaseResponse<bool>> DeleteAsync(int id);
    Task<BaseResponse<IEnumerable<PatientDto>>> SearchAsync(string keyword);
}
