using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using System.Text;
using TnTComponents.Enum;

namespace TnTComponents.Forms;

public partial class TnTInputFile : InputFile, ITnTFormField {

    [Parameter]
    public string BaseCssClass { get; set; } = "tnt-form-field-input";

    [Parameter]
    public FormType FormType { get; set; }

    [CascadingParameter]
    public TnTForm? ParentForm { get; set; }

    [Parameter]
    public string? Placeholder { get; set; }

    [Parameter]
    public string Theme { get; set; } = "default";

    [Parameter]
    public bool Disabled { get; set; }

    [Parameter]
    public ButtonType ButtonType { get; set; }

    [Parameter, EditorRequired]
    public string Label { get; set; } = default!;

    protected override void OnParametersSet() {
        this.MatchParentFormIfExists();
        if (AdditionalAttributes == null) {
            AdditionalAttributes = new Dictionary<string, object>() {
                { "name", Label },
                { "theme", Theme },
                { "class",  GetButtonClass() }
            };
        }
        else {
            var dict = new Dictionary<string, object>(AdditionalAttributes);
            if (!dict.ContainsKey("name")) {
                dict.Add("name", Label);
            }

            dict.TryGetValue("class", out var result);
            dict["class"] = string.Join(' ', GetButtonClass(), result);

            if (!dict.ContainsKey("theme")) {
                dict.Add("theme", Theme);
            }

            AdditionalAttributes = dict;
        }

        base.OnParametersSet();
    }

    public string GetCssClass() {
        var strBuilder = new StringBuilder(BaseCssClass);

        if (Disabled) {
            strBuilder.Append(' ').Append("disabled");
        }

        return strBuilder.ToString();
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        builder.OpenElement(0, "div");
        builder.AddAttribute(10, "class", GetCssClass());
        builder.AddAttribute(20, "theme", Theme);

        builder.OpenElement(30, "label");
        builder.AddAttribute(40, "class", "tnt-input-field-label");

        builder.OpenRegion(50);
        base.BuildRenderTree(builder);
        builder.CloseRegion();

        builder.AddContent(60, Label);
        builder.CloseElement();

        builder.CloseElement();
    }

    private string GetButtonClass() {
        var strBuilder = new StringBuilder();

        switch (ButtonType) {
            case ButtonType.Default:
            default:
                break;

            case ButtonType.Flat:
                strBuilder.Append(' ').Append("flat");
                break;

            case ButtonType.Filled:
                strBuilder.Append(' ').Append("filled");
                break;

            case ButtonType.Outlined:
                strBuilder.Append(' ').Append("outlined");
                break;
        }

        if (Disabled) {
            strBuilder.Append(' ').Append("disabled");
        }

        return strBuilder.ToString();
    }
}