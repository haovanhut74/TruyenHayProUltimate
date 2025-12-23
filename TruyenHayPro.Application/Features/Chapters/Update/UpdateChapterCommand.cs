using MediatR;
using TruyenHayPro.Application.DTO;
using TruyenHayPro.Shared.Wrapper;

namespace TruyenHayPro.Application.Features.Chapters.Update;

public record UpdateChapterCommand(UpdateChapterDto Request) : IRequest<Result<Guid>>;