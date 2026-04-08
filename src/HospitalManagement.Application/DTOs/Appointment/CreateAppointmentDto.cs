using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Application.DTOs.Appointment;

public class CreateAppointmentDto
{
    [Required]
    public Guid PatientId { get; set; }

    [Required]
    public Guid DoctorId { get; set; }

    [Required]
    public DateTime AppointmentDate { get; set; }

    [Required]
    public TimeSpan StartTime { get; set; }

    [Required]
    public TimeSpan EndTime { get; set; }

    [MaxLength(1000)]
    public string? Notes { get; set; }
}
