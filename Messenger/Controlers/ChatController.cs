using Messenger.DTO;
using Messenger.Services;
using Messenger.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Messenger.Controlers
{
    [ApiController]
    [Route("api/chats")]
    [Authorize]
    public class ChatController : Controller
    {
        private readonly IChatService _service;
        public ChatController(IChatService service) => _service = service;

        private Guid GetUserId()
        {
            var raw = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
            if (raw is null)
                throw new UnauthorizedAccessException("User id claim is missing.");

            return Guid.Parse(raw);
        }
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ChatDto>>> GetChats(CancellationToken cancellationToken)
        {
            var userId = GetUserId();
            var result = _service.GetUserChatsAsync(userId);
            return Ok(result);
        }
        [HttpPost]
        public async Task<ActionResult<ChatDto>> CreateChat(ChatDto dto)
        {
            var userId = GetUserId();
            var createdChat = await _service.CreatChatAsync(userId, dto.CompanionId);

            return Ok(createdChat);
        }
        //[HttpDelete]
        //public async Task<ActionResult> DeleteChat()
        //{
        //    ToDo доделать
        //}
    }
}
