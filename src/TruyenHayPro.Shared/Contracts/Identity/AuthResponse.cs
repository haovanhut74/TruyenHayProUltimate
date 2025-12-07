namespace TruyenHayPro.Shared.Contracts.Identity;

public record AuthResponse(Guid Id, string Username, string Email, string Token);