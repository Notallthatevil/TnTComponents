using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace TnTComponents.Grid;

public partial class TnTPropertyColumn<TGridItem, TProperty> : TnTColumnBase<TGridItem> {
    private Expression<Func<TGridItem, TProperty>>? _lastUsedProperty;
    private Func<TGridItem, string?>? _cellContentFunc;

    [Parameter, EditorRequired]
    public Expression<Func<TGridItem, TProperty>> Property { get; set; } = default!;

    [Parameter]
    public string? Format { get; set; }

    protected override void OnParametersSet() {
        if (_lastUsedProperty != Property) {
            _lastUsedProperty = Property;
            var propFunc = Property.Compile();

            if (!string.IsNullOrWhiteSpace(Format)) {
                if (!typeof(IFormattable).IsAssignableFrom(Nullable.GetUnderlyingType(typeof(TProperty)) ?? typeof(TProperty))) {
                    throw new InvalidOperationException($"Unable to apply format '{Format}' to {typeof(TProperty)}. {typeof(TProperty)} does not implement {nameof(IFormattable)}");
                }

                _cellContentFunc = gridItem => (propFunc(gridItem) as IFormattable)?.ToString(Format, null);
            }
            else {
                _cellContentFunc = gridItem => propFunc(gridItem)?.ToString();
            }

            if (Title is null && Property.Body is MemberExpression expression) {
                Title = SplitOnCapitalLetters().Replace(expression.Member.Name, " $1");
            }
        }

        base.OnParametersSet();
    }

    protected internal override RenderFragment CellContent(TGridItem item) {
        return new RenderFragment(builder => builder.AddContent(0, _cellContentFunc!(item)));
    }

    [GeneratedRegex(@"((?<=\p{Ll})\p{Lu}|\p{Lu}(?=\p{Ll}))")]
    private static partial Regex SplitOnCapitalLetters();
}