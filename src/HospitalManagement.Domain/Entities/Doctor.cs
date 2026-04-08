using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HospitalManagement.Domain.Enums;

namespace HospitalManagement.Domain.Entities;

public class Doctor : BaseEntity
{
    [Required]
    [MaxLength(50)]
    public string FirstName      { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string LastName       { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    [EmailAddress]
    public string Email          { get; set; } = string.Empty;

    [Required]
    [MaxLength(15)]
    [Phone]
    public string Phone          { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Specialization { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Qualification  { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string LicenseNumber  { get; set; } = string.Empty;

    [Range(0, 60)]
    public int ExperienceYears   { get; set; }

    [Required]
    [Column(TypeName = "decimal(10,2)")]
    [Range(0.01, double.MaxValue)]
    public decimal ConsultationFee { get; set; }

    [Required]
    public Gender Gender         { get; set; }

    [MaxLength(300)]
    public string? Address       { get; set; }

    public bool IsAvailable      { get; set; } = true;

    // Computed
    [NotMapped]
    public string FullName => $"{FirstName} {LastName}";
}
