using Messenger.Domain.Entities;
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
            var result = await _service.GetUserChatsAsync(userId);
            return Ok(result);
        }
        [HttpPost]
        public async Task<ActionResult<ChatDto>> CreateChat([FromBody]RequestChatCreateDto dto)
        {
            var userId = GetUserId();
            
            try
            {
                var createdChat = await _service.CreatChatAsync(userId, dto.CompanionId);
                return Ok(createdChat);

            }
            catch(Exception ex)
            {
                return BadRequest(new ResponseError(ex.Message));//400
            }
        }
        [HttpDelete("{chatId:guid}")]
        public async Task<ActionResult> DeleteChat(Guid chatId)
        {
            var UserId = GetUserId();

            try
            {
                await _service.DeleteChatAsync(chatId, UserId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseError(ex.Message));//400
            }
            
        }
    }
}
