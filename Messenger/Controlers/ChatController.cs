using Messenger.Domain.Entities;
using Messenger.DTO;
using Messenger.Hubs;
using Messenger.Services;
using Messenger.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Messenger.Controlers
{
    [ApiController]
    [Route("api/chats")]
    [Authorize]
    public class ChatController : Controller
    {
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly IChatService _chatService;
        private readonly IChatMessageService _messageService; 
        public ChatController(IChatService chatService, IChatMessageService messageService, IHubContext<ChatHub> hubContext)
        {
            _chatService = chatService;
            _messageService = messageService;
            _hubContext = hubContext;
        }

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
            var result = await _chatService.GetUserChatsAsync(userId);
            return Ok(result);
        }
        [HttpPost]
        public async Task<ActionResult<ChatDto>> CreateChat([FromBody]RequestChatCreateDto dto)
        {
            var userId = GetUserId();
            
            try
            {
                var createdChat = await _chatService.CreateChatAsync(userId, dto.CompanionId);
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
                await _chatService.DeleteChatAsync(chatId, UserId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseError(ex.Message));//400
            }
            
        }
        [HttpGet("{chatId:guid}/messages")]
        public async Task<ActionResult<ICollection<ChatMessageDto>>> GetMessages(Guid chatId)
        {
            try
            {
                var userId = GetUserId();
                var messages = await _messageService.GetChatMessagesAsync(chatId, userId);
                return Ok(messages);
            }
            catch(ChatMessageException ex)
            {
                return BadRequest(new ResponseError(ex.Message));
            }
        }
        [HttpPost("{chatId:guid}/messages")]
        public async Task<ActionResult<ChatMessageDto>> SendMessage(Guid chatId, [FromBody]CreateChatMessageDto dto)
        {
            try
            {
                var userId = GetUserId(); 
                var secureDto = dto with { ChatId = chatId, SenderId = userId };
                var result = await _messageService.SendAsync(secureDto);

                await _hubContext.Clients.Group(chatId.ToString())
                    .SendAsync("ReceiveMessage", result);
                Console.WriteLine($"Message was send {secureDto.Content}");
                return Ok(result);
            }
            catch (ChatMessageException ex)
            {
                return BadRequest(new { error = ex.Code, message = ex.Message });
            }
        }
        [HttpDelete("{chatId:guid}/messages")]
        public async Task<ActionResult<ChatMessageDto>> DeleteMessage([FromBody]JsonElement element)
        {
            try
            {
                var userId = GetUserId();
                element.TryGetProperty("messageId", out var elementmessageId);
                Guid.TryParse(elementmessageId.GetString(), out var messageId);
                await _messageService.DeleteMessageAsync(messageId, userId);
                return NoContent();
            }
            catch (ChatMessageException ex)
            {
                return BadRequest(new { error = ex.Code, message = ex.Message });
            }
        }
    }
}
