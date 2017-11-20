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
    /// <summary>
    /// A tag helper that provides fallback functionality for resources that fail to load. Supports link and script tags.
    /// </summary>
    [HtmlTargetElement("link", Attributes = "fallback-urls")]
    [HtmlTargetElement("script", Attributes = "fallback-urls")]
    public class FallbackTagHelper : TagHelper
    {
        /// <summary>
        /// A ~ delimited string of urls to be used if the current resource fails to load.
        /// Matches with the fallback-urls attribute in Razor pages.
        /// </summary>
        public string FallbackUrls { get; set; }
        
        // fallback-report-url
        /// <summary>
        /// A url to be used if a fallback supported resource fails to load. Optional.
        /// </summary>
        public string FallbackReportUrl { get; set; }

        /// <summary>
        /// Used to track if this is the first time the tag has been used on a page. Allows the required script to be placed once per page.
        /// </summary>
        private bool firstInvocation = false;
        
        /// <summary>
        /// Initalises tag helper.
        /// </summary>
        /// <param name="context"></param>
        public override void Init(TagHelperContext context)
        {
            // Check for previous invocations. If there is none, this invocation must provide the fallback method.
            if (!context.Items.ContainsKey("FirstFallbackFound"))
            {
                context.Items.Add("FirstFallbackFound", true);
                firstInvocation = true;
            }
        }
        
        /// <summary>
        /// Executes tag helper.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="output"></param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            // Inject required script if this is the first invocation.
            if (firstInvocation)
            {
                // TODO: Jordan Mele - The injected code includes a polyfill not required by all modern browsers. Is there a way we can detect this, and further minimise the payload? Diminishing returns?
                output.PreElement.AppendHtml(new HtmlString("<script type=\"application/javascript\">~~FALLBACK_SCRIPT_INJECTED_DURING_BUILD~~</script>"));
            }
            
            // Add onerror event to attribute list.
            output.Attributes.Add("onerror", $@"fallback(this, ""{FallbackUrls}"", ""{FallbackReportUrl}"");");

            // Remove tag helper attributes if they exist.
            output.Attributes.RemoveAll("fallback-urls");
            output.Attributes.RemoveAll("fallback-report-url");
        }
    }
}
