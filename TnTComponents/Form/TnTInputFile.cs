using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TnTComponents.Core;
using TnTComponents.Form;

namespace TnTComponents;
public class TnTInputFile : InputFile {

    public string FormCssClass => CssClassBuilder.Create()
        .AddClass("tnt-input")
        .AddOutlined((ParentFormAppearance ?? Appearance) == FormAppearance.Outlined)
        .AddFilled((ParentFormAppearance ?? Appearance) == FormAppearance.Filled)
        .Build();

    [Parameter]
    public TnTColor ButtonBackgroundColor { get; set; } = TnTColor.Primary;

    [Parameter]
    public TnTColor ButtonTextColor { get; set; } = TnTColor.OnPrimary;

    [CascadingParameter]
    public TnTForm? ParentForm { get; set; }
    [CascadingParameter(Name = nameof(IFormItem.ParentFormAppearance))]
    public FormAppearance? ParentFormAppearance { get; set; }
    [CascadingParameter(Name = nameof(IFormItem.ParentFormDisabled))]
    public bool? ParentFormDisabled { get; set; }
    [CascadingParameter(Name = nameof(IFormItem.ParentFormReadOnly))]
    public bool? ParentFormReadOnly { get; set; }

    [Parameter]
    public FormAppearance Appearance { get; set; }
    [Parameter]
    public bool Disabled { get; set; }
    [Parameter]
    public bool Readonly { get; set; }


    protected override void OnParametersSet() {
        base.OnParametersSet();
        var dict = AdditionalAttributes != null ? new Dictionary<string, object>(AdditionalAttributes) : [];

        if (ParentFormDisabled == true || Disabled) {
            dict.TryAdd("disabled", true);
        }

        if (ParentFormReadOnly == true || Readonly) {
            dict.TryAdd("readonly", true);
        }

        AdditionalAttributes = dict;
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        builder.OpenElement(0, "span");
        builder.AddAttribute(10, "class", FormCssClass);
        builder.AddAttribute(11, "style", $"--button-background-color: var(--tnt-color-{ButtonBackgroundColor.ToCssClassName()}); --button-text-color: var(--tnt-color-{ButtonTextColor.ToCssClassName()})");

        {
            builder.OpenRegion(20);
            base.BuildRenderTree(builder);
            builder.CloseRegion();
        }

        builder.CloseElement();
    }
}

