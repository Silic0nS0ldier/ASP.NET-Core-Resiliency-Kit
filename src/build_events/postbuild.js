const FS = require("fs");
const ReplaceInFile = require("replace-in-file");

// Only run if file exists.
FS.existsSync("./fallback.min.js");

// Fetch and minify script.
let fallbackScript = FS.readFileSync("./fallback.min.js");

// Delete cached script.
FS.unlinkSync("./fallback.min.js");

// Replace key phrase with minified script.
ReplaceInFile.sync({
    files: "../FallbackTagHelper.cs",
    from: fallbackScript,
    to: "~~FALLBACK_SCRIPT_INJECTED_DURING_BUILD~~"
});
