using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;

namespace TnTComponents.Forms;

public class TnTDateOnlyInput : TnTInputBase<DateOnly?> {

    [Parameter]
    public DateOnly? MaxDate { get; set; }

    [Parameter]
    public DateOnly? MinDate { get; set; }

    protected override DateOnly? BindValue { get => CurrentValue; set => CurrentValue = value; }

    protected override string GetInputType() => "date";

    protected override string? MaxString() => MaxDate?.ToString("yyyy-MM-dd");

    protected override string? MinString() => MinDate?.ToString("yyyy-MM-dd");

    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out DateOnly? result, [NotNullWhen(false)] out string? validationErrorMessage)
                => throw new NotSupportedException($"This component does not parse string inputs. Bind to the '{nameof(CurrentValue)}' property, not '{nameof(CurrentValueAsString)}'.");
}