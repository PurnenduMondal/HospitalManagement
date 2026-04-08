using HospitalManagement.Domain.Enums;

namespace HospitalManagement.Application.DTOs.Appointment;

public class AppointmentDto
{
    public Guid   Id              { get; set; }
    public Guid   PatientId       { get; set; }
    public string PatientName     { get; set; } = string.Empty;
    public Guid   DoctorId        { get; set; }
    public string DoctorName      { get; set; } = string.Empty;
    public string Specialization  { get; set; } = string.Empty;
    public DateTime AppointmentDate { get; set; }
    public TimeSpan StartTime     { get; set; }
    public TimeSpan EndTime       { get; set; }
    public AppointmentStatus Status { get; set; }
    public string StatusName      => Status.ToString();
    public string? Notes          { get; set; }
    public string? CancelReason   { get; set; }
    public decimal ConsultationFee { get; set; }
    public DateTime CreatedAt     { get; set; }
}
