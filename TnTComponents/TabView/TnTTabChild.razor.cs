using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using TnTComponents.Common;
using TnTComponents.Core;
using TnTComponents.Enum;

namespace TnTComponents;

public partial class TnTTabChild
{

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    [Parameter, EditorRequired]
    public string Title { get; set; } = default!;

    [Parameter]
    public RenderFragment? TabHeaderTemplate { get; set; }

    public override string? Class => CssBuilder.Create()
        .SetDisabled(Disabled)
        .Build();

    [Parameter]
    public TnTIcon? Icon { get; set; }

    [CascadingParameter]
    private TnTTabView _context { get; set; } = default!;

    private string? _style;

    protected override void OnInitialized()
    {
        base.OnInitialized();
        if (_context is null)
        {
            throw new InvalidOperationException($"A {nameof(TnTTabChild)} must be a child of {nameof(TnTTabView)}");
        }
        _context.AddTabChild(this);
        _style = $"{Style}; display:none;";
    }

    protected override void OnAfterRender(bool firstRender)
    {
        base.OnAfterRender(firstRender);
        _style = Style;
    }

    //[JSInvokable]
    //private void SetActive() {
    //    Console.WriteLine($"Set active {Title}");
    //}

    //public RenderFragment RenderTabHeader() {
    //    return new RenderFragment(builder => {
    //        builder.OpenElement(0, "button");
    //        builder.AddAttribute(5, "onclick", EventCallback.Factory.Create<MouseEventArgs>(this, async (args) => {
    //            await _context.SetActiveTab(this);
    //            await TabClickedCallback.InvokeAsync(this);
    //        }));
    //        builder.AddAttribute(10, "class", $"{TabButtonClass} {(_context.ActiveTab == this ? "active" : string.Empty)}");
    //        builder.AddAttribute(20, "disabled", Disabled);
    //        builder.AddAttribute(30, "type", "button");
    //        builder.AddAttribute(32, "name", Title.Replace(" ", string.Empty));

    //        if (TabHeaderTemplate is not null) {
    //            builder.AddContent(35, TabHeaderTemplate);
    //        }
    //        else {
    //            builder.AddContent(40, TnTIconComponent.RenderIcon(IconType, Icon));
    //            builder.AddContent(50, Title);
    //        }
    //        builder.AddElementReferenceCapture(60, e => TabHeaderElement = e);
    //        builder.CloseElement();
    //    });
    //}
}