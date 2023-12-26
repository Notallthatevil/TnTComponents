using Microsoft.AspNetCore.Components;

namespace TnTComponents.Layout;

public partial class TnTSideNav {

    [Parameter]
    public override string? Class { get; set; } = "tnt-side-nav";

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    [Parameter]
    public string? Title { get; set; }

    [CascadingParameter]
    private TnTLayout _layout { get; set; } = default!;

    protected override void OnInitialized() {
        base.OnInitialized();
        if (_layout is null) {
            throw new InvalidOperationException($"{nameof(TnTHeader)} must be a descendant of {nameof(TnTLayout)}");
        }
        _layout.SetSideNav(this);
    }
}