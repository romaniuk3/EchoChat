window.ScrollToBottom = (elementName) => {
    const element = document.getElementById(elementName);
    element.scrollTop = element.scrollHeight - element.clientHeight;
}