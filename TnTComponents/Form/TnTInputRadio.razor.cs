using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using TnTComponents;
using TnTComponents.Core;
using TnTComponents.Form;
using TnTComponents.Interfaces;

namespace TnTComponents;

/// <summary>
///     Represents a radio input component.
/// </summary>
/// <typeparam name="TInputType">The type of the input value.</typeparam>
public partial class TnTInputRadio<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TInputType> {

    /// <inheritdoc />
    [Parameter]
    public bool Disabled { get; set; }

    /// <inheritdoc />
    public override string? ElementClass => CssClassBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddClass("tnt-radio-input")
        .AddClass("tnt-interactable")
        .AddRipple(EnableRipple)
        .AddDisabled(_disabled)
        .Build();

    /// <inheritdoc />
    [Parameter]
    public string? ElementName { get; set; }

    /// <inheritdoc />
    public override string? ElementStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .Build();

    /// <inheritdoc />
    [Parameter]
    public bool EnableRipple { get; set; }

    /// <summary>
    ///     Gets or sets the icon displayed at the end of the radio input.
    /// </summary>
    [Parameter]
    public TnTIcon? EndIcon { get; set; }

    /// <summary>
    ///     Gets or sets the label for the radio input.
    /// </summary>
    [Parameter]
    public string? Label { get; set; }

    /// <inheritdoc />
    [Parameter]
    public TnTColor? OnTintColor { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether the radio input is read-only.
    /// </summary>
    [Parameter]
    public bool ReadOnly { get; set; }

    /// <summary>
    ///     Gets or sets the icon displayed at the start of the radio input.
    /// </summary>
    [Parameter]
    public TnTIcon? StartIcon { get; set; }

    /// <inheritdoc />
    [Parameter]
    public TnTColor? TintColor { get; set; }

    /// <summary>
    ///     Gets or sets the value of the radio input.
    /// </summary>
    [Parameter, EditorRequired]
    public TInputType Value { get; set; } = default!;

    private bool _disabled => _group.FieldDisabled || Disabled;

    /// <summary>
    ///     Gets or sets the cascading parameter for the radio group.
    /// </summary>
    [CascadingParameter]
    private TnTInputRadioGroup<TInputType> _group { get; set; } = default!;

    private bool _readOnly => _group.FieldReadonly || ReadOnly;
    private bool _trueValueToggle;

    /// <inheritdoc />
    protected override void OnParametersSet() {
        base.OnParametersSet();
        if (_group is null) {
            throw new ArgumentNullException($"{nameof(TnTInputRadio<TInputType>)} must be a child of {nameof(TnTInputRadioGroup<TInputType>)}. If you still receive this error, try explicitly setting the {nameof(TInputType)} of {nameof(TnTInputRadio<TInputType>)} to a Nullable type. This is likely the cause if {nameof(TInputType)} is struct or enum, as {nameof(TnTInputRadioGroup<TInputType>)} could be using the Nullable version ({nameof(TInputType)}?).");
        }
    }

    private string? GetValueAsString() => BindConverter.FormatValue(Value?.ToString());


    private string GetToggledTrueValue() {
        _trueValueToggle = !_trueValueToggle;
        return _trueValueToggle ? "a" : "b";
    }
}