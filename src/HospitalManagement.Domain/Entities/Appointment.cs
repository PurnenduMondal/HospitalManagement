using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HospitalManagement.Domain.Enums;

namespace HospitalManagement.Domain.Entities;

public class Appointment : BaseEntity
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

    [Required]
    public AppointmentStatus Status { get; set; } = AppointmentStatus.Scheduled;

    [MaxLength(1000)]
    public string? Notes { get; set; }

    [MaxLength(500)]
    public string? CancelReason { get; set; }

    [Required]
    [Column(TypeName = "decimal(10,2)")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Consultation fee must be greater than 0.")]
    public decimal ConsultationFee { get; set; }

    // Navigation properties
    public Patient Patient { get; set; } = null!;
    public Doctor  Doctor  { get; set; } = null!;
}
