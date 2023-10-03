using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TnTComponents.Common.Ext;

namespace TnTComponents.Forms;
public abstract class TnTFormDialogField<TInputType> : TnTFormField<TInputType> {

    [Parameter]
    public string? DisabledProperty { get; set; }

    [Parameter]
    public string? TextProperty { get; set; }

    protected ElementReference BoxInputReference { get; set; }
    protected ElementReference InputElementReference { get; set; }
    protected bool HasSpaceBelow { get; private set; }

    [Inject]
    protected IJSRuntime JSRuntime { get; set; } = default!;

    private PropertyInfo? _textProperty;
    private PropertyInfo? _disabledProperty;


    protected override void OnInitialized() {
        if (!typeof(TInputType).IsPrimitive && typeof(TInputType) != typeof(string) && string.IsNullOrWhiteSpace(TextProperty)) {
            throw new ArgumentNullException(nameof(TextProperty), $"When specifying a non-primitive type and non-string type. The {TextProperty} must be provided!");
        }

        if (!string.IsNullOrWhiteSpace(TextProperty)) {
            _textProperty = typeof(TInputType).GetProperty(TextProperty);
            if (_textProperty?.PropertyType != typeof(string)) {
                throw new InvalidOperationException($"{nameof(TextProperty)} must reflect a property of type string");
            }
        }

        if (!string.IsNullOrWhiteSpace(DisabledProperty)) {
            _disabledProperty = typeof(TInputType).GetProperty(DisabledProperty);
            if (_disabledProperty?.PropertyType != typeof(bool)) {
                throw new InvalidOperationException($"{nameof(DisabledProperty)} must reflect a property of type bool");
            }
        }

        base.OnInitialized();
    }

    protected async Task ClearSelection() {
        if (!Disable) {
            await Deactivate();
            Value = default;
            await OnChange(new ChangeEventArgs() { Value = Value });
        }
    }

    protected async Task Deactivate() {
        await JSRuntime.RemoveElementFocus(InputElementReference);
        Active = false;
    }

    protected async Task OnFocusAsync() {
        var rect = await JSRuntime.GetElementBoundingRectForComponent(BoxInputReference);
        var windowHeight = await JSRuntime.GetWindowHeight();
        if (rect is not null) {
            var distanceToBottom = windowHeight - rect.Bottom;
            HasSpaceBelow = distanceToBottom >= await JSRuntime.ConvertRemToPixels(15);
        }
        Active = true;
    }

    protected async Task Focus() {
        if (!Disable) {
            await JSRuntime.SetElementFocus(InputElementReference);
            await OnFocusAsync();
        }
    }

    protected bool IsItemDisabled(TInputType item) {
        return _disabledProperty is not null && (((bool?)_disabledProperty.GetValue(item)) ?? false);
    }

    protected string GetItemClass(TInputType item) {
        return IsItemDisabled(item) ? "disabled" : string.Empty;
    }

    protected object? GetItemValue(TInputType? item) {
        if (_textProperty is not null && item != null) {
            return _textProperty.GetValue(item);
        }
        return item?.ToString() ?? string.Empty;
    }
}
