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

        public async Task JoinRoom(string roomId, string userId)
        {
            ConnectedUsers.list.Add(Context.ConnectionId, userId);
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
            await Clients.Group(roomId).SendAsync("user-connected", userId);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);

            await Clients.All.SendAsync("user-disconnected", ConnectedUsers.list[Context.ConnectionId]);
        }
    }
}
