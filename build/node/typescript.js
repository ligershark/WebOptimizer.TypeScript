var tsc = require("typescript");

module.exports = function (callback, input, filename) {
    var result = tsc.transpile(input,
        /* options  */ { allowJs: true, allowSyntheticDefaultImports: true, allowUnreachableCode: true, allowUnusedLabels: true, target: "ES5"},
        /* filename */ filename);
    callback(/* error */ null, result);
};