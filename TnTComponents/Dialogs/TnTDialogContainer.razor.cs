using Microsoft.AspNetCore.Components;
using TnTComponents.Common;

namespace TnTComponents.Dialogs;

public partial class TnTDialogContainer {
    private RenderFragment _content = default!;

    [Parameter]
    public override string BaseCssClass { get; set; } = "tnt-dialog-container";

    [Parameter, EditorRequired]
    public Dialog Dialog { get; set; } = default!;

    protected override void OnInitialized() {
        _content = new RenderFragment(builder => {
            builder.OpenComponent<TnTCard>(0);
            builder.AddComponentParameter(1, nameof(TnTCard.Type), Dialog.Options.CardType);

            builder.AddComponentParameter(2, nameof(TnTCard.ChildContent), new RenderFragment(cardBuilder => {
                if (Dialog.Options.ShowClose) {
                    cardBuilder.OpenElement(3, "span");
                    cardBuilder.AddAttribute(4, "onclick", EventCallback.Factory.Create(this, Close));
                    cardBuilder.AddContent(5, TnTIconComponent.RenderIcon(Dialog.Options.IconType, Dialog.Options.CloseIcon));
                    cardBuilder.CloseElement();
                }

                cardBuilder.OpenComponent(6, Dialog.Type);
                if (Dialog.Parameters is not null && Dialog.Parameters.Any()) {
                    cardBuilder.AddMultipleAttributes(7, Dialog.Parameters);
                }
                cardBuilder.CloseComponent();
            }));

            builder.CloseComponent();
        });
    }

    private void Close() {
        DialogService.CloseAsync(Dialog);
    }
}