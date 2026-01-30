using FluentValidation;

namespace Application.Settings.Commands.CreateSetting;

/// <summary>
/// Validator for CreateSettingCommand
/// </summary>
public class CreateSettingCommandValidator : AbstractValidator<CreateSettingCommand>
{
    public CreateSettingCommandValidator()
    {
        RuleFor(x => x.Key)
            .NotEmpty().WithMessage("Key is required")
            .MaximumLength(100).WithMessage("Key must not exceed 100 characters");

        RuleFor(x => x.Value)
            .NotEmpty().WithMessage("Value is required")
            .MaximumLength(2000).WithMessage("Value must not exceed 2000 characters");

        RuleFor(x => x.Category)
            .NotEmpty().WithMessage("Category is required")
            .MaximumLength(50).WithMessage("Category must not exceed 50 characters");
    }
}
