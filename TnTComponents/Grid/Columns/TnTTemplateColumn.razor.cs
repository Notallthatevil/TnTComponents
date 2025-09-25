using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System.Diagnostics.CodeAnalysis;
using TnTComponents.Grid;
using TnTComponents.Grid.Columns;

namespace TnTComponents;

/// <summary>
///     Represents a grid column that renders custom content for each item using a template.
/// </summary>
/// <typeparam name="TGridItem">The type of items displayed in the grid.</typeparam>
[CascadingTypeParameter(nameof(TGridItem))]
public partial class TnTTemplateColumn<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields)] TGridItem> {

    /// <summary>
    ///     Specifies the template to render for each grid item.
    /// </summary>
    [Parameter, EditorRequired]
    public RenderFragment<TGridItem> ChildContent { get; set; } = default!;

    /// <inheritdoc />
    public override string? ElementClass { get; }

    /// <inheritdoc />
    public override string? ElementStyle { get; }

    /// <inheritdoc />
    [Parameter]
    public override TnTGridSort<TGridItem>? SortBy { get; set; }
}