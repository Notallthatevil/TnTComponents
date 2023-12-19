﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TnTComponents.Common.Ext;

namespace TnTComponents.Grid.Columns;
public class TnTPropertyColumn<TGridItem, TProperty> : TnTColumnBase<TGridItem> {


    [Parameter, EditorRequired]
    public Expression<Func<TGridItem, TProperty>> Property { get; set; } = default!;

    [Parameter]
    public string? Format { get; set; }

    [Parameter]
    public override bool Sortable { get; set; } = true;

    [Parameter]
    public override Expression<Func<TGridItem, object>>? SortFunction { get; set; }


    private Expression<Func<TGridItem, TProperty>>? _lastUsedExpression;
    private Func<TGridItem, string?>? _cellValueFunc;


    protected override void OnParametersSet() {

        if (_lastUsedExpression != Property) {
            _lastUsedExpression = Property;
            var compiledPropExpression = Property.Compile();

            if (!string.IsNullOrWhiteSpace(Format)) {
                if (typeof(IFormattable).IsAssignableFrom(Nullable.GetUnderlyingType(typeof(TProperty)) ?? typeof(TProperty))) {
                    _cellValueFunc = gridItem => ((IFormattable?)compiledPropExpression!(gridItem))?.ToString(Format, null);
                }
            }

            _cellValueFunc ??= gridItem => compiledPropExpression!(gridItem)?.ToString();
        }

        if (ColumnHeader is null && Property.Body is MemberExpression member) {
            ColumnHeader = member.Member.Name.SplitPascalCase();
        }

        SortFunction ??= gridItem => Property.Compile().Invoke(gridItem);

        base.OnParametersSet();
    }

    internal override void RenderCellContent(RenderTreeBuilder __builder, TGridItem item) {
        __builder.AddContent(0, _cellValueFunc?.Invoke(item));
    }
}
