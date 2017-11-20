using System;
using Xunit;
using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Fallback;

namespace Fallback.AspNetCore.Test
{
    public class FallbackTagHelperTest
    {
        // Fallback Script (script is inserted during build)
        private string FallbackScript = "<script type=\"application/javascript\">~~FALLBACK_SCRIPT_INJECTED_DURING_BUILD~~</script>";

        /// <summary>
        /// Generates a clean environment for testing the tag helper.
        /// </summary>
        /// <param name="tagName">Name of HTML tag.</param>
        /// <param name="fallbackUrls">Fallback URLs delimited with '~'.</param>
        /// <param name="reportUrl">URL to be used to report fallback use and failure.</param>
        /// <param name="items">Optional. Items register used to establish a relation between steps of larger tests.</param>
        /// <returns></returns>
        private static (TagHelperContext context, TagHelperOutput output) EnvironmentGenerator(string tagName, string fallbackUrls, string reportUrl, IDictionary<object, object> items = null)
        {
            // Create and populate attribute list
            var attributes = new TagHelperAttributeList
            {
                new TagHelperAttribute("fallback-urls", fallbackUrls),
                new TagHelperAttribute("fallback-report-url", reportUrl)
            };

            // Create items register
            items = items ?? new Dictionary<object, object>();

            // Create context
            var context = new TagHelperContext(tagName, attributes, items, "someId");

            // Create output
            var output = new TagHelperOutput(tagName, attributes, (bool b, HtmlEncoder h) => {
                var tagHelperContent = new DefaultTagHelperContent();
                tagHelperContent.SetContent(string.Empty);
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

            return (context: context, output: output);
        }

        // Single first
        [Theory]
        [InlineData("link", "http://www.example.com/1.css~http://www.example.com/2.css")]
        [InlineData("script", "http://www.example.com/3.js", "http://www.example.com/report/resourcefailure/")]
        public void FallbackUsedOneTest(string tagName, string fallbackUrls, string reportUrl = "")
        {
            // Create environment
            var env = EnvironmentGenerator(tagName, fallbackUrls, reportUrl);

            // Construct helper
            var fth = new FallbackTagHelper();

            // Apply attributes to emulate what ASP.NET Core does for us.
            fth.FallbackUrls = fallbackUrls;
            fth.FallbackReportUrl = reportUrl;

            // Initalise
            fth.Init(env.context);

            // Execute
            fth.Process(env.context, env.output);

            // Inspect output

            // Tag name should be equal to input
            Assert.Equal(tagName, env.output.TagName);
            // Attribute onerror should be equal.
            TagHelperAttribute onerrorResult;
            env.output.Attributes.TryGetAttribute("onerror", out onerrorResult);
            Assert.Equal($@"fallback(this, ""{fallbackUrls}"", ""{reportUrl}"");", onerrorResult.Value);
            // Attribute fallback-urls should not exist.
            Assert.False(env.output.Attributes.ContainsName("fallback-urls"));
            // Attribute fallback-report-url should not exist.
            Assert.False(env.output.Attributes.ContainsName("fallback-report-url"));
        }

        // Twice second
        //[Theory]
        public void FallbackUsedTwiceTest()
        {

        }
    }
}
