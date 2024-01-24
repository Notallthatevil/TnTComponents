using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TnTComponents;
public enum InputType {
    Button = 1,
    Checkbox,
    Color,
    Date,
    DateTime,
    DateTimeLocal = DateTime,
    Email,
    File,
    Hidden,
    Image,
    Month,
    Number,
    Password,
    Radio,
    Range,
    Search,
    Tel,
    Text,
    Time,
    Url,
    Week
}

public static class InputTypeExt {
    public static string ToInputTypeString(this InputType inputType) {
        return inputType switch {
            InputType.Button => "button",
            InputType.Checkbox => "checkbox",
            InputType.Color => "color",
            InputType.Date => "date",
            InputType.DateTime => "datetime-local",
            InputType.Email => "email",
            InputType.File => "file",
            InputType.Hidden => "hidden",
            InputType.Image => "image",
            InputType.Month => "month",
            InputType.Number => "number",
            InputType.Password => "password",
            InputType.Radio => "radio",
            InputType.Range => "range",
            InputType.Search => "search",
            InputType.Tel => "tel",
            InputType.Text => "text",
            InputType.Time => "time",
            InputType.Url => "url",
            InputType.Week => "week",
            _ => throw new InvalidOperationException($"{inputType} is not a valid value of {nameof(InputType)}")
        };
    }
}