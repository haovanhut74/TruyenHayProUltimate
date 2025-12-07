using FluentValidation;
using TruyenHayPro.Application.DTO;

namespace TruyenHayPro.Application.Features.Novels.Create;

public class CreateNovelValidator : AbstractValidator<CreateNovelDto>
{
    public CreateNovelValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Tên truyện không được để trống")
            .MaximumLength(200).WithMessage("Tên truyện quá dài");

        RuleFor(x => x.Author)
            .NotEmpty().WithMessage("Bút danh không được để trống");

        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("Phải chọn thể loại")
            .NotEqual(Guid.Empty).WithMessage("Thể loại không hợp lệ");

        RuleFor(x => x.CoverImageUrl)
            .Must(uri => string.IsNullOrEmpty(uri) || Uri.TryCreate(uri, UriKind.Absolute, out _))
            .WithMessage("Link ảnh bìa không đúng định dạng URL");

        RuleFor(x => x.Description)
            .MaximumLength(5000).WithMessage("Giới thiệu không được quá 5000 ký tự");
    }
}