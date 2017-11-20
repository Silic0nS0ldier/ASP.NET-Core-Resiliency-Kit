const FS = require("fs");
const UglifyJS = require("uglify-js");
const ReplaceInFile = require("replace-in-file");

// Fetch, and minify script.
let fallbackScript = UglifyJS.minify(FS.readFileSync("../../src/fallback.js").toString()).code;

// Apply escape codes.
fallbackScript = fallbackScript.replace(/\\/g, "\\\\");/* \ */
fallbackScript = fallbackScript.replace(/"/g, "\\\"");/* " */

// Cache minified script for next build step.
FS.writeFileSync("./fallback.js.cache", fallbackScript);

// Replace key phrase with minified script.
ReplaceInFile.sync({
    files: "../FallbackTagHelperTest.cs",
    from: "~~FALLBACK_SCRIPT_INJECTED_DURING_BUILD~~",
    to: fallbackScript
});
