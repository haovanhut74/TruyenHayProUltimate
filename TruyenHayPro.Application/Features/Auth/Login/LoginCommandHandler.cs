using MediatR;
using TruyenHayPro.Application.Common.Interfaces.Services;
using TruyenHayPro.Shared.Contracts.Identity;
using TruyenHayPro.Shared.Wrapper;

namespace TruyenHayPro.Application.Features.Auth.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<AuthResponse>>
{
    private readonly IIdentityService _identityService;
    public LoginCommandHandler(IIdentityService identityService) => _identityService = identityService;

    public async Task<Result<AuthResponse>> Handle(LoginCommand req, CancellationToken ct)
        => await _identityService.LoginAsync(req.Username, req.Password);
}