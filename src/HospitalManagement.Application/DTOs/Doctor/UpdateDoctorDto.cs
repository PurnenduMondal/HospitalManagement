using HospitalManagement.Domain.Enums;

namespace HospitalManagement.Application.DTOs.Doctor;

public class UpdateDoctorDto
{
    public string FirstName      { get; set; } = string.Empty;
    public string LastName       { get; set; } = string.Empty;
    public string Email          { get; set; } = string.Empty;
    public string Phone          { get; set; } = string.Empty;
    public string Specialization { get; set; } = string.Empty;
    public string Qualification  { get; set; } = string.Empty;
    public string LicenseNumber  { get; set; } = string.Empty;
    public int    ExperienceYears  { get; set; }
    public decimal ConsultationFee { get; set; }
    public Gender Gender         { get; set; }
    public string? Address       { get; set; }
    public bool   IsAvailable    { get; set; }
}
