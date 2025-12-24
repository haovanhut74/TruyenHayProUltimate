using MediatR;
using TruyenHayPro.Application.Common.Interfaces.Repositories;
using TruyenHayPro.Application.DTO;
using TruyenHayPro.Shared.Wrapper;

namespace TruyenHayPro.Application.Features.Chapters.Queries.GetByNovel;

// 2. Tạo Handler (Xử lý)
public class GetChaptersByNovelHandler : IRequestHandler<GetChaptersByNovelQuery, Result<List<ChapterDto>>>
{
    private readonly IChapterRepository _repository;

    public GetChaptersByNovelHandler(IChapterRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<List<ChapterDto>>> Handle(GetChaptersByNovelQuery request,
        CancellationToken cancellationToken)
    {
        var chapters = await _repository.GetListByNovelIdAsync(request.NovelId);
        return Result<List<ChapterDto>>.Success(chapters);
    }
}