using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TnTComponents.Common.Ext;
internal static partial class StringExt {

    public static string SplitPascalCase(this string str) {
        return PascalCaseSplit().Replace(str, " ");
    }

    [GeneratedRegex(@"(?<=[A-Z])(?=[A-Z][a-z])|(?<=[^A-Z])(?=[A-Z])|(?<=[A-Za-z])(?=[^A-Za-z])", RegexOptions.Compiled)]
    private static partial Regex PascalCaseSplit();
}

