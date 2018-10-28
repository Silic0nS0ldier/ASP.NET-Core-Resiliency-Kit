using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;

namespace AspNetCoreResiliencyKit.TagHelpers
{
    [HtmlTargetElement("script", Attributes = "src,src-alternatives")]
    public class ScriptResilienceTagHelper : ResourceResilienceTagHelper
    {
        /// <summary>
        /// Source attribute value. Used for automatic binding.
        /// </summary>
        public string Src { get; set; }

        /// <summary>
        /// Alternative sources attribute value. Used for automatic binding.
        /// </summary>
        public List<string> SrcAlternatives { get; set; }

        /// <summary>
        /// Name of source attribute.
        /// </summary>
        protected override string ResourceAttributeName => "src";

        /// <summary>
        /// Name of alternative sources attribute.
        /// </summary>
        protected override string ResourceAlternativesAttributeName => "src-alternatives";

        /// <summary>
        /// Aid for parent class to locate "controlled" resource.
        /// </summary>
        protected override string ControlledResource => Src;

        /// <summary>
        /// Aid for parent class to locate alternative resources.
        /// </summary>
        protected override List<string> AlternativeResources => SrcAlternatives;
    }
}
