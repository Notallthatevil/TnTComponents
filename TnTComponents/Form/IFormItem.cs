namespace TnTComponents.Form;

public interface IFormItem {
    IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }
    FormAppearance Appearance { get; set; }
    string Class { get; }
    bool Disabled { get; set; }
    TnTForm? ParentForm { get; set; }
    FormAppearance? ParentFormAppearance { get; }
    bool? ParentFormDisabled { get; }
    bool? ParentFormReadOnly { get; }
    bool ReadOnly { get; set; }
    string? Style { get; set; }
}