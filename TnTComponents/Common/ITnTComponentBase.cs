using Microsoft.AspNetCore.Components;
using System.Text;

namespace TnTComponents.Common;

public interface ITnTComponentBase {
    IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }
    string? Class { get; set; }
    ElementReference Element { get; }
    string? Id { get; set; }
    string? Style { get; set; }
    string? Theme { get; set; }

    string GetClass();
}

internal static class ITnTComponentBaseExt {

    internal static string GetClassDefault(this ITnTComponentBase componentBase) {
        if (componentBase is null) return string.Empty;

        var strBuilder = new StringBuilder(componentBase.Class ?? string.Empty);

        if (componentBase.AdditionalAttributes?.TryGetValue("class", out var @class) == true) {
            strBuilder.Append(' ').AppendJoin(' ', @class);
        }

        return strBuilder.ToString();
    }
}