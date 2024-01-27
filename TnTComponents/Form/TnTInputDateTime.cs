using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TnTComponents;
public class TnTInputDateTime : TnTInputBase<DateTime?> {
    public override InputType Type => InputType.DateTime;
    [Parameter]
    public string Format { get; set; } = "MM/dd/yyyy hh:mm:ss";

    protected override void OnInitialized() {
        base.OnInitialized();
        var attributes = AdditionalAttributes is null ? new Dictionary<string, object>() : new Dictionary<string, object>(AdditionalAttributes);
        attributes.Add("format", "YYYY-MM-DDThh:mm");
        AdditionalAttributes = attributes;

    }

    protected override string? FormatValueAsString(DateTime? value) => BindConverter.FormatValue(value, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture);

    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out DateTime? result, [NotNullWhen(false)] out string? validationErrorMessage) {
        if (BindConverter.TryConvertTo(value, CultureInfo.InvariantCulture, out result)) {
            Debug.Assert(result != null);
            validationErrorMessage = null;
            return true;
        }
        else {
            validationErrorMessage = $"Failed to parse {value} into a {typeof(DateTime).Name}";
            return false;
        }
    }
}

public class TnTInputDateOnly : TnTInputBase<DateOnly?> {
    public override InputType Type => InputType.Date;
    [Parameter]
    public string Format { get; set; } = "MM/dd/yyyy hh:mm:ss";

    protected override void OnInitialized() {
        base.OnInitialized();
        var attributes = AdditionalAttributes is null ? new Dictionary<string, object>() : new Dictionary<string, object>(AdditionalAttributes);
        attributes.Add("format", "YYYY-MM-DDThh:mm");
        AdditionalAttributes = attributes;

    }

    protected override string? FormatValueAsString(DateOnly? value) => BindConverter.FormatValue(value, "yyyy-MM-dd", CultureInfo.InvariantCulture);

    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out DateOnly? result, [NotNullWhen(false)] out string? validationErrorMessage) {
        if (BindConverter.TryConvertTo(value, CultureInfo.InvariantCulture, out result)) {
            Debug.Assert(result != null);
            validationErrorMessage = null;
            return true;
        }
        else {
            validationErrorMessage = $"Failed to parse {value} into a {typeof(DateOnly).Name}";
            return false;
        }
    }
}

public class TnTInputTimeOnly : TnTInputBase<TimeOnly?> {
    public override InputType Type => InputType.Time;
    [Parameter]
    public string Format { get; set; } = "MM/dd/yyyy hh:mm:ss";

    protected override void OnInitialized() {
        base.OnInitialized();
        var attributes = AdditionalAttributes is null ? new Dictionary<string, object>() : new Dictionary<string, object>(AdditionalAttributes);
        attributes.Add("format", "YYYY-MM-DDThh:mm");
        AdditionalAttributes = attributes;

    }

    protected override string? FormatValueAsString(TimeOnly? value) => BindConverter.FormatValue(value, "HH:mm:ss", CultureInfo.InvariantCulture);

    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out TimeOnly? result, [NotNullWhen(false)] out string? validationErrorMessage) {
        if (BindConverter.TryConvertTo(value, CultureInfo.InvariantCulture, out result)) {
            Debug.Assert(result != null);
            validationErrorMessage = null;
            return true;
        }
        else {
            validationErrorMessage = $"Failed to parse {value} into a {typeof(TimeOnly).Name}";
            return false;
        }
    }
}

public class TnTInputMonth : TnTInputBase<DateOnly?> {
    public override InputType Type => InputType.Month;
    [Parameter]
    public string Format { get; set; } = "MM/dd/yyyy hh:mm:ss";

    protected override void OnInitialized() {
        base.OnInitialized();
        var attributes = AdditionalAttributes is null ? new Dictionary<string, object>() : new Dictionary<string, object>(AdditionalAttributes);
        attributes.Add("format", "YYYY-MM-DDThh:mm");
        AdditionalAttributes = attributes;

    }

    protected override string? FormatValueAsString(DateOnly? value) => BindConverter.FormatValue(value, "yyyy-MM", CultureInfo.InvariantCulture);

    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out DateOnly? result, [NotNullWhen(false)] out string? validationErrorMessage) {
        if (BindConverter.TryConvertTo(value, CultureInfo.InvariantCulture, out result)) {
            Debug.Assert(result != null);
            validationErrorMessage = null;
            return true;
        }
        else {
            validationErrorMessage = $"Failed to parse {value} into a {typeof(DateOnly).Name}";
            return false;
        }
    }
}