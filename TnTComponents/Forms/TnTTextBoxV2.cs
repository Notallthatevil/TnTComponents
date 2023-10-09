using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using System.Text;
using TnTComponents.Common;
using TnTComponents.Enum;

namespace TnTComponents.Forms;
public partial class TnTTextBoxV2 : InputText, ITnTFormField {
    [Parameter]
    public string BaseCssClass { get; set; } = ITnTFormField.DefaultBaseCssClass;

    [Parameter]
    public bool Disabled { get; set; }

    [Parameter]
    public FormType FormType { get; set; }

    [Parameter, EditorRequired]
    public required string Label { get; set; }

    [CascadingParameter]
    public TnTForm? ParentForm { get; set; }

    [Parameter]
    public string? Placeholder { get; set; }

    [Parameter]
    public string Theme { get; set; } = TnTConstants.DefaultTheme;

    private bool _active;

    public TnTTextBoxV2() {
        this.MatchParentFormIfExists();
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        builder.OpenElement(0, "div");
        builder.AddAttribute(1, "class", GetCssClass());
        builder.AddAttribute(2, "theme", Theme);

        builder.OpenElement(3, "input");
        builder.AddMultipleAttributes(4, AdditionalAttributes);
        if (!string.IsNullOrWhiteSpace(NameAttributeValue)) {
            builder.AddAttribute(5, "name", NameAttributeValue);
        }
        if (!string.IsNullOrWhiteSpace(CssClass)) {
            builder.AddAttribute(6, "class", CssClass);
        }
        builder.AddAttribute(7, "value", CurrentValueAsString);
        builder.AddAttribute(8, "onchange", EventCallback.Factory.CreateBinder<string?>(this, v => CurrentValueAsString = v, CurrentValueAsString));
        builder.SetUpdatesAttributeName("value");
        builder.AddElementReferenceCapture(9, r => Element = r);
        if (!string.IsNullOrWhiteSpace(Placeholder)) {
            builder.AddAttribute(10, "placeholder", Placeholder);
        }
        builder.AddAttribute(11, "aria-label", Label);


        builder.CloseComponent();
    }


    private string GetCssClass() {
        var strBuilder = new StringBuilder(BaseCssClass);

        if (_active) {
            strBuilder.Append(' ').Append("active");
        }

        if (AdditionalAttributes?.TryGetValue("class", out var @class) ?? false) {
            strBuilder.Append(' ').Append(@class);
        }

        strBuilder.Append(' ').Append(CssClass);

        return strBuilder.ToString();
    }
}
