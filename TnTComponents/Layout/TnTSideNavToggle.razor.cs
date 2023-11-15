using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace TnTComponents.Layout;
public partial class TnTSideNavToggle {
    [Parameter]
    public string? Label { get; set; }

    [Parameter]
    public override string Icon { get; set; } = "menu";

    [CascadingParameter]
    private TnTLayout _tntLayout { get; set; } = default!;

    protected override void OnInitialized() {
        if (_tntLayout is null) {
            throw new InvalidOperationException($"{nameof(TnTSideNavToggle)} must be a child of {nameof(TnTLayout)}");
        }
        base.OnInitialized();
    }

    protected override void OnAfterRender(bool firstRender) {
        if (firstRender) {
            Console.WriteLine("SideNav");
            StateHasChanged();
        }
        base.OnAfterRender(firstRender);
    }

    private void Toggle() {
        if (_tntLayout is not null) {
            _tntLayout.Expand = !_tntLayout.Expand;
        }
        _tntLayout?.Refresh();
    }
}
