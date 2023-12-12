using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TnTComponents.Common;
using TnTComponents.Enum;
using TnTComponents.Events;

namespace TnTComponents.Grid.Infrastructure;
internal interface ITnTDataGridSettings<TGridItem> : ITnTComponentBase {
    IQueryable<TGridItem>? Items { get; set; }
    EventCallback<DataGridRowClickEventArgs> RowClickedCallback { get; }
    bool Virtualize { get; }
    string ContainerClass { get; }
    string CellContentContainerClass { get; }
    double? MaxHeight { get; }
    double? Height { get; }
    bool ShowRowIndex { get; }
    DataGridAppearance Appearance { get; }
    IconType IconType { get; }
    Expression<Func<TGridItem, object>>? DefaultSort { get; }
    string Name { get; }
}

