using FluentValidation;

namespace TruyenHayPro.Application.Features.Auth.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Họ tên không được để trống")
            .MaximumLength(50).WithMessage("Tối đa 50 ký tự");

        RuleFor(x => x.Email)
            .NotEmpty().EmailAddress();

        RuleFor(x => x.Username)
            .NotEmpty().MinimumLength(3);

        RuleFor(x => x.Password)
            .NotEmpty().MinimumLength(12).WithMessage("Mật khẩu ít nhất 12 ký tự");

        // Logic Confirm Password chuyển sang đây
        RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.Password).WithMessage("Mật khẩu nhập lại không khớp");
    }
}