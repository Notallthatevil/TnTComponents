using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;
using Microsoft.AspNetCore.Components.Rendering;
using System.Linq.Expressions;
using TnTComponents.Ext;
using TnTComponents.Grid;

namespace TnTComponents;

public partial class TnTPropertyColumn<TItem, TProperty> : TnTColumnBase<TItem> {
    [Parameter]
    public override bool Sortable { get; set; } = false;

    [Parameter]
    public string? Format { get; set; }

    [Parameter, EditorRequired]
    public Expression<Func<TItem, TProperty>> Property { get; set; } = default!;

    private Func<TItem, string?> _cellValueFunc = default!;


    protected override void OnInitialized() {
        base.OnInitialized();
        ArgumentNullException.ThrowIfNull(Property, nameof(Property));

        var compiled = Property.Compile()!;
        if (!string.IsNullOrWhiteSpace(Format)) {
            var underlyingType = Nullable.GetUnderlyingType(typeof(TProperty));
            if (!typeof(IFormattable).IsAssignableFrom(underlyingType ?? typeof(TProperty))) {
                throw new InvalidOperationException($"A '{nameof(Format)}' parameter was supplied, but the type '{typeof(TProperty)}' does not implement '{typeof(IFormattable)}'.");
            }

            _cellValueFunc = item => ((IFormattable)(object)(compiled(item) ?? default!))?.ToString(Format, null);
        }
        else {
            _cellValueFunc = item => compiled(item)?.ToString();
        }

        if (Title is null && Property.Body is MemberExpression expression) {
            Title = expression.Member.Name.SplitPascalCase();
        }
    }

    public override void RenderCellContent(RenderTreeBuilder builder, TItem item) {
        builder.AddContent(0, _cellValueFunc(item));
    }
}
