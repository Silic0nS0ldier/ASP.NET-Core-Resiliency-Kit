using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Html;

/*
Use onerror event to track load failure.
Support error reporting via AJAX and callbacks.
https://www.davepaquette.com/archive/2015/06/22/creating-custom-mvc-6-tag-helpers.aspx
https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.razor.taghelpers.taghelper?view=aspnetcore-2.0
http://www.dotnetcurry.com/aspnet-mvc/1266/using-tag-helpers-aspnetmvc-6-core-1
https://docs.microsoft.com/en-us/aspnet/core/mvc/views/tag-helpers/intro
*/

namespace Fallback.AspNetCore
{
    [HtmlTargetElement("link", Attributes = FallbackHrefAttributeName)]
    public class LinkFallbackTagHelper : ITagHelper
    {
        private const string FallbackHrefAttributeName = "fallback-href";

        [HtmlAttributeName(FallbackHrefAttributeName)]
        public string FallbackHref { get; set; }

        private bool firstInvocation = false;

        // No time dependencies, and so can be run right off the bat.
        public int Order => 0;

        public void Init(TagHelperContext context)
        {
            // Check for previous invocations. If there is none, this invocation must provide the fallback method.
            if (!context.Items.ContainsKey("FallbackFirstFound")) {
                context.Items.Add("FallbackFirstFound", true);
                firstInvocation = true;
            }
        }

        public Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (firstInvocation) {
/*
// Recycles tag with fallback used upon download failure.
function fallback(sender, type, url, phoneHomeUrl, callback) {
    // Ping home about error if requested.
    if (phoneHome) {
        var req = new XMLHttpRequest();
        req.open("POST", phoneHomeUrl, true);
        req.send(JSON.stringify({
            message: ""
        })
    }
    // Replace initial url
    if (type == "css") sender.href = url;
    else if (type == "js") sender.src = url;
    else throw "Type unsupported";

    // Prevent loop, and log errors if requested.
    //alt onerror
    //

    // Execute callback, if defined.

    // Re-plug.
    if (sender.replaceWith) sender.replaceWith(sender);
    else sender.parentNode.replaceChild(sender, sender);
}

function fallbackFailure(sender, phoneHome, callback) {

}
*/
                output.PreElement.AppendHtml(new Microsoft.AspNetCore.Html.HtmlString(
@"<script type=""application/javascript"">
function fallback(sender, type) {
    switch (type) {
        case ""css"":
        case ""js"":
    }
}
</script>
"));
            }
            throw new NotImplementedException();
        }
    }
}
