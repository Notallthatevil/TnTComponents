using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;
using System.Security.Claims;
using System.Text;
using TnTComponents.Common.Ext;
using TnTComponents.Enum;

namespace TnTComponents;

[CascadingTypeParameter(nameof(TGridItem))]
public partial class TnTQuickGrid<TGridItem>
{

    [Parameter]
    public DataGridAppearance Appearance { get; set; }
    [Parameter]
    public string GridContainerClass { get; set; } = "tnt-quick-grid-container";

    [Parameter]
    public double? Height { get; set; } = 0;

    [Inject]
    private IJSRuntime _jsRuntime { get; set; } = default!;

    private IJSObjectReference? _isolatedJsModule;


    protected override void OnParametersSet()
    {
        Theme = "TnTComponents";
        if (string.IsNullOrWhiteSpace(Class))
        {
            Class = "tnt-quick-grid";
        }
        var strBuilder = new StringBuilder(Class);

        if (Appearance.HasFlag(DataGridAppearance.Stripped))
        {
            strBuilder.Append(' ').Append(DataGridAppearance.Stripped.ToString().ToLower());
        }
        if (Appearance.HasFlag(DataGridAppearance.Compact))
        {
            strBuilder.Append(' ').Append(DataGridAppearance.Compact.ToString().ToLower());
        }
        else
        {
            strBuilder.Append(" tnt-resizable");
        }

        Class = strBuilder.ToString();
        base.OnParametersSet();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);
        _isolatedJsModule ??= await _jsRuntime.ImportIsolatedJs(this);
        if (firstRender)
        {
            await (_isolatedJsModule?.InvokeVoidAsync("onLoad") ?? ValueTask.CompletedTask);
        }
        await (_isolatedJsModule?.InvokeVoidAsync("onUpdate") ?? ValueTask.CompletedTask);
    }

    public new async ValueTask DisposeAsync()
    {
        try
        {
            if (_isolatedJsModule is not null)
            {
                await _isolatedJsModule.InvokeVoidAsync("onDispose");
                await _isolatedJsModule.DisposeAsync();
            }
        }
        catch (JSDisconnectedException) { }
        await base.DisposeAsync();
    }

    private string? GetContainerStyle()
    {
        if (Height.HasValue)
        {
            var strBuilder = new StringBuilder("height:");
            if (Height <= 1)
            {
                strBuilder.Append(Height * 100).Append('%');
            }
            else
            {
                strBuilder.Append(Height).Append("px");
            }
            strBuilder.Append(';');
            return strBuilder.ToString();
        }
        return null;
    }
}
