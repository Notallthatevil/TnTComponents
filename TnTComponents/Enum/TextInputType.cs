namespace TnTComponents.Enum;

public enum TextInputType {
    Text,
    Email,
    Password,
    Tel,
    Url,
    Search
}

public static class TextInputTypeExt {

    public static InputType ToInputType(this TextInputType textInputType) {
        return textInputType switch {
            TextInputType.Text => InputType.Text,
            TextInputType.Email => InputType.Email,
            TextInputType.Password => InputType.Password,
            TextInputType.Tel => InputType.Tel,
            TextInputType.Url => InputType.Url,
            TextInputType.Search => InputType.Search,
            _ => throw new InvalidOperationException($"{textInputType} is not a valid value for {nameof(TextInputType)}")
        };
    }
}