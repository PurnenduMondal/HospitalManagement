using HospitalManagement.Domain.Enums;

namespace HospitalManagement.Application.DTOs.Patient;

public class PatientDto
{
    public int Id { get; set; }
    public string FirstName  { get; set; } = string.Empty;
    public string LastName   { get; set; } = string.Empty;
    public string FullName   { get; set; } = string.Empty;
    public string Email      { get; set; } = string.Empty;
    public string Phone      { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public int Age => DateTime.Today.Year - DateOfBirth.Year;
    public Gender Gender     { get; set; }
    public string Address    { get; set; } = string.Empty;
    public string BloodGroup { get; set; } = string.Empty;
    public string? EmergencyContactName  { get; set; }
    public string? EmergencyContactPhone { get; set; }
    public DateTime CreatedAt { get; set; }
}
