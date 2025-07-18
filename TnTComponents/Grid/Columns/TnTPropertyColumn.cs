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

/// <summary>
///     Represents a <see cref="TnTDataGrid{TGridItem}" /> column whose cells display a single value.
/// </summary>
/// <typeparam name="TGridItem">The type of data represented by each row in the grid.</typeparam>
/// <typeparam name="TProp">The type of the value being displayed in the column's cells.</typeparam>
public class TnTPropertyColumn<TGridItem, TProp> : TnTColumnBase<TGridItem>, IBindableColumn<TGridItem, TProp> {

    /// <summary>
    ///     Optionally specifies how to compare values in this column when sorting. Using this requires the <typeparamref name="TProp" /> type to implement <see cref="IComparable{T}" />.
    /// </summary>
    [Parameter]
    public IComparer<TProp>? Comparer { get; set; }

    /// <summary>
    ///     Optionally specifies a format string for the value. Using this requires the <typeparamref name="TProp" /> type to implement <see cref="IFormattable" />.
    /// </summary>
    [Parameter]
    public string? Format { get; set; }

    /// <summary>
    ///     The culture to use when formatting the value. This is only used if <see cref="Format" /> is set.
    /// </summary>
    [Parameter]
    public CultureInfo FormatCulture { get; set; } = CultureInfo.CurrentCulture;

    /// <summary>
    ///     Defines the value to be displayed in this column's cells.
    /// </summary>
    [Parameter, EditorRequired]
    public Expression<Func<TGridItem, TProp>> Property { get; set; } = default!;

    /// <summary>
    ///     The property info for the property being displayed in this column's cells.
    /// </summary>
    public PropertyInfo? PropertyInfo { get; private set; }

    /// <summary>
    ///     Not supported. This property is generated internally by the framework.
    /// </summary>
    public override TnTGridSort<TGridItem>? SortBy {
        get => _sortBuilder;
        set => throw new NotSupportedException($"PropertyColumn generates this member internally. For custom sorting rules, see '{typeof(TnTTemplateColumn<TGridItem>)}'.");
    }

    private readonly Func<TGridItem, string?>? _cellTooltipTextFunc = (item) => item?.ToString();
    private Func<TGridItem, string>? _cellTextFunc;
    private Expression<Func<TGridItem, TProp>>? _lastAssignedProperty;
    private TnTGridSort<TGridItem>? _sortBuilder;

    /// <inheritdoc />
    protected internal override void CellContent(RenderTreeBuilder builder, TGridItem item) => builder.AddContent(0, _cellTextFunc?.Invoke(item));

    /// <inheritdoc />
    protected internal override string? RawCellContent(TGridItem item) => _cellTooltipTextFunc?.Invoke(item);

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

            _sortBuilder = Comparer is not null ? TnTGridSort<TGridItem>.ByAscending(Property, Comparer) : TnTGridSort<TGridItem>.ByAscending(Property);
        }

        if (Title is null && Property.Body is MemberExpression memberExpression) {
            PropertyInfo = memberExpression.Member as PropertyInfo;
            var displayAttribute = Attribute.GetCustomAttribute(memberExpression.Member, typeof(DisplayAttribute)) as DisplayAttribute;

            var daText = displayAttribute?.Name;
            Title = !string.IsNullOrEmpty(daText) ? daText : memberExpression.Member.Name.SplitPascalCase();
        }
    }
}