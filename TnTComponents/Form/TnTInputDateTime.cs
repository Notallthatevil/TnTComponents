using Microsoft.AspNetCore.Components;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace TnTComponents;

public class TnTInputDateTime<DateTimeType> : TnTInputBase<DateTimeType> {

    [Parameter]
    public string Format { get; set; } = default!;

    [Parameter]
    public bool MonthOnly { get; set; }

    public override InputType Type => _type;

    private InputType _type;

    protected override string? FormatValueAsString(DateTimeType? value) => (Nullable.GetUnderlyingType(typeof(DateTimeType)) ?? typeof(DateTimeType)) switch {
        var t when t == typeof(DateTime) => BindConverter.FormatValue(value as DateTime?, _format, CultureInfo.InvariantCulture),
        var t when t == typeof(DateTimeOffset) => BindConverter.FormatValue(value as DateTimeOffset?, _format, CultureInfo.InvariantCulture),
        var t when t == typeof(TimeOnly) => BindConverter.FormatValue(value as TimeOnly?, _format, CultureInfo.InvariantCulture),
        var t when t == typeof(DateOnly) => BindConverter.FormatValue(value as DateOnly?, _format, CultureInfo.InvariantCulture),
        _ => throw new InvalidOperationException($"The type '{typeof(DateTimeType)}' is not a supported DateTime type.")
    };

    private string _format = default!;

    protected override void OnInitialized() {
        base.OnInitialized();

        _format = (Nullable.GetUnderlyingType(typeof(DateTimeType)) ?? typeof(DateTimeType)) switch {
            var t when t == typeof(DateTime) => "yyyy-MM-ddTHH:mm:ss",
            var t when t == typeof(DateTimeOffset) => "yyyy-MM-ddTHH:mm:ss",
            var t when t == typeof(TimeOnly) => "HH:mm:ss",
            var t when t == typeof(DateOnly) => MonthOnly ? "yyyy-MM" : "yyyy-MM-dd",
            _ => throw new InvalidOperationException($"The type '{typeof(DateTimeType)}' is not a supported DateTime type.")
        };

        _type = (Nullable.GetUnderlyingType(typeof(DateTimeType)) ?? typeof(DateTimeType)) switch {
            var t when t == typeof(DateTime) => InputType.DateTime,
            var t when t == typeof(DateTimeOffset) => InputType.DateTime,
            var t when t == typeof(TimeOnly) => InputType.Time,
            var t when t == typeof(DateOnly) => MonthOnly ? InputType.Month : InputType.Date,
            _ => throw new InvalidOperationException($"The type '{typeof(DateTimeType)}' is not a supported DateTime type.")
        };

        var attributes = AdditionalAttributes is null ? new Dictionary<string, object>() : new Dictionary<string, object>(AdditionalAttributes);
        attributes.Add("format", _format);
        AdditionalAttributes = attributes;
    }

    protected override void OnParametersSet() {
        base.OnParametersSet();
        if (string.IsNullOrWhiteSpace(Format)) {
            Format = _format;
        }
    }

    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out DateTimeType result, [NotNullWhen(false)] out string? validationErrorMessage) {
        if (BindConverter.TryConvertTo(value, CultureInfo.InvariantCulture, out result)) {
            Debug.Assert(result != null);
            validationErrorMessage = null;
            return true;
        }
        else {
            validationErrorMessage = $"Failed to parse {value} into a {typeof(DateTimeType).Name}";
            return false;
        }
    }
}