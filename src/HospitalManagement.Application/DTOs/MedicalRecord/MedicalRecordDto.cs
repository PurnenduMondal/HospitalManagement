namespace HospitalManagement.Application.DTOs.MedicalRecord;

public class MedicalRecordDto
{
    public Guid      Id             { get; set; }
    public Guid      PatientId      { get; set; }
    public string    PatientName    { get; set; } = string.Empty;
    public Guid      DoctorId       { get; set; }
    public string    DoctorName     { get; set; } = string.Empty;
    public string    Specialization { get; set; } = string.Empty;
    public Guid?     AppointmentId  { get; set; }
    public DateTime  RecordDate     { get; set; }
    public string    Diagnosis      { get; set; } = string.Empty;
    public string    Symptoms       { get; set; } = string.Empty;
    public string    Treatment      { get; set; } = string.Empty;
    public string?   Prescription   { get; set; }
    public string?   LabResults     { get; set; }
    public string?   Notes          { get; set; }
    public string?   BloodPressure  { get; set; }
    public decimal?  Temperature    { get; set; }
    public decimal?  Weight         { get; set; }
    public decimal?  Height         { get; set; }
    public DateTime  CreatedAt      { get; set; }
    public DateTime? UpdatedAt      { get; set; }
}
