﻿using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TnTComponents.Forms {
    public class TnTTextBox : TnTFormField<string> {
        protected override string InputType => "text";

        protected override async Task OnChange(ChangeEventArgs e) {
            await ValueChanged.InvokeAsync(e?.Value?.ToString());
        }
    }
}