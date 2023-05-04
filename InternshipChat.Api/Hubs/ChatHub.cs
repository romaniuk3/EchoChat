using InternshipChat.DAL.Entities;
using InternshipChat.Shared.DTO.ChatDtos;
using Microsoft.AspNetCore.SignalR;

namespace InternshipChat.Api.Hubs
{
    public class ChatHub : Hub
    {
        const string GroupName = "room";
        public async Task SendMessageAsync(MessageDTO message, string userName)
        {
            await Clients.All.SendAsync("ReceiveMessage", message, userName);
        }

        /*
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();

            ConnectedUsers.list.Add(Context.ConnectionId, Context.ConnectionId);
        }
        */

        /*
        public async Task JoinRoom(string userId)
        {
            ConnectedUsers.list.Add(Context.ConnectionId, userId);
            await Groups.AddToGroupAsync(Context.ConnectionId, GroupName);
            await Clients.Group(GroupName).SendAsync("user-connected", userId);
        } */

        public async Task Call(string callerUserName, string receiverUserName, string callerPeer)
        {
            if (callerUserName != receiverUserName)
            {
                await Clients.All.SendAsync("ReceiveCall", callerUserName, receiverUserName, callerPeer);
            }
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);

            await Clients.All.SendAsync("user-disconnected", ConnectedUsers.list[Context.ConnectionId]);
        }
    }
}
