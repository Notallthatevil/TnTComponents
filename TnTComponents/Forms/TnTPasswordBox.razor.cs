
using Microsoft.AspNetCore.Components;

namespace TnTComponents.Forms;
public partial class TnTPasswordBox {
    [Parameter]
    public bool AllowPasswordReveal { get; set; } = true;

    private bool PasswordRevealed { get; set; } = false;
}

