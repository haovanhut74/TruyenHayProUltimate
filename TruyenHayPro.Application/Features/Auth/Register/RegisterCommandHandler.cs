using MediatR;
using TruyenHayPro.Application.Common.Interfaces.Services;
using TruyenHayPro.Shared.Wrapper;

namespace TruyenHayPro.Application.Features.Auth.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<Guid>>
{
    private readonly IIdentityService _identityService;

    public RegisterCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<Result<Guid>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        return await _identityService.RegisterUserAsync(request.Email, request.Username, request.Password,
            request.FullName);
    }
}