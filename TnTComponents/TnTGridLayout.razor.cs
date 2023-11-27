using Microsoft.AspNetCore.Components;
using System.Text;
using TnTComponents.Common;

namespace TnTComponents;

public partial class TnTGridLayout {

    [Parameter]
    public string? Id { get; set; }

    [Parameter]
    public string? Class { get; set; } = "tnt-grid-layout";

    [Parameter]
    public string? Theme { get; set; }

    [Parameter]
    public string? Style { get; set; }

    [Parameter]
    public object? Data { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    [Parameter]
    public int? ColumnCount { get; set; }

    [Parameter]
    public int? RowCount { get; set; }

    public ElementReference Element { get; protected set; }

    public string GetClass() => this.GetClassDefault();

    private string GetStyle() {
        var strBuilder = new StringBuilder();

        if (ColumnCount.HasValue) {
            strBuilder.Append(' ').Append("grid-template-columns:");
            if (ColumnCount.Value == 0) {
                strBuilder.Append(" auto;");
            }
            else {
                var percentage = 100.0 / ColumnCount.Value;
                for (var i = 0; i < ColumnCount.Value; ++i) {
                    strBuilder.Append($" {percentage}%");
                }
                strBuilder.Append(";");
            }
        }

        if (RowCount.HasValue) {
            strBuilder.Append(' ').Append("grid-template-rows:");
            if (RowCount.Value == 0) {
                strBuilder.Append(" auto;");
            }
            else {
                var percentage = 100.0 / RowCount.Value;
                for (var i = 0; i < RowCount.Value; ++i) {
                    strBuilder.Append($" {percentage}%");
                }
                strBuilder.Append(";");
            }
        }

        return strBuilder.ToString();
    }
}