//using Microsoft.AspNetCore.Components;
//using System.Reflection.Metadata;
//using System.Text;
//using TnTComponents.Enum;

//namespace TnTComponents.Forms;

//public partial class TnTRadioButton<TInputType> {
//    private string _id = default!;

//    private bool _trueValueToggle;

//    [Parameter]
//    public string? Icon { get; set; }

//    [Parameter]
//    public IconType IconType { get; set; }

//    [Parameter, EditorRequired]
//    public string Label { get; set; } = default!;

//    [Parameter]
//    public TInputType? Value { get; set; }

//    [CascadingParameter]
//    private RadioGroupContext Context { get; set; } = default!;

//    public override string GetCssClass() {
//        var strBuilder = new StringBuilder(base.GetCssClass());
//        strBuilder.Append(" tnt-radio-btn");

//        if (Disabled) {
//            strBuilder.Append(" disabled");
//        }

//        return strBuilder.ToString();
//    }

//    protected override void OnParametersSet() {
//        _id = Guid.NewGuid().ToString();
//        if (Context is null) {
//            throw new InvalidOperationException($"{GetType().Name} must have a parent of {typeof(TnTRadioGroup<TInputType>)}");
//        }

//        if (string.IsNullOrWhiteSpace(Label)) {
//            throw new ArgumentNullException($"Must provide a valid label for each {GetType().Name}");
//        }

//        base.OnParametersSet();
//    }

//    private char GetToggledTrueValue() {
//        _trueValueToggle = !_trueValueToggle;
//        return _trueValueToggle ? 'a' : 'b';
//    }
//}