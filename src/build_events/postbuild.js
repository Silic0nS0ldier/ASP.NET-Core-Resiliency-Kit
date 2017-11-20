const FS = require("fs");
const ReplaceInFile = require("replace-in-file");

// Fetch and minify script.
let fallbackScript = fs.readFileSync("./fallback.min.js");

// Delete cached script.
fs.unlinkSync("./fallback.min.js");

// Replace key phrase with minified script.
ReplaceInFile.sync({
    files: "../FallbackTagHelper.cs",
    from: fallbackScript,
    to: "~~FALLBACK_CODE~~"
});
