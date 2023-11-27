using Microsoft.AspNetCore.Components;
using System.Text;
using TnTComponents.Common;
using TnTComponents.Enum;

namespace TnTComponents;

public partial class TnTRow {

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    [Parameter]
    public string? Class { get; set; } = "tnt-row";

    [Parameter]
    public object? Data { get; set; }

    public ElementReference Element { get; protected set; }

    [Parameter]
    public AlignContent FlexAlignContent { get; set; }

    [Parameter]
    public AlignItems FlexAlignItems { get; set; }

    [Parameter]
    public JustifyContent FlexJustifyContent { get; set; }

    [Parameter]
    public WrapStyle FlexWrap { get; set; }

    [Parameter]
    public string? Id { get; set; }

    [Parameter]
    public string? Style { get; set; }

    [Parameter]
    public string? Theme { get; set; }

    public string GetClass() => this.GetClassDefault();

    private string? GetStyle() {
        var strBuilder = new StringBuilder(FlexWrap.ToStyle())
            .Append(' ').Append(FlexAlignContent.ToStyle())
            .Append(' ').Append(FlexJustifyContent.ToStyle())
            .Append(' ').Append(FlexAlignItems.ToStyle());

        if (AdditionalAttributes?.TryGetValue("style", out var style) ?? false) {
            strBuilder.AppendJoin(' ', style);
        }
        var result = strBuilder.ToString();
        return !string.IsNullOrWhiteSpace(result) ? result : null;
    }
}