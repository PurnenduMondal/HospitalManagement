using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HospitalManagement.Domain.Enums;

namespace HospitalManagement.Domain.Entities;

public class Patient : BaseEntity
{
    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string LastName  { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    [EmailAddress]
    public string Email     { get; set; } = string.Empty;

    [Required]
    [MaxLength(15)]
    [Phone]
    public string Phone     { get; set; } = string.Empty;

    [Required]
    public DateTime DateOfBirth { get; set; }

    [Required]
    public Gender Gender    { get; set; }

    [Required]
    [MaxLength(300)]
    public string Address   { get; set; } = string.Empty;

    [Required]
    [MaxLength(5)]
    public string BloodGroup { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? EmergencyContactName  { get; set; }

    [MaxLength(15)]
    [Phone]
    public string? EmergencyContactPhone { get; set; }

    // Computed
    [NotMapped]
    public string FullName => $"{FirstName} {LastName}";
}
