using Microsoft.AspNetCore.Components;

namespace TnTComponents.Common;

public abstract class TnTComponentBase : ComponentBase, ITnTComponentBase {
    public ElementReference Element { get; protected set; }

    [Parameter]
    public virtual string? Id { get; set; }

    [Parameter]
    public virtual string? Class { get; set; }

    [Parameter]
    public virtual string? Theme { get; set; }

    [Parameter]
    public virtual string? Style { get; set; }

    [Parameter]
    public virtual object? Data { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public virtual IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    protected bool Interactive { get; set; }

    public virtual string GetClass() => this.GetClassDefault();

    protected override void OnParametersSet() {
        if (string.IsNullOrWhiteSpace(Id)) {
            Id = Guid.NewGuid().ToString();
        }
        base.OnParametersSet();
    }

    protected override void OnAfterRender(bool firstRender) {
        base.OnAfterRender(firstRender);
        if (firstRender) {
            Interactive = true;
        }
    }
}