using Microsoft.AspNetCore.Components;
using System.Text;
using TnTComponents.Enum;

namespace TnTComponents;

public partial class TnTDivider {

    [Parameter]
    public Orientation Orientation { get; set; } = Orientation.Horizontal;

    [Parameter]
    public int? Size { get; set; }

    [Parameter]
    public bool Percentage { get; set; } = true;

    [Parameter]
    public string Class { get; set; } = "tnt-divider";

    protected override void OnInitialized() {
        base.OnInitialized();

        if (Size < 0) {
            throw new InvalidOperationException($"{nameof(Size)} must be greater then 0!");
        }

        if (Percentage & Size > 100) {
            throw new InvalidOperationException($"When {nameof(Percentage)} is true, {nameof(Size)} must be between 0 and 100.");
        }
    }

    private string GetStyle() {
        var stringBuilder = new StringBuilder();

        stringBuilder.Append(Orientation == Orientation.Horizontal ? "height: " : "width: ")
            .Append("1px;");

        if (Size.HasValue) {
            stringBuilder.Append(Orientation == Orientation.Horizontal ? "width: " : "height: ")
                .Append(Size.Value).Append(Percentage ? "%" : "px").Append(';');
        }

        stringBuilder.Append(Orientation == Orientation.Horizontal ? "border-top-width: " : "border-left-width: ")
            .Append("1px;");

        return stringBuilder.ToString();
    }
}