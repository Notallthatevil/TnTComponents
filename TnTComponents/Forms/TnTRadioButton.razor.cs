using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Reflection.Metadata;
using System.Text;
using TnTComponents.Enum;

namespace TnTComponents.Forms;
public partial class TnTRadioButton<TInputType> {


    [CascadingParameter]
    private RadioGroupContext Context { get; set; } = default!;

    [Parameter, EditorRequired]
    public string Label { get; set; } = default!;

    [Parameter]
    public string? Icon { get; set; }

    [Parameter]
    public IconType IconType { get; set; }

    [Parameter]
    public bool Disabled { get; set; }

    [Parameter]
    public TInputType? Value { get; set; }

    private string _id;
    private bool _trueValueToggle;

    protected override void OnParametersSet() {
        _id = Guid.NewGuid().ToString();
        if (Context is null) {
            throw new InvalidOperationException($"{GetType().Name} must have a parent of {typeof(TnTRadioGroup<TInputType>)}");
        }

        if (string.IsNullOrWhiteSpace(Label)) {
            throw new ArgumentNullException($"Must provide a valid label for each {GetType().Name}");
        }

        base.OnParametersSet();
    }

    protected override string GetCssClass() {
        var strBuilder = new StringBuilder(base.GetCssClass());
        strBuilder.Append(" tnt-radio-btn");

        if (Disabled) {
            strBuilder.Append(" disabled");
        }

        return strBuilder.ToString();
    }

    private char GetToggledTrueValue() {
        _trueValueToggle = !_trueValueToggle;
        return _trueValueToggle ? 'a' : 'b';
    }

}
