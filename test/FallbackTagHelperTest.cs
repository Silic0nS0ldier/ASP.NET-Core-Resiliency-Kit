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
        // data generator
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
        [Fact]
        [InlineData("link", "http://www.example.com/1.css~http://www.example.com/2.css")]
        [InlineData("script", "http://www.example.com/3.js", "http://www.example.com/report/resourcefailure/")]
        public void FallbackUsedOneTest(string tagName, string fallbackUrls, string reportUrl = "")
        {
            // Create environment
            var env = EnvironmentGenerator(tagName, fallbackUrls, reportUrl);


        }

        // Twice second
        [Fact]
        public void FallbackUsedTwiceTest()
        {

        }
    }
}
