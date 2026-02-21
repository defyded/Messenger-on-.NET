using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Messenger.Hubs
{
    public class ChatHub : Hub
    {
        public async Task JoinChat(Guid chatId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString());
        }
        public async Task LeaveChat(Guid chatId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId.ToString());
        }
    }
}
