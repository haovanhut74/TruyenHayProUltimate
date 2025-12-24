using MediatR;
using TruyenHayPro.Application.DTO;
using TruyenHayPro.Shared.Wrapper;

namespace TruyenHayPro.Application.Features.Chapters.Queries.GetByNovel;

public record GetChaptersByNovelQuery(Guid NovelId) : IRequest<Result<List<ChapterDto>>>;