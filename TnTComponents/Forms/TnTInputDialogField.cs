using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Diagnostics.CodeAnalysis;
using TnTComponents.Common.Ext;

namespace TnTComponents.Forms;

public abstract class TnTInputDialogField<TInputType> : TnTInputField<TInputType> {

    [Parameter]
    public bool AllowClear { get; set; } = true;

    protected ElementReference ContainerElementReference { get; set; }
    protected bool HasSpaceBelow { get; private set; } = true;

    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out TInputType result, [NotNullWhen(false)] out string? validationErrorMessage)
               => throw new NotSupportedException($"This component does not parse string inputs. Bind to the '{nameof(CurrentValue)}' property, not '{nameof(CurrentValueAsString)}'.");

    protected override async Task OnFocusInAsync(FocusEventArgs e) {
        var rect = await JSRuntime.GetElementBoundingRect(ContainerElementReference);
        var windowHeight = await JSRuntime.GetWindowHeight();
        if (rect is not null) {
            var distanceToBottom = windowHeight - rect.Bottom;
            HasSpaceBelow = distanceToBottom >= await JSRuntime.ConvertRemToPixels(15);
        }
        await base.OnFocusInAsync(e);
    }

    protected async Task ClearSelection() {
        if (!Disabled) {
            CurrentValue = default;
        }
        await OnFocusOutAsync(new FocusEventArgs());
    }
}