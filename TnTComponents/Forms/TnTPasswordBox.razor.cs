
using Microsoft.AspNetCore.Components;

namespace TnTComponents.Forms;
public partial class TnTPasswordBox {
    protected override string InputType => PasswordRevealed ? "text" : "password";

    [Parameter]
    public bool AllowPasswordReveal { get; set; } = true;

    private bool PasswordRevealed { get; set; } = false;
}

