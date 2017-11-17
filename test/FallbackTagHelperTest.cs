using System;
using Xunit;
using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Fallback.AspNetCore.Test
{
    public class FallbackTagHelperTest
    {
        // data generator
        /// <summary>
        /// Generates a clean environment for testing the tag helper.
        /// </summary>
        /// <param name="tagName">Name of HTML tag.</param>
        /// <param name="fallbackUrl">Fallback URLs delimited with '~'.</param>
        /// <param name="reportUrl">Optional. URL to be used to report fallback use and failure.</param>
        /// <returns></returns>
        private static (TagHelperContext context, TagHelperOutput output) EnvironmentGenerator(string tagName, string fallbackUrl, string reportUrl = "", IDictionary<object, object> items = null)
        {
            var allAttributes = new TagHelperAttributeList();
            items = items ?? new Dictionary<object, object>();
            var context = new TagHelperContext(tagName, allAttributes, items, "id");
            throw new NotImplementedException();
        }

        // Single first
        [Fact]
        public void FallbackUsedOneTest()
        {
            var env = EnvironmentGenerator("link", "http://www.example.com/1.css~http://www.example.com/2.css");
        }

        // Twice second
        [Fact]
        public void FallbackUsedTwiceTest()
        {

        }
    }
}
