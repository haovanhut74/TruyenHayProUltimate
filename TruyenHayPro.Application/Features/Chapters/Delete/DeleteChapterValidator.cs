using FluentValidation;

namespace TruyenHayPro.Application.Features.Chapters.Delete;

public class DeleteChapterValidator : AbstractValidator<DeleteChapterCommand>
{
    public DeleteChapterValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("ID chương không được để trống.");
    }
}