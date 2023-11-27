using TnTComponents.Enum;

namespace TnTComponents.Forms;

internal interface ITnTForm {
    Dictionary<string, object> AdditionalAttributes { get; }
    string BaseCssClass { get; set; }
    FormType FormType { get; set; }
    string Theme { get; set; }
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