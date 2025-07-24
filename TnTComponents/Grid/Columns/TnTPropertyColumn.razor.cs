using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using TnTComponents.Ext;
using TnTComponents.Grid;
using TnTComponents.Grid.Columns;
using TnTComponents.Grid.Infrastructure;

namespace TnTComponents;


[CascadingTypeParameter(nameof(TGridItem))]
public partial class TnTPropertyColumn<TGridItem, TProp>  {

    [Parameter]
    public IComparer<TProp>? Comparer { get; set; }

    [Parameter]
    public string? Format { get; set; }

    [Parameter]
    public CultureInfo FormatCulture { get; set; } = CultureInfo.CurrentCulture;

    [Parameter, EditorRequired]
    public Expression<Func<TGridItem, TProp>> Property { get; set; } = default!;
    public PropertyInfo? PropertyInfo { get; private set; }

    public override TnTGridSort<TGridItem>? SortBy {
        get => _sortBuilder;
        set => throw new NotSupportedException($"{nameof(TnTPropertyColumn<TGridItem, TProp>)} generates this member internally. For custom sorting rules, see '{typeof(TnTTemplateColumn<TGridItem>)}'.");
    }

    //private readonly Func<TGridItem, string?>? _cellTooltipTextFunc = (item) => item?.ToString();
    private Func<TGridItem, string>? _cellTextFunc;
    private Expression<Func<TGridItem, TProp>>? _lastAssignedProperty;
    private TnTGridSort<TGridItem>? _sortBuilder;

    /// <inheritdoc />
    protected override void OnParametersSet() {
        // We have to do a bit of pre-processing on the lambda expression. Only do that if it's new or changed.
        if (_lastAssignedProperty != Property) {
            _lastAssignedProperty = Property;
            var compiledPropertyExpression = Property.Compile();

            if (!string.IsNullOrEmpty(Format)) {
                var nullableUnderlyingTypeOrNull = Nullable.GetUnderlyingType(typeof(TProp));
                if (!typeof(IFormattable).IsAssignableFrom(nullableUnderlyingTypeOrNull ?? typeof(TProp))) {
                    throw new InvalidOperationException($"A '{nameof(Format)}' parameter was supplied, but the type '{typeof(TProp)}' does not implement '{typeof(IFormattable)}'.");
                }

                _cellTextFunc = item => {
                    var result = ((IFormattable?)compiledPropertyExpression!(item))?.ToString(Format, FormatCulture);
                    return !string.IsNullOrEmpty(result) ? result : " ";
                };
            }
            else {
                _cellTextFunc = item => {
                    var result = compiledPropertyExpression!(item)?.ToString();
                    return !string.IsNullOrEmpty(result) ? result : " ";
                };
            }

            _sortBuilder ??= Comparer is not null ? TnTGridSort<TGridItem>.ByAscending(Property, Comparer) : TnTGridSort<TGridItem>.ByAscending(Property);
        }

        if (Title is null && Property.Body is MemberExpression memberExpression) {
            PropertyInfo = memberExpression.Member as PropertyInfo;
            var displayAttribute = Attribute.GetCustomAttribute(memberExpression.Member, typeof(DisplayAttribute)) as DisplayAttribute;

            var daText = displayAttribute?.Name;
            Title = !string.IsNullOrEmpty(daText) ? daText : memberExpression.Member.Name.SplitPascalCase();
        }

        // Call after to register sorting if needed.
        base.OnParametersSet();
    }

    public override string? ElementClass => throw new NotImplementedException();

    public override string? ElementStyle => throw new NotImplementedException();

}