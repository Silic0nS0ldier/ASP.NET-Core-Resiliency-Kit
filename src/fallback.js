// Polyfill for replaceWith from https://developer.mozilla.org/en-US/docs/Web/API/ChildNode/replaceWith#Polyfill
function ReplaceWith(Ele) {
    'use-strict'; // For safari, and IE > 10
    var parent = this.parentNode,
        i = arguments.length,
        firstIsNode = +(parent && typeof Ele === 'object');
    if (!parent) return;

    while (i-- > firstIsNode) {
        if (parent && typeof arguments[i] !== 'object') {
            arguments[i] = document.createTextNode(arguments[i]);
        }
        if (!parent && arguments[i].parentNode) {
            arguments[i].parentNode.removeChild(arguments[i]);
            continue;
        }
        parent.insertBefore(this.previousSibling, arguments[i]);
    }
    if (firstIsNode) parent.replaceChild(Ele, this);
}
if (!Element.prototype.replaceWith) Element.prototype.replaceWith = ReplaceWith;
if (!CharacterData.prototype.replaceWith) CharacterData.prototype.replaceWith = ReplaceWith;
if (!DocumentType.prototype.replaceWith) DocumentType.prototype.replaceWith = ReplaceWith;

// Recycles tag with fallback used upon download failure.
function fallback(sender, type, urls, reportUrl) {
    "use strict";

    // Extract first url from urls
    urls = urls.split("~");
    var url = urls.shift();
    urls = urls.join("~");


    // Ping home about error if requested.
    if (reportUrl) {
        var req = new XMLHttpRequest();
        req.open("POST", reportUrl, true);
        req.send(JSON.stringify({
            message: "Failed to load resource. Fallback triggered.",
            data: {
                failed_url: sender.href,
                fallback_url: url
            }
        }));
    }

    // Replace initial url
    if (type == "css") sender.href = url;
    else if (type == "js") sender.src = url;
    else throw "Type unsupported";

    // Update onerror trigger, setting to fallbackFailure if this is the last url.
    if (urls) sender.setAttribute("onerror", "fallback(this, '" + type + "', '" + urls +"', '" + reportUrl + "')");
    else sender.setAttribute("onerror", "fallbackFailure(this, '" + reportUrl + "')");

    // Re-plug.
    sender.replaceWith(sender);
}

// Triggered when last fallback fails.
function fallbackFailure(sender, reportUrl) {
    "use strict";

    // Ping home about error if requested.
    if (reportUrl) {
        var req = new XMLHttpRequest();
        req.open("POST", reportUrl, true);
        req.send(JSON.stringify({
            message: "Failed to load resource. No fallbacks remaining.",
            data: {
                failed_url: sender.href
            }
        }));
    }
}