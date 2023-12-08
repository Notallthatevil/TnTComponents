using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Xml.Linq;
using TnTComponents.Common;
using TnTComponents.Enum;

namespace TnTComponents.Forms;

[CascadingTypeParameter(nameof(TInputType))]
public partial class TnTRadioGroup<TInputType> {

    private TnTRadioButtonContext? _context;

    [Parameter]
    public Orientation Orientation { get; set; }

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    [Parameter]
    public string? Name { get; set; }

    protected override TInputType BindValue { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    protected override string InputType => throw new NotImplementedException();

    [Parameter]
    public override string? Class { get; set; } = "tnt-radio-group";

    [CascadingParameter]
    private TnTRadioButtonContext? _cascadedContext { get; set; }

    public override string GetClass() => $"{base.GetClass()} {Orientation.ToString().ToLower()} {GetFormType()}";


    protected override void OnParametersSet() {
        Name ??= FieldIdentifier.FieldName;
        if (_context is null) {
            var changeEventCallback = EventCallback.Factory.CreateBinder<string?>(this, __value => CurrentValueAsString = __value, CurrentValueAsString);
            _context = new TnTRadioButtonContext(_cascadedContext, changeEventCallback, Name);
        }
        else if (_context.ParentContext != _cascadedContext) {
            // This should never be possible in any known usage pattern, but if it happens, we want to know
            throw new InvalidOperationException($"A {nameof(TnTRadioGroup<TInputType>)} cannot change context after creation");
        }

        _context.CurrentValue = CurrentValue;
        _context.FieldClass = EditContext?.FieldCssClass(FieldIdentifier) ?? string.Empty;
        base.OnParametersSet();
    }

    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out TInputType result, [NotNullWhen(false)] out string? validationErrorMessage)
        => TryParseSelectableValueFromString(value, out result, out validationErrorMessage);

    private bool TryParseSelectableValueFromString<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TValue>(string? value, [MaybeNullWhen(false)] out TValue result, [NotNullWhen(false)] out string? validationErrorMessage) {
        try {
            if (typeof(TValue) == typeof(bool)) {
                if (TryConvertToBool<TValue>(value, out result)) {
                    validationErrorMessage = null;
                    return true;
                }
            }
            else if (typeof(TValue) == typeof(bool?)) {
                if (TryConvertToNullableBool<TValue>(value, out result)) {
                    validationErrorMessage = null;
                    return true;
                }
            }
            else if (BindConverter.TryConvertTo<TValue>(value, CultureInfo.CurrentCulture, out TValue value2)) {
                result = value2;
                validationErrorMessage = null;
                return true;
            }

            result = default(TValue);
            validationErrorMessage = "The " + (DisplayName ?? FieldIdentifier.FieldName) + " field is not valid.";
            return false;
        }
        catch (InvalidOperationException innerException) {
            throw new InvalidOperationException($"{GetType()} does not support the type '{typeof(TValue)}'.", innerException);
        }
    }

    private static bool TryConvertToBool<TValue>(string? value, out TValue result) {
        if (bool.TryParse(value, out var result2)) {
            result = (TValue)(object)result2;
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

        return TryConvertToBool<TValue>(value, out result);
    }
}
