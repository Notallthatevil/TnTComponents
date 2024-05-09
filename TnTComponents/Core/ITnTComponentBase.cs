using Microsoft.AspNetCore.Components;
using System.Text;

namespace TnTComponents.Core;

/// <summary>
/// Simple interface that represents a TnTComponent. Each TnTComponent can be thought of as a single
/// Html Element.
/// </summary>
public interface ITnTComponentBase {

    /// <summary>
    /// Contains all attributes provided to this component that do not match a parameter.
    /// </summary>
    IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    /// <summary>
    /// Gets or sets the automatic focus. <see
    /// cref="https://developer.mozilla.org/en-US/docs/Web/HTML/Global_attributes/autofocus" />
    /// </summary>
    bool? AutoFocus { get; set; }

    /// <summary>
    /// The base css class that should be provided to this component. <see
    /// cref="https://developer.mozilla.org/en-US/docs/Web/HTML/Global_attributes/class" />
    /// </summary>
    string? CssClass { get; }

    /// <summary>
    /// Additional styles that can be applied to this component. <see
    /// cref="https://developer.mozilla.org/en-US/docs/Web/HTML/Global_attributes/style" />
    /// </summary>
    string? CssStyle { get; }

    bool Disabled { get; set; }

    /// <summary>
    /// A reference to the main html tag that this component is made of.
    /// </summary>
    ElementReference Element { get; }

    /// <summary>
    /// If set, applies this Id to the main html tag for this component. <see
    /// cref="https://developer.mozilla.org/en-US/docs/Web/HTML/Global_attributes/id" />
    /// </summary>
    string? Id { get; }
}

internal static class ITnTComponentBaseExt {

    internal static string GetClassDefault(this ITnTComponentBase componentBase) {
        if (componentBase is null) return string.Empty;

        var strBuilder = new StringBuilder(componentBase.CssClass ?? string.Empty);

        if (componentBase.AdditionalAttributes?.TryGetValue("class", out var @class) == true) {
            strBuilder.Append(' ').AppendJoin(' ', @class);
        }

        return strBuilder.ToString();
    }
}