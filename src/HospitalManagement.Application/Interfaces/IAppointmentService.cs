using HospitalManagement.Application.Common;
using HospitalManagement.Application.DTOs.Appointment;

namespace HospitalManagement.Application.Interfaces;

public interface IAppointmentService
{
    Task<BaseResponse<IEnumerable<AppointmentDto>>> GetAllAsync();
    Task<BaseResponse<AppointmentDto>>              GetByIdAsync(Guid id);
    Task<BaseResponse<IEnumerable<AppointmentDto>>> GetByPatientIdAsync(Guid patientId);
    Task<BaseResponse<IEnumerable<AppointmentDto>>> GetByDoctorIdAsync(Guid doctorId);
    Task<BaseResponse<IEnumerable<AppointmentDto>>> GetByDateAsync(DateTime date);
    Task<BaseResponse<AppointmentDto>>              CreateAsync(CreateAppointmentDto dto);
    Task<BaseResponse<AppointmentDto>>              UpdateAsync(Guid id, UpdateAppointmentDto dto);
    Task<BaseResponse<AppointmentDto>>              CancelAsync(Guid id, CancelAppointmentDto dto);
    Task<BaseResponse<AppointmentDto>>              CompleteAsync(Guid id);
    Task<BaseResponse<AppointmentDto>>              ConfirmAsync(Guid id);
}
