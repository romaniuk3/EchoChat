using InternshipChat.Shared.Enums;

namespace InternshipChat.Api.Hubs
{
    public static class ConnectedUsers
    {
        public static IDictionary<string, CallInitiator> list = new Dictionary<string, CallInitiator>();
    }
}
