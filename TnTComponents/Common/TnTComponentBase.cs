using Microsoft.AspNetCore.Components;

namespace TnTComponents.Common;

public abstract class TnTComponentBase : ComponentBase, ITnTComponentBase {

    [Parameter(CaptureUnmatchedValues = true)]
    public virtual IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    [Parameter]
    public virtual string? Class { get; set; }

    [Parameter]
    public virtual object? Data { get; set; }

    public ElementReference Element { get; protected set; }

    [Parameter]
    public virtual string? Id { get; set; }

    [Parameter]
    public virtual string? Style { get; set; }

    [Parameter]
    public virtual string? Theme { get; set; }

    protected bool Interactive { get; set; }

    [Parameter]
    public string ComponentIdentifier { get; set; } = TnTComponentIdentifier.NewId();

    public virtual string GetClass() => this.GetClassDefault();


    protected override void OnAfterRender(bool firstRender) {
        base.OnAfterRender(firstRender);
        if (firstRender) {
            Interactive = true;
        }
    }

    protected override void OnParametersSet() {
        base.OnParametersSet();

        var dict = AdditionalAttributes is not null ? AdditionalAttributes.ToDictionary() : new Dictionary<string, object>();
        dict.TryAdd(ComponentIdentifier, string.Empty);
        AdditionalAttributes = dict;
    }
}