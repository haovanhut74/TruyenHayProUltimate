using FluentValidation;
using TruyenHayPro.Application.DTO;

namespace TruyenHayPro.Application.Features.Chapters.Update;

public class UpdateChapterValidator : AbstractValidator<UpdateChapterDto>
{
    public UpdateChapterValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Tiêu đề không được để trống")
            .MaximumLength(500).WithMessage("Tiêu đề tối đa 500 ký tự");

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Nội dung không được để trống")
            .MinimumLength(1000).WithMessage("Nội dung quá ngắn (tối thiểu 1000 ký tự)");

        RuleFor(x => x.ChapterNumber).GreaterThan(0).WithMessage("Số chương phải lớn hơn 0");
    }
}