using System;
using System.IO;
using System.Text;
using System.Reflection;
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
    // We can probably resolve this to just attribute detection. JS can check the type client-side.
    [HtmlTargetElement("link", Attributes = FallbackHrefAttributeName)]
    [HtmlTargetElement("script", Attributes = FallbackHrefAttributeName)]
    public class FallbackTagHelper : ITagHelper
    {
        private const string FallbackHrefAttributeName = "fallback-href";

        [HtmlAttributeName(FallbackHrefAttributeName)]
        public string FallbackHref { get; set; }

        public string PhoneHomeUrl;

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

        public async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (firstInvocation) {
                Stream resourceStream = Assembly.GetEntryAssembly().GetManifestResourceStream("EmbeddedResource.Data.fallback.partial.html");
                using (StreamReader reader = new StreamReader(resourceStream, Encoding.UTF8))
                {
                    output.PreElement.AppendHtml(new HtmlString(await reader.ReadToEndAsync()));
                }
            }
            // add onerror (contains fallback urls, etc)
            output.Attributes.Add("onerror", $"fallback({FallbackHref}, );");
            throw new NotImplementedException();
        }
    }
}
