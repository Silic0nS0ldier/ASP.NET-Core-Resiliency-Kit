using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;

namespace AspNetCoreResiliencyKit.TagHelpers
{
    public abstract class ResourceResilienceTagHelper : TagHelper
    {
        /// <summary>
        /// The script that will be injected into the page when a resource with alternative sources is defined.
        /// Exact code is inserted into file and subsequently removed during build.
        /// This member is marked as public to permit auditing (this is recommended as external libraries are used during library build, any dependency in the tree could be modified by a malicious party or contain a security flaw).
        /// </summary>
        public const string FallbackScript = @"<script type=""application/javascript"">~~FallbackScriptInjectedDuringBuild~~</script>";

        /// <summary>
        /// Name of local resource attribute.
        /// </summary>
        protected abstract string ResourceAttributeName { get; }

        /// <summary>
        /// Name of alternative resources attribute.
        /// </summary>
        protected abstract string ResourceAlternativesAttributeName { get; }

        /// <summary>
        /// Resource in a location that is fully controlled by the site owner.
        /// This may be safely used for integrity hash generation.
        /// </summary>
        protected abstract string ControlledResource { get; }

        /// <summary>
        /// Alternative resources that may not be in control of the site owner.
        /// The first will be the default, then falling back through the remaining options until landing on the controlled resource in the worst case scenario.
        /// </summary>
        protected abstract List<string> AlternativeResources { get; }

        /// <summary>
        /// Key used in <see cref="HttpContext"/> to detect if the script has been injected on the current page.
        /// </summary>
        protected const string CoreScriptKey = "CoreScriptInjected";

        /// <summary>
        /// Used to get a reference to the HttpContext for tracking state during a request.
        /// </summary>
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        /// <summary>
        /// Injects the core fallback script into the page if not done already.
        /// When overriding this method be sure to call the base method to ensure proper functionality.
        /// </summary>
        /// <param name="context">Contains information associated with the current HTML tag.</param>
        /// <param name="output">A stateful HTML element used to generate an HTML tag.</param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            // Inject core script if not already done
            if (!ViewContext.HttpContext.Items.ContainsKey(CoreScriptKey))
            {
                output.PreElement.AppendHtml(FallbackScript);
                ViewContext.HttpContext.Items[CoreScriptKey] = true;
            }

            if (AlternativeResources.Count > 0)
            {
                // Build fallback array
                string altResources = "[";
                for (int i = 1; i < AlternativeResources.Count; i++)
                    altResources += $"'{AlternativeResources[i]}',";
                altResources += $"'{ControlledResource}']";

                // Compare function and set onerror attribute
                output.Attributes.SetAttribute("onerror", $"fallback(this,'{ResourceAttributeName}',{altResources})");

                // Set default resource
                output.Attributes.SetAttribute(ResourceAttributeName, AlternativeResources[0]);
            }

            // Remove alternatives attribute
            output.Attributes.RemoveAll(ResourceAlternativesAttributeName);
        }
    }
}
