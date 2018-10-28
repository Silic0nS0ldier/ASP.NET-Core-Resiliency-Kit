/**
 * Creates a new node to permit the loading of a fallback resource.
 * @param {HTMLElement} sender Sender of the event
 * @param {string} attributeName Name of attribute to update
 * @param {string[]} urls Urls to now use instead
 */
function fallback(sender, attributeName, urls) {
    // Make new element
    var newNode = document.createElement(sender.tagName);
    for (var i = 0, n = sender.attributes.length; i < n; i++) {
        var attr = sender.attributes.item(i);
        newNode.setAttribute(attr.name, attr.value);
    }
    // Remove onerror
    newNode.removeAttribute("onerror");
    // Set new url
    newNode.setAttribute(attributeName, urls.shift());
    // Add onerror event if there is more to come
    if (urls.length > 0) newNode.setAttribute("onerror", "fallback(this,'" + attributeName + "'," + JSON.stringify(urls) + ")");
    // Add element
    sender.parentNode.insertBefore(newNode, sender.nextSibling);
}