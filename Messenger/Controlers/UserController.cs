using Messenger.DTO;
using Messenger.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Messenger.Controlers
{
    [ApiController]
    [Route("api/users")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<UserDto>>> SearchUsers(
            [FromQuery] string query, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("Строка поиска не может быть пустой");
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdClaim, out var currentUserId))
            {
                return Unauthorized();
            }

            var users = await _userService.GetUsersByUsername(query, currentUserId, ct);

            return Ok(users);
        }
    }
}
