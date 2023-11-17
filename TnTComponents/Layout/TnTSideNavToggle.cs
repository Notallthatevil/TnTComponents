using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using TnTComponents.Common;

namespace TnTComponents.Layout;

public class TnTSideNavToggle : TnTIconComponent {

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

    protected override void BuildRenderTree(RenderTreeBuilder __builder) {
        if (_tntLayout?.SideNav is not null) {
            __builder.OpenElement(0, "button");
            __builder.AddMultipleAttributes(5, AdditionalAttributes);
            __builder.AddAttribute(10, "type", "button");
            __builder.AddAttribute(20, "class", GetCssClass());
            __builder.AddAttribute(30, "theme", Theme);
            if (_tntLayout.Interactive) {
                __builder.AddAttribute(40, "onclick", EventCallback.Factory.Create<MouseEventArgs>(this, (_) => Toggle()));
            }
            else {
                __builder.AddAttribute(50, "onclick", new MarkupString($"(function() {{let e = document.getElementById('{_tntLayout.TnTSideNavId}'); if(e) {{ if(e.classList.contains('{TnTLayout.ExpandClass}')) {{ e.classList.remove('{TnTLayout.ExpandClass}');{CreateToggleForId(_tntLayout.TntHeaderId, false)}{CreateToggleForId(_tntLayout.TntBodyId, false)}{CreateToggleForId(_tntLayout.TntFooterId, false)}}} else {{ e.classList.add('{TnTLayout.ExpandClass}'); {CreateToggleForId(_tntLayout.TntHeaderId, true)}{CreateToggleForId(_tntLayout.TntBodyId, true)}{CreateToggleForId(_tntLayout.TntFooterId, true)}}} }} }})()"));
            }

            __builder.AddContent(60, GetIcon());
            __builder.AddContent(70, Label);
            __builder.CloseElement();
        }
    }

    private MarkupString CreateToggleForId(string? id, bool expand) {
        if (!string.IsNullOrWhiteSpace(id)) {
            return new MarkupString($"let elem{id}=document.getElementById('{id}');if(elem{id}){{elem{id}.classList.{(expand ? "add" : "remove")}('{TnTLayout.ExpandClass}');}}");
        }
        return default;
    }

    private void Toggle() {
        if (_tntLayout is not null) {
            _tntLayout.Expand = !_tntLayout.Expand;
        }
        _tntLayout?.Refresh();
    }
}