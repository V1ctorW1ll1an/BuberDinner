namespace BuberDinner.Infrastructure.Authentication;

public class JwtSettings
{
    public const string _SectionName = "JwtSettings";
    public string Secret { get; init; } = null!;
    public string Issuer { get; init; } = null!;
    public string Audience { get; init; } = null!;
    public int expiryMinutes { get; init; }
}
