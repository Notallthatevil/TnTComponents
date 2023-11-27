using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace TnTComponents.Forms;

public partial class TnTRadioGroup<TInputType> {
    private bool _disabled;

    private RadioGroupContext _radioGroupContext = default!;

    [Parameter]
    public bool AllowClear { get; set; }

    [Parameter]
    public override string BaseCssClass { get; set; } = "tnt-radio-group";

    [Parameter]
    public override bool Disabled { get => _disabled; set { _disabled = value; if (_radioGroupContext is not null) { _radioGroupContext.Disabled = _disabled; } } }

    [Parameter]
    public ICollection<TInputType>? RadioButtonItems { get; set; }

    [Parameter]
    public RenderFragment RadioButtons { get; set; } = default!;

    protected override void OnParametersSet() {
        if (RadioButtons is null && (RadioButtonItems is null || RadioButtonItems.Count != 0)) {
            throw new ArgumentNullException(nameof(RadioButtons), "Must provide at least one radio button within this group. This can be done by providing a valid " +
                $"{nameof(TnTRadioButton<TInputType>)} child element within the {nameof(RadioButtons)} tag, or by providing {nameof(RadioButtonItems)}");
        }

        string groupName = default!;
        if (!string.IsNullOrEmpty(Label)) {
            groupName = Label;
        }
        else if (!string.IsNullOrEmpty(NameAttributeValue)) {
            groupName = NameAttributeValue;
        }
        else {
            groupName = Guid.NewGuid().ToString("N");
        }

        var changeEventCallback = EventCallback.Factory.CreateBinder<string?>(this, value => CurrentValueAsString = value, CurrentValueAsString);
        _radioGroupContext = new RadioGroupContext(groupName, GetLabelCssClass(), changeEventCallback) {
            CurrentValue = CurrentValue,
            Disabled = Disabled
        };

        base.OnParametersSet();
    }

    /// <inheritdoc />
    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out TInputType result, [NotNullWhen(false)] out string? validationErrorMessage) {
        try {
            if (typeof(TInputType) == typeof(bool)) {
                if (TryConvertToBool(value, out result)) {
                    validationErrorMessage = null;
                    return true;
                }
            }
            else if (typeof(TInputType) == typeof(bool?)) {
                if (TryConvertToNullableBool(value, out result)) {
                    validationErrorMessage = null;
                    return true;
                }
            }
            else if (BindConverter.TryConvertTo<TInputType>(value, CultureInfo.CurrentCulture, out var parsedValue)) {
                result = parsedValue;
                validationErrorMessage = null;
                return true;
            }

            result = default;
            validationErrorMessage = $"The {DisplayName ?? FieldIdentifier.FieldName} field is not valid.";
            return false;
        }
        catch (InvalidOperationException ex) {
            throw new InvalidOperationException($"{GetType()} does not support the type '{typeof(TInputType)}'.", ex);
        }
    }

    private static bool TryConvertToBool<TValue>(string? value, out TValue result) {
        if (bool.TryParse(value, out var @bool)) {
            result = (TValue)(object)@bool;
            return true;
        }

        result = default!;
        return false;
    }

    private static bool TryConvertToNullableBool<TValue>(string? value, out TValue result) {
        if (string.IsNullOrEmpty(value)) {
            result = default!;
            return true;
        }

        return TryConvertToBool(value, out result);
    }
}

internal class RadioGroupContext(string groupName, string labelCss, EventCallback<ChangeEventArgs> eventCallback) {
    public object? CurrentValue { get; set; }
    public bool Disabled { get; set; }
    public EventCallback<ChangeEventArgs> EventCallback { get; } = eventCallback;
    public string GroupName { get; } = groupName;
    public string LabelCss { get; } = labelCss;
}