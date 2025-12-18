using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TruyenHayPro.Application.Common.Interfaces.Services;
using TruyenHayPro.Application.Features.Auth.Login;
using TruyenHayPro.Application.Features.Auth.Register;
using TruyenHayPro.Shared.Contracts.Identity;

namespace TruyenHayPro.WebAPI.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/auth").WithTags("Auth");

        group.MapPost("/register", async (ISender sender, [FromBody] RegisterRequest request) =>
        {
            // Map từ Request (Shared) sang Command (Application)
            var command = new RegisterCommand(
                request.FullName,
                request.Email,
                request.Username,
                request.Password,
                request.ConfirmPassword
            );

            var result = await sender.Send(command);

            if (result.IsFailure)
            {
                return Results.BadRequest(result);
            }

            return Results.Ok(result);
        });

        group.MapPost("/login", async (ISender sender, [FromBody] LoginRequest request) =>
        {
            var command = new LoginCommand(request.Username, request.Password);
            var result = await sender.Send(command);
            return result.IsSuccess ? Results.Ok(result) : Results.BadRequest(result);
        });
        group.MapGet("/me", async (ClaimsPrincipal user, IIdentityService identityService) =>
        {
            var userId = identityService.GetUserId(user);
            if (userId == Guid.Empty) return Results.Unauthorized();

            var info = await identityService.GetUserInfoAsync(userId);
            return Results.Ok(info); // { Id, Username, FullName, Email }
        }).RequireAuthorization();
    }
}