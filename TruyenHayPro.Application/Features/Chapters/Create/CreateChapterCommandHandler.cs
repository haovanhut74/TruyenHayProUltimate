using MediatR;
using TruyenHayPro.Application.Common.Interfaces.Repositories;
using TruyenHayPro.Application.Common.Interfaces.Services;
using TruyenHayPro.Domain.Common.Entities;
using TruyenHayPro.Shared.Wrapper;

namespace TruyenHayPro.Application.Features.Chapters.Create;

public class CreateChapterCommandHandler : IRequestHandler<CreateChapterCommand, Result<Guid>>
{
    private readonly IChapterRepository _chapterRepository;
    private readonly INovelRepository _novelRepository;
    private readonly ICurrentUserService _currentUserService;

    // Bỏ IUnitOfWork vì chưa được định nghĩa và Repository thường đã xử lý Save
    public CreateChapterCommandHandler(
        IChapterRepository chapterRepository,
        INovelRepository novelRepository,
        ICurrentUserService currentUserService)
    {
        _chapterRepository = chapterRepository;
        _novelRepository = novelRepository;
        _currentUserService = currentUserService;
    }

    public async Task<Result<Guid>> Handle(CreateChapterCommand command, CancellationToken cancellationToken)
    {
        // 1. SỬA LỖI: Lấy DTO từ thuộc tính .Request của Command
        var request = command.Request;

        // 2. SỬA LỖI: Gọi đúng tên hàm GetNovelByIdAsync
        var novel = await _novelRepository.GetNovelByIdAsync(request.NovelId);

        if (novel == null)
        {
            return Result<Guid>.Failure("Truyện không tồn tại!");
        }

        // 3. Kiểm tra quyền sở hữu (Bảo mật)
        if (novel.CreatedBy != _currentUserService.UserId)
        {
            return Result<Guid>.Failure("Bạn không phải là tác giả của truyện này nên không thể thêm chương!");
        }


        // 5. Lưu vào DB

        var isExist = await _chapterRepository.IsChapterNumberExistsAsync(request.NovelId, request.ChapterNumber);
        if (isExist)
        {
            return Result<Guid>.Failure($"Chương số {request.ChapterNumber} đã tồn tại!");
        }

        // 4. Tạo Entity Chapter
        var chapter = new Chapter
        {
            NovelId = request.NovelId,
            Title = request.Title,
            Content = request.Content,
            OrderIndex = request.ChapterNumber,
            WordCount = request.Content.Length,
            CreatedDate = DateTimeOffset.UtcNow,
            CreatedBy = _currentUserService.UserId
        };

        var createdChapter = await _chapterRepository.AddAsync(chapter);

        return Result<Guid>.Success(createdChapter.Id);
    }
}