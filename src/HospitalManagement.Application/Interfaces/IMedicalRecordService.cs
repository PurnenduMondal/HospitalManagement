using HospitalManagement.Application.Common;
using HospitalManagement.Application.DTOs.MedicalRecord;

namespace HospitalManagement.Application.Interfaces;

public interface IMedicalRecordService
{
    Task<BaseResponse<IEnumerable<MedicalRecordDto>>> GetAllAsync();
    Task<BaseResponse<MedicalRecordDto>>              GetByIdAsync(Guid id);
    Task<BaseResponse<IEnumerable<MedicalRecordDto>>> GetByPatientIdAsync(Guid patientId);
    Task<BaseResponse<IEnumerable<MedicalRecordDto>>> GetByDoctorIdAsync(Guid doctorId);
    Task<BaseResponse<IEnumerable<MedicalRecordDto>>> GetByAppointmentIdAsync(Guid appointmentId);
    Task<BaseResponse<MedicalRecordDto>>              CreateAsync(CreateMedicalRecordDto dto);
    Task<BaseResponse<MedicalRecordDto>>              UpdateAsync(Guid id, UpdateMedicalRecordDto dto);
    Task<BaseResponse<bool>>                          DeleteAsync(Guid id);
}
