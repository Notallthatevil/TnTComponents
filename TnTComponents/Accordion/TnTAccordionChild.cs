using Microsoft.AspNetCore.Components;
using TnTComponents.Core;

namespace TnTComponents;
public class TnTAccordionChild : TnTInteractableComponentBase, IDisposable {
    public override string? ElementClass => throw new NotImplementedException();
    public override string? ElementStyle => throw new NotImplementedException();

    [Parameter, EditorRequired]
    public string Title { get; set; } = default!;

    [Parameter]
    public bool OpenByDefault { get; set; }

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    [CascadingParameter]
    private TnTAccordion _parent { get; set; } = default!;

    protected override void OnInitialized() {
        base.OnInitialized();
        _parent.RegisterChild(this);
    }

    public void Dispose() {
        GC.SuppressFinalize(this);
        _parent.RemoveChild(this);
    }
}
