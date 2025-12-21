using AutoMapper;
using MediatR;
using TruyenHayPro.Application.Common.Interfaces.Repositories;
using TruyenHayPro.Domain.Common.Entities;
using TruyenHayPro.Shared.Wrapper;

namespace TruyenHayPro.Application.Features.Chapters.Create;

public class CreateChapterCommandHandler : IRequestHandler<CreateChapterCommand, Result<Guid>>
{
    private readonly IChapterRepository _chapterRepository;
    private readonly IMapper _mapper;

    public CreateChapterCommandHandler(IChapterRepository chapterRepository, IMapper mapper)
    {
        _chapterRepository = chapterRepository;
        _mapper = mapper;
    }

    public async Task<Result<Guid>> Handle(CreateChapterCommand command, CancellationToken cancellationToken)
    {
        // 1. Check trùng số chương
        var exists =
            await _chapterRepository.IsChapterNumberExistsAsync(command.Request.NovelId, command.Request.ChapterNumber);

        if (exists)
        {
            // SỬA 1: Bỏ 'await', đổi 'Failure' thành 'Fail' (nếu thư viện dùng chữ Fail) 
            // hoặc giữ 'Failure' nếu đúng tên hàm, nhưng chắc chắn phải bỏ 'await'
            return Result<Guid>.Failure($"Chương số {command.Request.ChapterNumber} đã tồn tại.");
            // LƯU Ý: Nếu thư viện của bạn có hàm FailAsync thì giữ await. 
            // Nếu báo lỗi "not awaitable" -> Dùng dòng dưới đây:
            // return Result<Guid>.Fail($"Chương số {command.Request.ChapterNumber} đã tồn tại.");
        }

        // 2. Map DTO -> Entity
        var chapter = new Chapter
        {
            NovelId = command.Request.NovelId,
            Title = command.Request.Title,
            Content = command.Request.Content,
            OrderIndex = command.Request.ChapterNumber, // Đảm bảo Property này khớp với Entity
        };

        // 3. Lưu vào DB
        await _chapterRepository.AddAsync(chapter);

        // SỬA 2: Bỏ 'await' và bỏ tham số string (message) vì hàm Success chỉ nhận Guid
        return Result<Guid>.Success(chapter.Id);
        // Tương tự, nếu báo lỗi "not awaitable" hoặc sai tham số -> Dùng dòng dưới đây:
        // return Result<Guid>.Success(chapter.Id);
    }
}