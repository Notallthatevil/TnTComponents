using Microsoft.AspNetCore.Components;
using System.Text;
using TnTComponents.Enum;

namespace TnTComponents;

public partial class TnTProgressIndicator {

    [Parameter]
    public ProgressAppearance Appearance { get; set; }

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public override string? Class { get; set; } = "tnt-progress-indicator";

    [Parameter]
    public double Max { get; set; } = 100.0;

    [Parameter]
    public double? Value { get; set; }

    public override string GetClass() => base.GetClass() + " " + Appearance.ToString().ToLower();

    private string GetStyle() {
        var stringBuilder = new StringBuilder(Style);

        if (Value is not null) {
            var deg = 360 * (Value / Max);
            stringBuilder.Append($"background: conic-gradient(from 0deg, currentColor {deg}deg, transparent {deg}deg);");
        }

        return stringBuilder.ToString();
    }
}