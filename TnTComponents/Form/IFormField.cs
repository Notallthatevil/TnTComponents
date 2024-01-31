namespace TnTComponents.Form;

public interface IFormField : IFormItem {
    TnTIcon? EndIcon { get; }
    string? Placeholder { get; }
    TnTIcon? StartIcon { get; }
    InputType Type { get; }
}