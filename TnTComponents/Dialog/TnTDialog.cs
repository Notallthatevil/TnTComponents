using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using TnTComponents.Core;
using TnTComponents.Dialog;

namespace TnTComponents;

public class TnTDialog : ComponentBase, IDisposable {

    [Inject]
    private ITnTDialogService _service { get; set; } = default!;

    private readonly List<ITnTDialog> _dialogs = [];

    [Inject]
    private IJSRuntime _jsRuntime { get; set; } = default!;

    public void Dispose() {
        _service.OnClose -= OnClose;
        _service.OnOpen -= OnOpen;
        GC.SuppressFinalize(this);
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        foreach (var dialog in _dialogs) {
            builder.OpenElement(0, "dialog");
            builder.AddAttribute(10, "class", CssClassBuilder.Create().AddTnTStyleable(dialog.Options).AddClass("tnt-closing", dialog.Options.Closing).AddClass(dialog.Options.ElementClass).AddClass("tnt-dialog").Build());
            builder.AddAttribute(20, "style", CssStyleBuilder.Create().AddStyle(dialog.Options.ElementStyle, string.Empty).Build());
            builder.AddAttribute(30, "id", dialog.ElementId);
            builder.AddAttribute(40, "oncancel", EventCallback.Factory.Create<EventArgs>(this, dialog.CloseAsync));
            builder.SetKey(dialog);

            if (dialog == _dialogs.Last() && dialog.Options.CloseOnExternalClick) {
                builder.OpenComponent<TnTExternalClickHandler>(60);
                builder.AddComponentParameter(70, nameof(TnTExternalClickHandler.ExternalClickCallback), EventCallback.Factory.Create(this, dialog.CloseAsync));
                builder.AddComponentParameter(80, nameof(TnTExternalClickHandler.ChildContent), RenderDialogContent(dialog));
                builder.CloseComponent();
            }
            else {
                builder.AddContent(60, RenderDialogContent(dialog));
            }

            builder.CloseElement();
        }
    }

    protected override void OnInitialized() {
        base.OnInitialized();
        _service.OnOpen += OnOpen;
        _service.OnClose += OnClose;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender) {
        await base.OnAfterRenderAsync(firstRender);
        foreach (var dialog in _dialogs) {
            await _jsRuntime.InvokeVoidAsync("TnTComponents.openModalDialog", dialog.ElementId);
        }
    }

    private async Task OnClose(ITnTDialog dialog) {
        dialog.Options.Closing = true;
        StateHasChanged();
        await Task.Delay(150);
        _dialogs.Remove(dialog);
        StateHasChanged();
    }

    private Task OnOpen(ITnTDialog dialog) {
        _dialogs.Add(dialog);
        StateHasChanged();
        return Task.CompletedTask;
    }

    private RenderFragment RenderDialogContent(ITnTDialog dialog) {
        return new RenderFragment(builder => {
            if (dialog.Options.Title is not null || dialog.Options.ShowCloseButton) {
                builder.OpenElement(0, "div");
                builder.AddAttribute(10, "class", "tnt-dialog-header");
                {
                    if (dialog.Options.Title is not null) {
                        builder.OpenElement(20, "h2");
                        builder.AddContent(30, dialog.Options.Title);
                        builder.CloseElement();
                    }

                    if (dialog.Options.ShowCloseButton) {
                        builder.OpenComponent<TnTImageButton>(40);
                        builder.AddComponentParameter(50, nameof(TnTImageButton.Icon), new MaterialIcon{ Icon = MaterialIcon.Close });
                        builder.AddComponentParameter(60, nameof(TnTImageButton.OnClickCallback), EventCallback.Factory.Create<MouseEventArgs>(this, dialog.CloseAsync));
                        builder.CloseComponent();
                    }
                }
                builder.CloseElement();

                builder.OpenComponent<TnTDivider>(70);
                builder.CloseComponent();
            }

            {
                builder.OpenComponent<CascadingValue<ITnTDialog>>(80);
                builder.AddAttribute(150, nameof(CascadingValue<ITnTDialog>.Value), dialog);
                builder.AddAttribute(160, nameof(CascadingValue<ITnTDialog>.IsFixed), true);
                builder.AddAttribute(170, nameof(CascadingValue<ITnTDialog>.ChildContent), new RenderFragment(cascadingBuilder => {
                    cascadingBuilder.OpenComponent(0, dialog.Type);
#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
                    // Disabling warning since value in these key value pairs is allowed to be null
                    // when the parameter on the component allows it. It is up to the caller when
                    // opening a dialog to set the parameters correctly.
                    cascadingBuilder.AddMultipleAttributes(10, dialog.Parameters);
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
                    cascadingBuilder.CloseComponent();
                }));

                builder.CloseComponent();
            }
        });
    }
}