using TnTComponents.Enum;

namespace TnTComponents.Forms;

internal interface ITnTForm {
    FormType FormType { get; set; }
    string Theme { get; set; }
    string BaseCssClass { get; set; }
    Dictionary<string, object> AdditionalAttributes { get; }
}

internal static class ITnTFormExt {

    public static void InitializeAdditionalAttributes(this ITnTForm form) {
        if (!form.AdditionalAttributes.TryGetValue("class", out var result)) {
            result = string.Empty;
        }
        form.AdditionalAttributes["class"] = $"{form.BaseCssClass} {string.Join(' ', result)}";

        if (!form.AdditionalAttributes.ContainsKey("theme")) {
            form.AdditionalAttributes["theme"] = form.Theme;
        }
    }
}