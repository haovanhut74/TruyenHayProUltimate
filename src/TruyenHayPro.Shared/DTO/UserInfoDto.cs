namespace TruyenHayPro.Shared.DTO;

public class UserInfoDto
{
    public Guid Id { get; set; }
    public string Username { get; set; } = default!;
    public string FullName { get; set; } = default!;
    public string? Email { get; set; }
}
