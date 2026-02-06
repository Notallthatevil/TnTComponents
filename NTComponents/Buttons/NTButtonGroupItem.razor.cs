using System;
using System.ComponentModel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using NTComponents.Core;

namespace NTComponents;

/// <summary>
///     Represents a button description that registers itself with <see cref="NTButtonGroup{TObjectType}" />.
/// </summary>
public sealed partial class NTButtonGroupItem<TObjectType> : ComponentBase, IDisposable {
    private NTButtonGroup<TObjectType>? _registeredParent;


    /// <summary>
    ///     The parent button group that owns this item.
    /// </summary>
    [CascadingParameter]
    public NTButtonGroup<TObjectType>? Parent { get; set; }

    /// <summary>
    ///     A unique key used to identify the item inside the group.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public TObjectType Key { get; set; } = default!;

    /// <summary>
    ///     Optional label text; when unset, the button may render as an icon-only control.
    /// </summary>
    [Parameter]
    public string? Label { get; set; }

    /// <summary>
    ///     Optional icon rendered before the label.
    /// </summary>
    [Parameter]
    public TnTIcon? StartIcon { get; set; }

    /// <summary>
    ///     Optional icon rendered after the label.
    /// </summary>
    [Parameter]
    public TnTIcon? EndIcon { get; set; }

    /// <summary>
    ///     Marks the item as disabled.
    /// </summary>
    [Parameter]
    public bool Disabled { get; set; }

    /// <summary>
    ///     Indicates whether this item should be selected by default when no other selection exists.
    /// </summary>
    [Parameter]
    public bool IsDefaultSelected { get; set; }

    /// <inheritdoc/>
    protected override void OnParametersSet() {
        base.OnParametersSet();

        if (Parent is null) {
            throw new InvalidOperationException($"{nameof(NTButtonGroupItem<TObjectType>)} must be used inside {nameof(NTButtonGroup<TObjectType>)}.");
        }

        if (!ReferenceEquals(_registeredParent, Parent)) {
            _registeredParent?.UnregisterItem(this);
        }

        Parent.RegisterItem(this);
        _registeredParent = Parent;
    }

    /// <inheritdoc/>
    public void Dispose() {
        _registeredParent?.UnregisterItem(this);
        _registeredParent = null;
    }

    /// <inheritdoc/>
    protected override void BuildRenderTree(RenderTreeBuilder builder) { }
}
