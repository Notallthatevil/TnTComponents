using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using TnTComponents.Enum;

namespace TnTComponents.Forms;
public abstract class TnTForm : EditForm {
    [Parameter]
    public FormType FormType { get; set; }

    [Parameter]
    public string Theme { get; set; } = "default";

    [Parameter]
    public virtual string BaseCssClass { get; set; } = "tnt-form";

    private readonly Dictionary<string, object> _additionalAttributes;

    protected TnTForm() {
        _additionalAttributes = AdditionalAttributes is not null ? new Dictionary<string, object>(AdditionalAttributes) : new Dictionary<string, object>();

        if (!_additionalAttributes.TryGetValue("class", out var result)) {
            result = string.Empty;
        }
        _additionalAttributes["class"] = $"{BaseCssClass} {string.Join(' ', result)}";

        if (!_additionalAttributes.ContainsKey("theme")) {
            _additionalAttributes["theme"] = Theme;
        }

        AdditionalAttributes = _additionalAttributes;
    }



    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        builder.OpenRegion(0);

        builder.OpenComponent<CascadingValue<TnTForm>>(1);
        builder.AddAttribute(2, "Value", this);
        builder.AddAttribute(3, "IsFixed", true);
        builder.AddAttribute(4, "ChildContent", new RenderFragment(base.BuildRenderTree));

        builder.CloseComponent();
        builder.CloseRegion();
    }
}

public class TnTForm<TModelType> : TnTForm {

}
