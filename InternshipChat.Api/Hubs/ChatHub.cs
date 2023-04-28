using InternshipChat.DAL.Entities;
using InternshipChat.Shared.DTO.ChatDtos;
using Microsoft.AspNetCore.SignalR;

namespace InternshipChat.Api.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessageAsync(MessageDTO message, string userName)
        {
            await Clients.All.SendAsync("ReceiveMessage", message, userName);
        }
    }
}
