using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;

namespace TnTComponents.Forms;

public partial class TnTDateTimeInput {

    [Parameter]
    public DateTime? MinDate { get; set; }

    [Parameter]
    public DateTime? MaxDate { get; set; }

    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out DateTime? result, [NotNullWhen(false)] out string? validationErrorMessage)
        => throw new NotSupportedException($"This component does not parse string inputs. Bind to the '{nameof(CurrentValue)}' property, not '{nameof(CurrentValueAsString)}'.");
}