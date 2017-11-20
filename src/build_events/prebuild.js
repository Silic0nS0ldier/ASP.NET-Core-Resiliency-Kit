const FS = require("fs");
const UglifyJS = require("uglify-js");
const ReplaceInFile = require("replace-in-file");

// Fetch and minify script.
let fallbackScript = UglifyJS.minify(fs.readFileSync("../fallback.js"));

// Cache minified script for next build step.
fs.writeFileSync("./fallback.min.js", fallbackScript);

// Replace key phrase with minified script.
ReplaceInFile.sync({
    files: "../FallbackTagHelper.cs",
    from: "~~FALLBACK_CODE~~",
    to: fallbackScript
});
