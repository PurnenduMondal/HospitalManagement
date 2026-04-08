using FluentValidation;
using HospitalManagement.Application.DTOs.Appointment;

namespace HospitalManagement.Application.Validators;

public class CreateAppointmentValidator : AbstractValidator<CreateAppointmentDto>
{
    public CreateAppointmentValidator()
    {
        RuleFor(x => x.PatientId)
            .NotEmpty().WithMessage("Valid patient is required.");

        RuleFor(x => x.DoctorId)
            .NotEmpty().WithMessage("Valid doctor is required.");

        RuleFor(x => x.AppointmentDate)
            .NotEmpty().WithMessage("Appointment date is required.")
            .GreaterThanOrEqualTo(DateTime.Today)
            .WithMessage("Appointment date cannot be in the past.");

        RuleFor(x => x.StartTime)
            .NotEmpty().WithMessage("Start time is required.");

        RuleFor(x => x.EndTime)
            .NotEmpty().WithMessage("End time is required.")
            .GreaterThan(x => x.StartTime)
            .WithMessage("End time must be after start time.");

        RuleFor(x => x.Notes)
            .MaximumLength(1000).WithMessage("Notes cannot exceed 1000 characters.")
            .When(x => x.Notes != null);
    }
}
