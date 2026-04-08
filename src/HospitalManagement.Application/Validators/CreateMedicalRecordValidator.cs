using FluentValidation;
using HospitalManagement.Application.DTOs.MedicalRecord;

namespace HospitalManagement.Application.Validators;

public class CreateMedicalRecordValidator : AbstractValidator<CreateMedicalRecordDto>
{
    public CreateMedicalRecordValidator()
    {
        RuleFor(x => x.PatientId)
            .NotEmpty().WithMessage("Valid patient is required.");

        RuleFor(x => x.DoctorId)
            .NotEmpty().WithMessage("Valid doctor is required.");

        RuleFor(x => x.RecordDate)
            .NotEmpty().WithMessage("Record date is required.")
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("Record date cannot be in the future.");

        RuleFor(x => x.Diagnosis)
            .NotEmpty().WithMessage("Diagnosis is required.")
            .MaximumLength(1000).WithMessage("Diagnosis cannot exceed 1000 characters.");

        RuleFor(x => x.Symptoms)
            .NotEmpty().WithMessage("Symptoms are required.")
            .MaximumLength(1000).WithMessage("Symptoms cannot exceed 1000 characters.");

        RuleFor(x => x.Treatment)
            .NotEmpty().WithMessage("Treatment is required.")
            .MaximumLength(2000).WithMessage("Treatment cannot exceed 2000 characters.");

        RuleFor(x => x.Prescription)
            .MaximumLength(2000).WithMessage("Prescription cannot exceed 2000 characters.")
            .When(x => x.Prescription != null);

        RuleFor(x => x.LabResults)
            .MaximumLength(2000).WithMessage("Lab results cannot exceed 2000 characters.")
            .When(x => x.LabResults != null);

        RuleFor(x => x.Notes)
            .MaximumLength(2000).WithMessage("Notes cannot exceed 2000 characters.")
            .When(x => x.Notes != null);

        RuleFor(x => x.BloodPressure)
            .MaximumLength(20).WithMessage("Blood pressure cannot exceed 20 characters.")
            .When(x => x.BloodPressure != null);

        RuleFor(x => x.Temperature)
            .InclusiveBetween(30.0m, 45.0m).WithMessage("Temperature must be between 30°C and 45°C.")
            .When(x => x.Temperature.HasValue);

        RuleFor(x => x.Weight)
            .InclusiveBetween(0.5m, 700.0m).WithMessage("Weight must be between 0.5 kg and 700 kg.")
            .When(x => x.Weight.HasValue);

        RuleFor(x => x.Height)
            .InclusiveBetween(20.0m, 300.0m).WithMessage("Height must be between 20 cm and 300 cm.")
            .When(x => x.Height.HasValue);
    }
}
