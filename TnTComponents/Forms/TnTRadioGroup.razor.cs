//using Microsoft.AspNetCore.Components;
//using Microsoft.AspNetCore.Components.Forms;
//using System.Diagnostics.CodeAnalysis;
//using System.Globalization;
//using TnTComponents.Common;
//using TnTComponents.Enum;

//namespace TnTComponents.Forms;

//[CascadingTypeParameter(nameof(TInputType))]
//public partial class TnTRadioGroup<TInputType> {
//    [Parameter]
//    public RenderFragment ChildContent { get; set; } = default!;

//    [Parameter]
//    public EventCallback<ChangeEventArgs> CheckedChangedCallback { get; set; }

//    public readonly string GroupName = TnTComponentIdentifier.NewId();

//    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out TInputType result, [NotNullWhen(false)] out string? validationErrorMessage)
//    => TryParseSelectableValueFromString<TInputType>(value, out result, out validationErrorMessage);

//    private bool TryParseSelectableValueFromString<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TValue>(string? value, [MaybeNullWhen(false)] out TValue result, [NotNullWhen(false)] out string? validationErrorMessage) {
//        try {
//            if (typeof(TValue) == typeof(bool)) {
//                if (TryConvertToBool<TValue>(value, out result)) {
//                    validationErrorMessage = null;
//                    return true;
//                }
//            }
//            else if (typeof(TValue) == typeof(bool?)) {
//                if (TryConvertToNullableBool<TValue>(value, out result)) {
//                    validationErrorMessage = null;
//                    return true;
//                }
//            }
//            else if (BindConverter.TryConvertTo<TValue>(value, CultureInfo.CurrentCulture, out TValue value2)) {
//                result = value2;
//                validationErrorMessage = null;
//                return true;
//            }

//            result = default(TValue);
//            validationErrorMessage = "The " + (DisplayName ?? FieldIdentifier.FieldName) + " field is not valid.";
//            return false;
//        }
//        catch (InvalidOperationException innerException) {
//            throw new InvalidOperationException($"{GetType()} does not support the type '{typeof(TValue)}'.", innerException);
//        }
//    }

//    private static bool TryConvertToBool<TValue>(string value, out TValue result) {
//        if (bool.TryParse(value, out var result2)) {
//            result = (TValue)(object)result2;
//            return true;
//        }

//        result = default(TValue);
//        return false;
//    }

//    private static bool TryConvertToNullableBool<TValue>(string value, out TValue result) {
//        if (string.IsNullOrEmpty(value)) {
//            result = default(TValue);
//            return true;
//        }

//        return TryConvertToBool<TValue>(value, out result);
//    }
//}
