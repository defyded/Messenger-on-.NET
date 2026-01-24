using Messenger.DTO;

namespace Messenger.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponce> RegisterAsync(RegisterRequest request, CancellationToken ct = default);
        Task<AuthResponce> LoginAsync(LoginRequest request, CancellationToken ct = default);
    }
}
