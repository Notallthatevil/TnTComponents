using Microsoft.AspNetCore.Components;
using TnTComponents.Interfaces;

namespace TnTComponents.Core;

/// <summary>
///     Base component containing all common logic.
/// </summary>
/// <seealso cref="ComponentBase" />
/// <seealso cref="ITnTComponentBase" />
/// <seealso cref="IAsyncDisposable" />
public abstract class TnTComponentBase : ComponentBase, ITnTComponentBase {

    [Parameter(CaptureUnmatchedValues = true)]
    public virtual IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    /// <inheritdoc />
    [Parameter]
    public bool? AutoFocus { get; set; }

    /// <summary>
    ///     Unique identifier for the component.
    /// </summary>
    public string ComponentIdentifier { get; } = TnTComponentIdentifier.NewId();

    /// <inheritdoc />
    public ElementReference Element { get; protected set; }

    /// <inheritdoc />
    public abstract string? ElementClass { get; }

    /// <inheritdoc />
    [Parameter]
    public virtual string? ElementId { get; set; }

    [Parameter]
    public string? ElementLang { get; set; }

    /// <inheritdoc />
    public abstract string? ElementStyle { get; }

    [Parameter]
    public string? ElementTitle { get; set; }

    /// <summary>
    ///     Custom identifier attribute for the component.
    /// </summary>
    internal const string TnTCustomIdentifierAttribute = "tntid";

    protected override void OnParametersSet() {
        base.OnParametersSet();

        var dict = AdditionalAttributes is not null ? AdditionalAttributes.ToDictionary() : new Dictionary<string, object>();
        dict.TryAdd(TnTCustomIdentifierAttribute, ComponentIdentifier);
        AdditionalAttributes = dict;
    }
}