using MediatR;
using TruyenHayPro.Application.Common.Interfaces.Repositories;
using TruyenHayPro.Application.Common.Interfaces.Services;
using TruyenHayPro.Shared.Wrapper;

namespace TruyenHayPro.Application.Features.Chapters.Update;

public class UpdateChapterCommandHandler : IRequestHandler<UpdateChapterCommand, Result<Guid>>
{
    private readonly IChapterRepository _chapterRepository;
    private readonly ICurrentUserService _currentUserService;

    public UpdateChapterCommandHandler(
        IChapterRepository chapterRepository,
        ICurrentUserService currentUserService)
    {
        _chapterRepository = chapterRepository;
        _currentUserService = currentUserService;
    }

    public async Task<Result<Guid>> Handle(UpdateChapterCommand command, CancellationToken cancellationToken)
    {
        var dto = command.Request;

        // 1. Lấy chương kèm thông tin truyện để check quyền
        var chapter = await _chapterRepository.GetChapterWithNovelAsync(dto.Id);

        if (chapter == null)
            return Result<Guid>.Failure("Chương không tồn tại.");

        // 2. Check quyền: User hiện tại phải là người tạo truyện
        if (chapter.Novel.CreatedBy != _currentUserService.UserId)
            return Result<Guid>.Failure("Bạn không có quyền sửa chương này.");

        // 3. Cập nhật Entity
        chapter.Title = dto.Title;
        chapter.Content = dto.Content;
        chapter.OrderIndex = dto.ChapterNumber;
        chapter.WordCount = dto.Content.Split(new[] { ' ', '\n' }, StringSplitOptions.RemoveEmptyEntries).Length;
        chapter.LastModifiedBy = _currentUserService.UserId;
        chapter.LastModifiedAt = DateTimeOffset.UtcNow;

        // 4. Lưu xuống DB qua Repository
        await _chapterRepository.UpdateAsync(chapter);

        return Result<Guid>.Success(chapter.Id);
    }
}