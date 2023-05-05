using InternshipChat.WEB.Enums;

namespace InternshipChat.WEB.StateContainers
{
    public class CallStateContainer
    {
        public CallState UserCallState { get; set; }
        public string? ReceiverUserName { get; set; }

        public event Action? CallStateChanged;
        public void ChangeCallState(CallState callState, string? receiverUserName = null)
        {
            UserCallState = callState;
            ReceiverUserName = receiverUserName;
            CallStateChanged?.Invoke();
        }
    }
}
