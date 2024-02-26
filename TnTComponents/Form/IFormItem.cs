﻿using Microsoft.AspNetCore.Components;

namespace TnTComponents.Form;

public interface IFormItem {
    IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }
    FormAppearance Appearance { get; set; }
    TnTColor? BackgroundColor { get; set; }
    string FormCssClass { get; }
    bool Disabled { get; set; }
    ElementReference Element { get; }
    TnTForm? ParentForm { get; set; }
    FormAppearance? ParentFormAppearance { get; }
    bool? ParentFormDisabled { get; }
    bool? ParentFormReadOnly { get; }
    bool ReadOnly { get; set; }
    string? FormCssStyle { get; }
    TnTColor? TextColor { get; set; }
}