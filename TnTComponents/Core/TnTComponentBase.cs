using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using TnTComponents.Ext;
using TnTComponents.Interfaces;

namespace TnTComponents.Core;

/// <summary>
/// Base component containing all common logic.
/// </summary>
/// <seealso cref="Microsoft.AspNetCore.Components.ComponentBase" />
/// <seealso cref="Interfaces.ITnTComponentBase" />
/// <seealso cref="System.IAsyncDisposable" />
public abstract class TnTComponentBase : ComponentBase, ITnTComponentBase {

    [Parameter(CaptureUnmatchedValues = true)]
    /// <inheritdoc />
    public virtual IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    /// <inheritdoc />
    [Parameter]
    public bool? AutoFocus { get; set; }

    public string ComponentIdentifier { get; } = TnTComponentIdentifier.NewId();
    /// <inheritdoc />
    public abstract string? CssClass { get; }
    /// <inheritdoc />
    public abstract string? CssStyle { get; }

    [Parameter]
    public bool Disabled { get; set; }

    public ElementReference Element { get; protected set; }

    /// <inheritdoc />
    [Parameter]
    public virtual string? Id { get; set; }

    [Parameter]
    public TextAlign? TextAlign { get; set; }

    internal const string TnTCustomIdentifierAttribute = "tntid";

    protected bool Interactive { get; private set; }

    protected override async Task OnAfterRenderAsync(bool firstRender) {
        await base.OnAfterRenderAsync(firstRender);
        if (firstRender) {
            Interactive = true;
        }
    }

    protected override void OnParametersSet() {
        base.OnParametersSet();

        var dict = AdditionalAttributes is not null ? AdditionalAttributes.ToDictionary() : [];
        dict.TryAdd(TnTCustomIdentifierAttribute, ComponentIdentifier);
        AdditionalAttributes = dict;
    }
}