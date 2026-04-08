using HospitalManagement.Domain.Enums;

namespace HospitalManagement.Application.DTOs.Patient;

public class CreatePatientDto
{
    public string FirstName  { get; set; } = string.Empty;
    public string LastName   { get; set; } = string.Empty;
    public string Email      { get; set; } = string.Empty;
    public string Phone      { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public Gender Gender     { get; set; }
    public string Address    { get; set; } = string.Empty;
    public string BloodGroup { get; set; } = string.Empty;
    public string? EmergencyContactName  { get; set; }
    public string? EmergencyContactPhone { get; set; }
}
