using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TnTComponents.Common;
using TnTComponents.Services;

namespace TnTComponents.Dialogs;
public partial class TnTDialogContainer {
    [Parameter, EditorRequired]
    public Dialog Dialog { get; set; } = default!;

    [Parameter]
    public override string BaseCssClass { get; set; } = "tnt-dialog-container";

    private RenderFragment _content = default!;

    protected override void OnInitialized() {
        _content = new RenderFragment(builder => {
            builder.OpenComponent<TnTCard>(0);
            builder.AddComponentParameter(1, nameof(TnTCard.CardType), Dialog.Options.CardType);

            builder.AddComponentParameter(2, nameof(TnTCard.ChildContent), new RenderFragment(cardBuilder => {
                if (Dialog.Options.ShowClose) {
                    cardBuilder.OpenElement(3, "span");
                    cardBuilder.AddAttribute(4, "onclick", EventCallback.Factory.Create(this, Close));
                    cardBuilder.AddContent(5, TnTIconComponent.GetIcon(Dialog.Options.IconType, Dialog.Options.CloseIcon));
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

