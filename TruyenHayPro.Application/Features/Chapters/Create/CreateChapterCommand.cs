using MediatR;
using TruyenHayPro.Application.DTO;
using TruyenHayPro.Shared.Wrapper;

namespace TruyenHayPro.Application.Features.Chapters.Create;

public record CreateChapterCommand(CreateChapterDto Request) : IRequest<Result<Guid>>;