namespace Messenger.DTO
{
    public sealed record RegisterRequest(string Username, string Email, string Password);
    public sealed record LoginRequest(string Email, string Password);
    public sealed record AuthResponce(
         Guid Id, 
         string Username,
         string Email, 
         string AccsesToken,
         DateTime ExpiresAtUtc
    );
}
