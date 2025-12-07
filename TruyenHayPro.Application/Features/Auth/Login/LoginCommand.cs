using MediatR;
using TruyenHayPro.Shared.Contracts.Identity;
using TruyenHayPro.Shared.Wrapper;

namespace TruyenHayPro.Application.Features.Auth.Login;

public record LoginCommand(string Username, string Password) : IRequest<Result<AuthResponse>>;