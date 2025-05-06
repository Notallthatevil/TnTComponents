using Microsoft.AspNetCore.Components;
using System.Text;

namespace TnTComponents.Interfaces;

/// <summary>
///     Simple interface that represents a TnTComponent. Each TnTComponent can be thought of as a single Html Element.
/// </summary>
public interface ITnTComponentBase {

    /// <summary>
    ///     Contains all attributes provided to this component that do not match a parameter.
    /// </summary>
    IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    /// <summary>
    ///     Gets or sets the automatic focus. <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Global_attributes/autofocus" />
    /// </summary>
    bool? AutoFocus { get; set; }

    /// <summary>
    ///     A reference to the main html tag that this component is made of.
    /// </summary>
    ElementReference Element { get; }

    /// <summary>
    ///     The base css class that should be provided to the root element of this component. <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Global_attributes/class" />
    /// </summary>
    string? ElementClass { get; }

    /// <summary>
    ///     If set, applies this Id to the root html tag for this component. <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Global_attributes/id" />
    /// </summary>
    string? ElementId { get; }

    /// <summary>
    ///     The language of the root html tag for this component. <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Global_attributes/lang" />
    /// </summary>
    string? ElementLang { get; }

    /// <summary>
    ///     Additional styles that can be applied to the root element for this component. <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Global_attributes/style" />
    /// </summary>
    string? ElementStyle { get; }

    /// <summary>
    ///     The title of the root html tag for this component. <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Global_attributes/title" />
    /// </summary>
    string? ElementTitle { get; }
}