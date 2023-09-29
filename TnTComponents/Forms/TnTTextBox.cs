using Microsoft.AspNetCore.Components;

namespace TnTComponents.Forms {
    public class TnTTextBox : TnTFormField<string> {
        protected override string InputType => "text";

        protected override async Task OnChange(ChangeEventArgs e) {
            await ValueChanged.InvokeAsync(e?.Value?.ToString());
        }
    }
}
