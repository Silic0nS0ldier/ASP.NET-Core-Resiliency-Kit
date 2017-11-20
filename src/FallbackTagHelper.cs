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
    [HtmlTargetElement("link", Attributes = FallbackUrlAttributeName)]
    [HtmlTargetElement("script", Attributes = FallbackUrlAttributeName)]
    public class FallbackTagHelper : ITagHelper
    {
        private const string FallbackUrlAttributeName = "fallback-href";

        [HtmlAttributeName(FallbackUrlAttributeName)]
        public string FallbackUrl { get; set; }

        public string ReportUrl;

        private bool firstInvocation = false;

        // No time dependencies, and so can be run right off the bat.
        public int Order => 0;

        public void Init(TagHelperContext context)
        {
            // Check for previous invocations. If there is none, this invocation must provide the fallback method.
            if (!context.Items.ContainsKey("FirstFallbackFound"))
            {
                context.Items.Add("FirstFallbackFound", true);
                firstInvocation = true;
            }
        }

        public async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (firstInvocation)
            {
                output.PreElement.AppendHtml(new HtmlString("<script type=\"application/javascript\">~~FALLBACK_CODE~~</script>"));
            }
            // add onerror (contains fallback urls, etc)
            output.Attributes.Add("onerror", $@"fallback(""{FallbackUrl}"", );");
            throw new NotImplementedException();
        }
    }
}
