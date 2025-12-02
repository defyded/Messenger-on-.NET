using Microsoft.AspNetCore.SignalR;

namespace Messenger.Hubs
{
    public class AuthHub : Hub
    {
        public async Task VKAuth(string user, string token)
        {

        }

        public async Task YandexAuth(string user, string message)
        {

        }

        public async Task T_BankAuth(string user, string message)
        {

        }
    }
}
