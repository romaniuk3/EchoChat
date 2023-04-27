using InternshipChat.DAL.Entities;
using Microsoft.AspNetCore.SignalR;

namespace InternshipChat.Api.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessageAsync(Message message, string userName)
        {
            await Clients.All.SendAsync("ReceiveMessage", message, userName);
        }
    }
}
