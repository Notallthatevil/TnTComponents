using Microsoft.AspNetCore.Components;
using System.Text;

namespace TnTComponents.Common;

public interface ITnTComponentBase {
    string? Id { get; set; }
    string? Class { get; set; }
    string? Theme { get; set; }
    string? Style { get; set; }
    object? Data { get; set; }
    IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }
    ElementReference Element { get; }

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