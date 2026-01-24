using Messenger.Domain.Entities;

namespace Messenger.Services.Interfaces
{
    public interface ITokenService
    {
        (string token, DateTime ExpiresAtUtc) CreateAccesToken(User user);
    }
}
