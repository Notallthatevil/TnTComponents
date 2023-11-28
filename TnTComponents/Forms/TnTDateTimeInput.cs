using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;

namespace TnTComponents.Forms;

public class TnTDateTimeInput : TnTInputBase<DateTime?> {

    [Parameter]
    public DateTime? MaxDate { get; set; }

    [Parameter]
    public DateTime? MinDate { get; set; }

    protected override DateTime? BindValue { get => CurrentValue; set => CurrentValue = value; }

    protected override string InputType => "datetime-local";

    protected override string? Max => MaxDate?.ToString("O");

    protected override string? Min => MinDate?.ToString("O");

    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out DateTime? result, [NotNullWhen(false)] out string? validationErrorMessage)
        => throw new NotSupportedException($"This component does not parse string inputs. Bind to the '{nameof(CurrentValue)}' property, not '{nameof(CurrentValueAsString)}'.");
}