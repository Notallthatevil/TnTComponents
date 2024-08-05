using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using TnTComponents.Core;
using TnTComponents.Interfaces;

namespace TnTComponents;
public class TnTAccordionChild : ComponentBase, ITnTComponentBase, ITnTInteractable, IDisposable {
    [Parameter, EditorRequired]
    public string Label { get; set; } = default!;

    [Parameter]
    public bool OpenByDefault { get; set; }

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    [CascadingParameter]
    private TnTAccordion _parent { get; set; } = default!;

    [Inject]
    private IJSRuntime _jsRuntime { get; set; } = default!;
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }
    [Parameter]
    public bool? AutoFocus { get; set; }
    public ElementReference Element { get; internal set; }

    [Parameter]
    public string? ElementId { get; set; }
    [Parameter]
    public string? ElementLang { get; set; }
    [Parameter]
    public string? ElementTitle { get; set; }
    [Parameter]
    public bool Disabled { get; set; }
    [Parameter]
    public string? ElementName { get; set; }
    [Parameter]
    public bool EnableRipple { get; set; }
    [Parameter]
    public TnTColor? TintColor { get; set; }
    [Parameter]
    public TnTColor? OnTintColor { get; set; }
    public string? ElementClass => throw new NotSupportedException();
    public string? ElementStyle => throw new NotSupportedException();

    protected override void OnInitialized() {
        base.OnInitialized();
        _parent.RegisterChild(this);
    }

    public void Dispose() {
        GC.SuppressFinalize(this);
        _parent.RemoveChild(this);
    }

    public async Task CloseAsync() {
        await _jsRuntime.InvokeVoidAsync("TnTComponents.addHidden", Element);

    }
}
