using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using TnTComponents.Enum;
using Microsoft.JSInterop;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Xml.Linq;

namespace TnTComponents.Forms;
public partial class TnTForm<TModelType> {
    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    [Parameter]
    public FormType FormType { get; set; }

    [Parameter]
    public EditContext EditContext { get; set; } = default!;

    [Parameter]
    public override string BaseCssClass { get; set; } = "tnt-form";

    [Parameter]
    public EventCallback<TModelType> OnSubmit { get; set; }

    [Parameter]
    public EventCallback<IEnumerable<string>> OnInvalidSubmit { get; set; }

    [Parameter]
    public TModelType Model { get; set; } = default!;

    [Parameter]
    public string FormName { get; set; } = default!;

    [Parameter]
    public bool Enhance { get; set; } = default;


    public TnTForm() {
        _onSubmitCallback = Submit;
    }

}
