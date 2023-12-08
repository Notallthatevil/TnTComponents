//using Microsoft.AspNetCore.Components;
//using TnTComponents.Enum;

//namespace TnTComponents.Forms;
//public partial class TnTRadioButton<TInputType> {
//    [CascadingParameter]
//    private TnTRadioGroup<TInputType> _radioGroup { get; set; } = default!;

//    [Parameter]
//    public string LabelClass { get; set; } = TnTInputBase<TInputType>.DefaultLabelClass;

//    [Parameter]
//    public string ContainerClass { get; set; } = TnTInputBase<TInputType>.DefaultContainerClass;

//    [Parameter]
//    public IconType IconType { get; set; }

//    [Parameter]
//    public bool Disabled { get; set; }

//    [Parameter]
//    public string? AriaLabel { get; set; }

//    [Parameter]
//    public string? Label { get; set; }

//    [Parameter]
//    public string? Title { get; set; }

//    [Parameter]
//    public string? StartIcon { get; set; }

//    [Parameter]
//    public string? EndIcon { get; set; }
//    [Parameter]
//    public bool Required { get; set; }

//    [Parameter]
//    public string LabelTextClass { get; set; } = TnTInputBase<TInputType>.DefaultLabelTextClass;

//    [Parameter]
//    public string SupportingTextClass { get; set; } = TnTInputBase<TInputType>.DefaultSupportingTextClass;

//    [Parameter]
//    public string? SupportingText { get; set; }

//    protected override void OnParametersSet() {
//        base.OnParametersSet();

//        if (_radioGroup is null) {
//            throw new InvalidOperationException($"{nameof(TnTRadioButton<TInputType>)} must be a child of {nameof(TnTRadioGroup<TInputType>)}");
//        }
//    }
//}
