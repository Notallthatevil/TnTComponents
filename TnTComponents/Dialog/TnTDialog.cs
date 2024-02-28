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
            builder.SetKey(dialog);

            {
                builder.OpenComponent<TnTExternalClickHandler>(40);
                builder.AddComponentParameter(50, "ExternalClickCssClass", dialog.Options.GetDialogClass());
                builder.AddAttribute(60, "style", dialog.Options.Style);

                if (dialog.Options.CloseOnExternalClick) {
                    builder.AddComponentParameter(70, "ExternalClickCallback", EventCallback.Factory.Create(this, async () => await _service.CloseAsync(dialog)));
                }

                builder.AddComponentParameter(80, "ChildContent", new RenderFragment(innerBuilder => {
                    var showDivider = false;
                    if (dialog.Options.ShowClose) {
                        showDivider = true;
                        innerBuilder.OpenComponent<TnTImageButton>(0);
                        innerBuilder.AddComponentParameter(10, "Icon", MaterialIcon.Close);
                        innerBuilder.AddComponentParameter(20, "OnClickCallback", EventCallback.Factory.Create<MouseEventArgs>(this, async _ => await _service.CloseAsync(dialog)));
                        innerBuilder.CloseComponent();
                    }

                    if (dialog.Options.ShowTitle && dialog.Options.Title is not null) {
                        showDivider = true;
                        innerBuilder.OpenElement(30, "h2");
                        innerBuilder.AddContent(40, dialog.Options.Title);
                        innerBuilder.CloseElement();
                    }

                    if (showDivider) {
                        innerBuilder.OpenComponent<TnTDivider>(50);
                        innerBuilder.CloseComponent();
                    }

                    {
                        innerBuilder.OpenComponent<CascadingValue<ITnTDialog>>(52);
                        innerBuilder.AddAttribute(60, "Value", dialog);
                        innerBuilder.AddAttribute(70, "IsFixed", true);
                        innerBuilder.AddAttribute(80, "ChildContent", new RenderFragment(cascadingBuilder => {
                            cascadingBuilder.OpenComponent(0, dialog.Type);
                            cascadingBuilder.AddMultipleAttributes(10, dialog.Parameters);
                            cascadingBuilder.CloseComponent();
                        }));

                        innerBuilder.CloseComponent();
                    }
                }));

                builder.CloseComponent();
            }

            builder.CloseElement();
        }
    }

    protected override void OnInitialized() {
        base.OnInitialized();
        _service.OnOpen += OnOpen;
        _service.OnClose += OnClose;
    }

    private Task OnClose(ITnTDialog dialog) {
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