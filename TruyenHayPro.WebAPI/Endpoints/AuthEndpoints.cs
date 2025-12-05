using MediatR;
using Microsoft.AspNetCore.Mvc;
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
    }
}