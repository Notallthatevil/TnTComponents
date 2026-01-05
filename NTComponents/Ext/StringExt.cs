using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace NTComponents.Ext;

[ExcludeFromCodeCoverage]
internal static partial class StringExt {

    public static string SplitPascalCase(this string str, string replacement = " ") => PascalCaseSplit().Replace(str, replacement);

    [GeneratedRegex(@"(?<=[A-Z])(?=[A-Z][a-z])|(?<=[^A-Z])(?=[A-Z])|(?<=[A-Za-z])(?=[^A-Za-z])", RegexOptions.Compiled)]
    private static partial Regex PascalCaseSplit();
}