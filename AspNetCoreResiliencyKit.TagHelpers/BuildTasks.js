import { minify } from "uglify-js";
import { readFileSync, writeFileSync } from "fs";
import { sync as replaceInFile } from "replace-in-file";

//
switch (process.argv[2]) {
    case "minify":
        Minify();
        break;
    case "insert":
        InsertInSource();
        break;
    case "remove":
        RemoveFromSource();
        break;
}


/**
 * Minify the fallback scrpt and save the result
 * process.argv[3] - Input path
 * process.argv[4] - Output path
 */
function Minify() {
    // Read and minify script
    let fallbackScript = minify(readFileSync(process.argv[3]).toString());
    // Ensure minified successfully
    if (fallbackScript.error !== undefined) throw `Failed to minify ${process.argv[3]}`;
    // Escape "
    fallbackScript = fallbackScript.code.replace(/"/g, '""');
    // Save
    writeFileSync(process.argv[4], fallbackScript);
}

/**
 * Inserts script into ResilienceTagHelper
 * process.argv[3] - File to replace line in
 * process.argv[4] - File to read replacement text from
 */
function InsertInSource() {
    replaceInFile({
        files: process.argv[3],
        from: "~~FallbackScriptInjectedDuringBuild~~",
        to: readFileSync(process.argv[4]).toString()
    });
}

/**
 * Reverts previous script insertion
 * process.argv[3] - File to replace line in
 * process.argv[4] - File to read text to replace from
 */
function RemoveFromSource() {
    replaceInFile({
        files: process.argv[3],
        from: readFileSync(process.argv[4]).toString(),
        to: "~~FallbackScriptInjectedDuringBuild~~"
    });
}
