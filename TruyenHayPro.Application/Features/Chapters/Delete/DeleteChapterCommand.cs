using MediatR;
using TruyenHayPro.Shared.Wrapper;

namespace TruyenHayPro.Application.Features.Chapters.Delete;

public record DeleteChapterCommand(Guid Id) : IRequest<Result<Guid>>;