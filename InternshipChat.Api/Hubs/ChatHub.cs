using InternshipChat.DAL.Entities;
using InternshipChat.Shared.DTO.ChatDtos;
using InternshipChat.Shared.Enums;
using Microsoft.AspNetCore.SignalR;

namespace InternshipChat.Api.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessageAsync(MessageDTO message, string userName)
        {
            await Clients.All.SendAsync("ReceiveMessage", message, userName);
        }

        public async Task Call(string callerUserName, string receiverUserName)
        {
            if (callerUserName != receiverUserName)
            {
                await Clients.All.SendAsync("ReceiveCallOffer", callerUserName, receiverUserName);
            }
        }

        public async Task AcceptCall(string acceptedUserName, string acceptedPeerId)
        {
            await Clients.All.SendAsync("ReceiveAcceptCall", acceptedUserName, acceptedPeerId);
        }

        public async Task EndCall()
        {
            ConnectedUsers.list.Clear();
            await Clients.All.SendAsync("ReceiveEndCall");
         
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);

            await Clients.All.SendAsync("ReceiveEndCall");
        }
    }
}
