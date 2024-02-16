using Microsoft.AspNetCore.SignalR;

namespace EYOkulProjectWebUI.Hubs
{
    public class TransactionHub : Hub
    {

        public async Task sendmessageAsync()
        {

        await Clients.All.SendAsync("receiveMessage","Merhaba");

        }
    }
}
