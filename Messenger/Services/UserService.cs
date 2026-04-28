using Messenger.DTO;
using Messenger.Infastucture.Repository;
using Messenger.Infastucture.Repository.Interfaces;
using Messenger.Services.Interfaces;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore.Storage;

namespace Messenger.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository) => _userRepository = userRepository;
        public async Task<List<UserDto>> GetUsersByUsername(string query, 
            Guid currentUserId,
            CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetByName(query);

            return users
                .Where(u => u.Id != currentUserId)
                .Select(u => new UserDto(
                    u.Id,
                    u.AvatarUrl, 
                    u.Username
                ))
                .ToList();
        }
    }
}
