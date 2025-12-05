using MediatR;
using TruyenHayPro.Shared.Wrapper;

namespace TruyenHayPro.Application.Features.Auth.Register;

public record RegisterCommand(
    string FullName, // <-- Mới thêm
    string Email,
    string Username,
    string Password,
    string ConfirmPassword // <-- Mới thêm
) : IRequest<Result<Guid>>;