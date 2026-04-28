using Messenger.Domain.Entities;
using Messenger.DTO;

namespace Messenger.Services.Interfaces
{
    public interface IUserService
    {
        Task<List<UserDto>> GetUsersByUsername(string query, Guid currentUserId, CancellationToken cancellationToken); 
    }
}
