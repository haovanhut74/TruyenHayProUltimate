using MediatR;
using TruyenHayPro.Application.Common.Interfaces.Repositories;
using TruyenHayPro.Application.Common.Interfaces.Services;
using TruyenHayPro.Shared.Wrapper;

namespace TruyenHayPro.Application.Features.Chapters.Delete;

public class DeleteChapterCommandHandler : IRequestHandler<DeleteChapterCommand, Result<Guid>>
{
    private readonly IChapterRepository _chapterRepository;
    private readonly ICurrentUserService _currentUserService;

    public DeleteChapterCommandHandler(
        IChapterRepository chapterRepository,
        ICurrentUserService currentUserService)
    {
        _chapterRepository = chapterRepository;
        _currentUserService = currentUserService;
    }

    public async Task<Result<Guid>> Handle(DeleteChapterCommand request, CancellationToken cancellationToken)
    {
        // 1. Sửa GetByIdAsync -> GetChapterDetailAsync
        var chapter = await _chapterRepository.GetChapterDetailAsync(request.Id);
        
        if (chapter == null)
        {
            // 2. Sửa FailAsync -> Failure và bỏ await
            return Result<Guid>.Failure("Không tìm thấy chương truyện.");
        }

        // 3. Kiểm tra quyền sở hữu
        var userId = _currentUserService.UserId;
        if (chapter.CreatedBy != userId) 
        {
            return Result<Guid>.Failure("Bạn không có quyền xóa chương này.");
        }

        // 4. Thực hiện xóa
        await _chapterRepository.DeleteAsync(chapter);

        // 5. Sửa SuccessAsync -> Success và bỏ await
        return Result<Guid>.Success(chapter.Id);
    }
}