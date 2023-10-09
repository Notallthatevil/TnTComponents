using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using TnTComponents.Enum;

namespace TnTComponents.Forms;
public interface ITnTFormField {

    public const string DefaultBaseCssClass = "tnt-form-field";
    internal const string InputFieldLabelCssClass = "tnt-form-field-label";
    internal const string InputFieldMessageCssClass = "tnt-form-field-message";

    string BaseCssClass { get; set; }
    bool Disabled { get; set; }
    FormType FormType { get; set; }
    string Label { get; set; }
    TnTForm? ParentForm { get; set; }
    string? Placeholder { get; set; }
    string Theme { get; set; }
}

internal static class ITnTFormFieldExt {
    public static void MatchParentFormIfExists(this ITnTFormField formField) {
        if (formField.ParentForm is not null) {
            formField.FormType = formField.ParentForm.FormType;
            formField.Theme = formField.ParentForm.Theme;
        }
    }

}

