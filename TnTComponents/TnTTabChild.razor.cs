using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using TnTComponents.Common;
using TnTComponents.Enum;

namespace TnTComponents;

public partial class TnTTabChild : IDisposable {

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    [Parameter, EditorRequired]
    public string Title { get; set; } = default!;

    [Parameter]
    public RenderFragment? TabHeaderTemplate { get; set; }

    [Parameter]
    public string TabButtonClass { get; set; } = "tnt-tab-view-button";

    [Parameter]
    public override string? Class { get; set; } = "tnt-tab-view-content";

    [Parameter]
    public string? Icon { get; set; }

    [Parameter]
    public IconType IconType { get; set; }

    [Parameter]
    public bool Active { get; set; } = false;

    [Parameter]
    public bool Disabled { get; set; }

    [CascadingParameter]
    private TnTTabView _context { get; set; } = default!;

    [Parameter]
    public EventCallback<TnTTabChild> TabClickedCallback { get; set; }

    public ElementReference TabHeaderElement { get; set; }
    public int Index { get; set; }

    public void Dispose() {
        GC.SuppressFinalize(this);
        _context.RemoveTabChild(this);
    }

    public override string GetClass() => base.GetClass() + " " + (Active ? "active" : string.Empty);

    protected override async Task OnInitializedAsync() {
        await base.OnInitializedAsync();
        if (_context is null) {
            throw new InvalidOperationException($"A {nameof(TnTTabChild)} must be a child of {nameof(TnTTabView)}");
        }
        await _context.AddTabChildAsync(this);
    }

    protected override void OnAfterRender(bool firstRender) {
        base.OnAfterRender(firstRender);
        if (firstRender) {
            StateHasChanged();
        }
    }

    [JSInvokable]
    private void SetActive() {
        Console.WriteLine($"Set active {Title}");
    }

    public RenderFragment RenderTabHeader() {
        return new RenderFragment(builder => {
            builder.OpenElement(0, "button");
            builder.AddAttribute(5, "onclick", EventCallback.Factory.Create<MouseEventArgs>(this, async (args) => {
                await _context.SetActiveTab(this);
                await TabClickedCallback.InvokeAsync(this);
            }));
            builder.AddAttribute(10, "class", $"{TabButtonClass} {(_context.ActiveTab == this ? "active" : string.Empty)}");
            builder.AddAttribute(20, "disabled", Disabled);
            builder.AddAttribute(30, "type", "button");
            builder.AddAttribute(32, "name", Title.Replace(" ", string.Empty));

            if (TabHeaderTemplate is not null) {
                builder.AddContent(35, TabHeaderTemplate);
            }
            else {
                builder.AddContent(40, TnTIconComponent.RenderIcon(IconType, Icon));
                builder.AddContent(50, Title);
            }
            builder.AddElementReferenceCapture(60, e => TabHeaderElement = e);
            builder.CloseElement();
        });
    }
}