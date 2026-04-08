using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Application.DTOs.MedicalRecord;

public class CreateMedicalRecordDto
{
    [Required]
    public Guid PatientId { get; set; }

    [Required]
    public Guid DoctorId { get; set; }

    public Guid? AppointmentId { get; set; }

    [Required]
    public DateTime RecordDate { get; set; }

    [Required]
    [MaxLength(1000)]
    public string Diagnosis { get; set; } = string.Empty;

    [Required]
    [MaxLength(1000)]
    public string Symptoms { get; set; } = string.Empty;

    [Required]
    [MaxLength(2000)]
    public string Treatment { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string? Prescription { get; set; }

    [MaxLength(2000)]
    public string? LabResults { get; set; }

    [MaxLength(2000)]
    public string? Notes { get; set; }

    [MaxLength(20)]
    public string? BloodPressure { get; set; }

    [Range(30.0, 45.0, ErrorMessage = "Temperature must be between 30°C and 45°C.")]
    public decimal? Temperature { get; set; }

    [Range(0.5, 700.0, ErrorMessage = "Weight must be between 0.5 kg and 700 kg.")]
    public decimal? Weight { get; set; }

    [Range(20.0, 300.0, ErrorMessage = "Height must be between 20 cm and 300 cm.")]
    public decimal? Height { get; set; }
}
