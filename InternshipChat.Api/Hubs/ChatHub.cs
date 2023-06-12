using InternshipChat.DAL.Entities;
using InternshipChat.Shared.DTO.ChatDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace InternshipChat.Api.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        public async Task SendMessageAsync(MessageDTO message, string userName)
        {
            await Clients.All.SendAsync("ReceiveMessage", message, userName);
        }


        //Video Calls
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
            await Clients.All.SendAsync("ReceiveEndCall");
         
        }

        //Attachments
        public async Task NotifyAboutAttachment(string receiverUserName, int chatId)
        {
            await Clients.All.SendAsync("ReceiveAttachmentStatus", receiverUserName, chatId);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);

            await Clients.All.SendAsync("ReceiveEndCall");
        }
    }
}
