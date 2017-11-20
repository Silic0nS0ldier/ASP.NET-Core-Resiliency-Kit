const FS = require("fs");
const ReplaceInFile = require("replace-in-file");

// Fetch minified script.
let fallbackScript = FS.readFileSync("./fallback.js.cache");

// Delete cached script.
FS.unlinkSync("./fallback.js.cache");

// Replace key phrase with minified script.
ReplaceInFile.sync({
    files: "../FallbackTagHelperTest.cs",
    from: fallbackScript,
    to: "~~FALLBACK_SCRIPT_INJECTED_DURING_BUILD~~"
});
