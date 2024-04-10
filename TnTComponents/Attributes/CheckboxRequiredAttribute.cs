using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TnTComponents.Attributes;
public class CheckboxRequiredAttribute : RangeAttribute{

    public CheckboxRequiredAttribute(string errorMessage = "Required"): base(typeof(bool), minimum: "true", maximum: "true") {
        ErrorMessage = errorMessage;
    }

}

