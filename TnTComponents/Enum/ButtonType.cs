﻿namespace TnTComponents;

public enum ButtonType {
    Button,
    Submit,
    Reset
}

public static class ButtonTypeExt {

    public static string ToHtmlAttribute(this ButtonType buttonType) {
        return buttonType switch {
            ButtonType.Button => "button",
            ButtonType.Submit => "submit",
            ButtonType.Reset => "reset",
            _ => throw new InvalidOperationException($"{buttonType} is not a valid for the enum {nameof(ButtonType)}")
        };
    }
}