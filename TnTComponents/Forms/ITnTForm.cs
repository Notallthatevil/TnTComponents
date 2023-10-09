using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using TnTComponents.Enum;

namespace TnTComponents.Forms;
public interface ITnTForm {
    public RenderFragment ChildContent { get; set; }
    public FormType FormType { get; set; }
    public EditContext EditContext { get; set; }
    public string Theme { get; set; }
}
