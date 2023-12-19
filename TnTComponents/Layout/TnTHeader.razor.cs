using Microsoft.AspNetCore.Components;

namespace TnTComponents.Layout;
public partial class TnTHeader {

    [Parameter]
    public override string? Class { get; set; } = "tnt-header";

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    [CascadingParameter]
    private TnTLayout _layout { get; set; } = default!;

    protected override void OnInitialized() {
        base.OnInitialized();
        if (_layout is null) {
            throw new InvalidOperationException($"{nameof(TnTHeader)} must be a descendant of {nameof(TnTLayout)}");
        }
        _layout.SetHeader(this);
    }
}
