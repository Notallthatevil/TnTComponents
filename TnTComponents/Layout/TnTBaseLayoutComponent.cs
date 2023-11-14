using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TnTComponents.Infrastructure;

namespace TnTComponents.Layout;
public abstract class TnTBaseLayoutComponent : ComponentBase {

    [Parameter]
    public virtual string BaseCssClass { get; set; } = string.Empty;

    [Parameter]
    public string Theme { get; set; } = "default";

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    [CascadingParameter]
    internal TnTLayoutContext Context { get; set; } = default!;

    protected bool Expanded { get; set; }

    protected string Id { get; } = Guid.NewGuid().ToString();
    protected ElementReference Element { get; set; }

    protected override void OnInitialized() {
        if (Context is null) {
            throw new InvalidOperationException($"{GetType().Name} must be a child of {nameof(TnTLayout)}!");
        }
        AddSelfToContext();
        base.OnInitialized();
    }

    protected abstract void AddSelfToContext();

    protected string GetCssClass() {
        var strBuilder = new StringBuilder(BaseCssClass);

        if (AdditionalAttributes?.TryGetValue("class", out var classList) == true) {
            strBuilder.Append(' ').AppendJoin(' ', classList);
        }

        return strBuilder.ToString();
    }
}

