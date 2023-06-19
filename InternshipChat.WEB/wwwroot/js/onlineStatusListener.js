window.registerForOnlineStatusChanged = (caller, methodName) => {
    caller.invokeMethodAsync(methodName, navigator.onLine);
    window.addEventListener("online", function () {
        caller.invokeMethodAsync(methodName, true);
    });
    window.addEventListener("offline", function () {
        caller.invokeMethodAsync(methodName, false);
    });
}