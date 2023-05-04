using InternshipChat.WEB.Enums;

namespace InternshipChat.WEB.StateContainers
{
    public class CallStateContainer
    {
        public CallState UserCallState { get; set; }

        public event Action? CallStateChanged;
        public void ChangeCallState(CallState callState)
        {
            UserCallState = callState;
            CallStateChanged?.Invoke();
        }
    }
}
