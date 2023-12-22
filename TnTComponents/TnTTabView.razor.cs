using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using TnTComponents.Enum;

namespace TnTComponents;

public partial class TnTTabView {

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    [Parameter]
    public string TabHeaderClass { get; set; } = "tnt-tab-header";

    [Parameter]
    public string TabActiveIndicatorClass { get; set; } = "tnt-tab-active-indicator";

    [Parameter]
    public string TabContentClass { get; set; } = "tnt-tab-content";

    [Parameter]
    public override string? Class { get; set; } = "tnt-tab-view";

    [Parameter]
    public TabViewAppearance Appearance { get; set; }

    public TnTTabChild? ActiveTab { get; private set; }

    protected override bool HasIsolatedJs => true;

    private List<TnTTabChild> _tabChildren = [];

    protected override void OnAfterRender(bool firstRender) {
        base.OnAfterRender(firstRender);
        if (firstRender) {
            StateHasChanged();
        }
    }

    protected override async Task OnInitializedAsync() {
        await base.OnInitializedAsync();
        if (IsolatedJsModule is not null) {
            await IsolatedJsModule.InvokeVoidAsync("ensureInteractive", Element);
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender) {
        await base.OnAfterRenderAsync(firstRender);
        if (firstRender) {
            Console.WriteLine("Updating active for first render");
            await UpdateActiveIndicator();
        }
    }

    public async Task SetActiveTab(TnTTabChild? tabChild) {
        ActiveTab = tabChild;
        if (IsolatedJsModule is not null && ActiveTab is not null) {
            await IsolatedJsModule.InvokeVoidAsync("updateActiveIndex", Element, ActiveTab.Index);
        }
        await UpdateActiveIndicator();
        StateHasChanged();
    }

    public async Task AddTabChildAsync(TnTTabChild tabChild) {
        var index = _tabChildren.Count;
        _tabChildren.Add(tabChild);
        tabChild.Index = index;
        if (ActiveTab is null && !tabChild.Disabled) {
            await SetActiveTab(tabChild);
        }
    }

    public void RemoveTabChild(TnTTabChild tabChild) {
        _tabChildren.Remove(tabChild);
        var index = 0;
        foreach (var child in _tabChildren) {
            child.Index = index++;
        }
    }

    private async Task ButtonScrollListener(EventArgs _) {
        await UpdateActiveIndicator();
    }

    private async Task UpdateActiveIndicator() {
        if (IsolatedJsModule is not null) {
            await IsolatedJsModule.InvokeVoidAsync("updateActiveIndicator", Element, ActiveTab?.TabHeaderElement);
        }
    }

    public override string GetClass() => $"{base.GetClass()} {Appearance.ToString().ToLower()}";
}