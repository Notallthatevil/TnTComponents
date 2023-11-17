using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using TnTComponents.Common;
using TnTComponents.Common.Ext;
using TnTComponents.Infrastructure;

namespace TnTComponents;

public partial class TnTTabView {

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    [Parameter]
    public override string BaseCssClass { get; set; } = "tnt-tab-view";

    [Parameter]
    public bool UseGlider { get; set; } = true;

    private TabViewContext _tabViewContext = default!;

    private ElementReference _gliderRef;

    [Inject]
    private IJSRuntime _jsRuntime { get; set; } = default!;

    public override string GetCssClass() {
        return base.GetCssClass() + (UseGlider ? " tnt-glider" : string.Empty);
    }

    protected override void OnInitialized() {
        _tabViewContext = new TabViewContext(this);
        base.OnInitialized();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender) {
        if (!firstRender) {
            if (_tabViewContext.ActiveTab is not null) {
                var offset = await _jsRuntime.GetElementOffset(_tabViewContext.ActiveTabElementRef);
                var boundingRect = await _jsRuntime.GetElementBoundingRect(_tabViewContext.ActiveTabElementRef);
                var newRect = new ElementBoundingRect();
                if (UseGlider) {
                    newRect.Left = offset.OffsetLeft;
                    newRect.Width = boundingRect.Width;
                    newRect.Height = boundingRect.Height - await _jsRuntime.ConvertRemToPixels(0.5);
                    newRect.Top = offset.OffsetTop + await _jsRuntime.ConvertRemToPixels(0.25);
                }
                else {
                    var threeRem = await _jsRuntime.ConvertRemToPixels(3);
                    newRect.Width = boundingRect.Width - threeRem;
                    newRect.Left = offset.OffsetLeft + (threeRem / 2);
                    newRect.Height = 4;
                    newRect.Top = boundingRect.Height - 4;
                }
                await _jsRuntime.SetElementBoundingRect(_gliderRef, newRect);
            }
        }
        await base.OnAfterRenderAsync(firstRender);
    }

    public void Refresh() {
        StateHasChanged();
    }
}