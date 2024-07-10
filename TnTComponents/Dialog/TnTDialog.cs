using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using TnTComponents.Core;
using TnTComponents.Dialog;

namespace TnTComponents;

public class TnTDialog : ComponentBase, IDisposable {

    [Inject]
    private TnTDialogService _service { get; set; } = default!;

    private readonly List<ITnTDialog> _dialogs = [];

    public void Dispose() {
        _service.OnClose -= OnClose;
        _service.OnOpen -= OnOpen;
        GC.SuppressFinalize(this);
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        foreach (var dialog in _dialogs) {
            builder.OpenElement(0, "div");
            builder.AddAttribute(10, "class", dialog.Options.GetOverlayClass());
            if (dialog == _dialogs.Last()) {
                string style;
                if (dialog.Options.OverlayOpacity is not null) {
                    style = $"background-color:color-mix(in srgb, var(--tnt-color-{dialog.Options.OverlayColor.ToCssClassName()}) {Math.Clamp(dialog.Options.OverlayOpacity.Value * 100, 0, 100)}%, transparent);";
                }
                else {
                    style = $"background-color:var(--tnt-color-{dialog.Options.OverlayColor.ToCssClassName()});";
                }
                if (dialog.Options.OverlayBlur) {
                    style += $"backdrop-filter:blur(0.25rem);";
                }

                if (!string.IsNullOrWhiteSpace(style)) {
                    builder.AddAttribute(20, "style", style);
                }
            }

            builder.OpenElement(30, "div");
            builder.SetKey(dialog);
            builder.AddAttribute(40, "class", dialog.Options.GetDialogClass());
            builder.AddAttribute(50, "style", dialog.Options.Style);

            if (dialog == _dialogs.Last()) {
                builder.OpenComponent<TnTExternalClickHandler>(60);
                builder.AddComponentParameter(70, nameof(TnTExternalClickHandler.ExternalClickCallback), EventCallback.Factory.Create(this, dialog.CloseAsync));
                builder.AddComponentParameter(80, nameof(TnTExternalClickHandler.ChildContent), RenderDialogContent(dialog));
                builder.CloseComponent();
            }
            else {
                builder.AddContent(60, RenderDialogContent(dialog));
            }


            builder.CloseElement();

            builder.CloseElement();

        }
    }

    private RenderFragment RenderDialogContent(ITnTDialog dialog) {
        return new RenderFragment(builder => {
            var showDivider = false;
            {

                builder.OpenElement(0, "div");
                builder.AddAttribute(10, "class", "tnt-dialog-header");
                {
                    if (dialog.Options.ShowTitle && dialog.Options.Title is not null) {
                        showDivider = true;
                        builder.OpenElement(20, "h2");
                        builder.AddContent(30, dialog.Options.Title);
                        builder.CloseElement();
                    }

                    if (dialog.Options.ShowClose) {
                        showDivider = true;
                        builder.OpenComponent<TnTImageButton>(40);
                        builder.AddComponentParameter(50, nameof(TnTImageButton.Icon), new MaterialIcon(MaterialIcon.Close));
                        builder.AddComponentParameter(60, nameof(TnTImageButton.OnClick), EventCallback.Factory.Create<MouseEventArgs>(this, dialog.CloseAsync));
                        builder.CloseComponent();
                    }
                }
                builder.CloseElement();
            }

            if (showDivider) {
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
                    // Disabling warning since value in these key value pairs is allowed to
                    // be null when the parameter on the component allows it. It is up to
                    // the caller when opening a dialog to set the parameters correctly.
                    cascadingBuilder.AddMultipleAttributes(10, dialog.Parameters);
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
                    cascadingBuilder.CloseComponent();
                }));

                builder.CloseComponent();
            }
        });
    }

    protected override void OnInitialized() {
        base.OnInitialized();
        _service.OnOpen += OnOpen;
        _service.OnClose += OnClose;
    }
    private Task OnClose(ITnTDialog dialog) {
        dialog.Options.Closing = true;
        _dialogs.Remove(dialog);
        StateHasChanged();
        return Task.CompletedTask;
    }

    private Task OnOpen(ITnTDialog dialog) {
        _dialogs.Add(dialog);
        StateHasChanged();
        return Task.CompletedTask;
    }
}