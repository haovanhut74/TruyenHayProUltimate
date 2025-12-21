using FluentValidation;

namespace TruyenHayPro.Application.Features.Chapters.Create;

public class CreateChapterValidator : AbstractValidator<CreateChapterCommand>
{
    public CreateChapterValidator()
    {
        RuleFor(x => x.Request.Title)
            .NotEmpty().WithMessage("Tên chương không được để trống")
            .MaximumLength(200).WithMessage("Tên chương quá dài");

        RuleFor(x => x.Request.Content)
            .NotEmpty().WithMessage("Nội dung chương không được để trống")
            .MinimumLength(100).WithMessage("Nội dung chương quá ngắn (tối thiểu 100 ký tự)");

        RuleFor(x => x.Request.ChapterNumber)
            .GreaterThan(0).WithMessage("Số chương phải lớn hơn 0");
    }
}