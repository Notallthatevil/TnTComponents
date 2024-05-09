using System.ComponentModel.DataAnnotations;

namespace TnTComponents.Attributes;

public class CheckboxRequiredAttribute : RangeAttribute {

    public CheckboxRequiredAttribute(string errorMessage = "Required") : base(typeof(bool), minimum: "true", maximum: "true") {
        ErrorMessage = errorMessage;
    }
}