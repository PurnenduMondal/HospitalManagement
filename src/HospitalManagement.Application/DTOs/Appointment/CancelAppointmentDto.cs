using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Application.DTOs.Appointment;

public class CancelAppointmentDto
{
    [Required(ErrorMessage = "Cancellation reason is required.")]
    [MaxLength(500)]
    public string Reason { get; set; } = string.Empty;
}
