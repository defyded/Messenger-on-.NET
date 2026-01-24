namespace Messenger.Settings
{
    public class JwtOptions
    {
        public string Issuer { get; init; } = default!;
        public string Audience { get; init; } = default!;
        public string SigningKey { get; init; } = default!;
        public int AccesTokenMinutes { get; init; } = 60;
    }
}
