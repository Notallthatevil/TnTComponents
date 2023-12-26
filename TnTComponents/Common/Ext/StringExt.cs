﻿using System.Text.RegularExpressions;

namespace TnTComponents.Common.Ext;

internal static partial class StringExt {

    public static string SplitPascalCase(this string str, string replacement = " ") {
        return PascalCaseSplit().Replace(str, replacement);
    }

    [GeneratedRegex(@"(?<=[A-Z])(?=[A-Z][a-z])|(?<=[^A-Z])(?=[A-Z])|(?<=[A-Za-z])(?=[^A-Za-z])", RegexOptions.Compiled)]
    private static partial Regex PascalCaseSplit();
}