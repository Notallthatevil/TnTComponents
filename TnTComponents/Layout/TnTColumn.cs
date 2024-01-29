using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TnTComponents.Core;

namespace TnTComponents;
public class TnTColumn : ComponentBase, ITnTComponentBase {
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }
    public string? Class => CssBuilder.Create()
        .AddClass("tnt-col")
        .AddClass(GetGridClass())
        .Build();

    public ElementReference Element { get; }
    [Parameter]
    public string? Id { get; set; }
    [Parameter]
    public string? Style { get; set; }
    [Parameter]
    public bool? AutoFocus { get; set; }
    [Parameter]
    public bool Disabled { get; set; }

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;


    [Parameter, ColSize(SizeClass = "l", PropertyName = nameof(ColSize.Size))]
    public int L { get; set; }

    [Parameter, ColSize(SizeClass = "l", PropertyName = nameof(ColSize.Offset))]
    public int LOffset { get; set; }

    [Parameter, ColSize(SizeClass = "l", PropertyName = nameof(ColSize.Pull))]
    public int LPull { get; set; }

    [Parameter, ColSize(SizeClass = "l", PropertyName = nameof(ColSize.Push))]
    public int LPush { get; set; }

    [Parameter, ColSize(SizeClass = "m", PropertyName = nameof(ColSize.Size))]
    public int M { get; set; }

    [Parameter, ColSize(SizeClass = "m", PropertyName = nameof(ColSize.Offset))]
    public int MOffset { get; set; }

    [Parameter, ColSize(SizeClass = "m", PropertyName = nameof(ColSize.Pull))]
    public int MPull { get; set; }

    [Parameter, ColSize(SizeClass = "m", PropertyName = nameof(ColSize.Push))]
    public int MPush { get; set; }

    [Parameter, ColSize(SizeClass = "s", PropertyName = nameof(ColSize.Size))]
    public int S { get; set; }

    [Parameter, ColSize(SizeClass = "s", PropertyName = nameof(ColSize.Offset))]
    public int SOffset { get; set; }

    [Parameter, ColSize(SizeClass = "s", PropertyName = nameof(ColSize.Pull))]
    public int SPull { get; set; }

    [Parameter, ColSize(SizeClass = "s", PropertyName = nameof(ColSize.Push))]
    public int SPush { get; set; }

    [Parameter, ColSize(SizeClass = "xl", PropertyName = nameof(ColSize.Size))]
    public int XL { get; set; }

    [Parameter, ColSize(SizeClass = "xl", PropertyName = nameof(ColSize.Offset))]
    public int XLOffset { get; set; }

    [Parameter, ColSize(SizeClass = "xl", PropertyName = nameof(ColSize.Pull))]
    public int XLPull { get; set; }

    [Parameter, ColSize(SizeClass = "xl", PropertyName = nameof(ColSize.Push))]
    public int XLPush { get; set; }
    private static readonly IReadOnlyDictionary<PropertyInfo, ColSizeAttribute> _sizeValues = GetSizeProperties();

    private Dictionary<string, ColSize> _sizes = new Dictionary<string, ColSize>();

    protected override void OnInitialized() {
        base.OnInitialized();
        foreach (var sizeValuePair in _sizeValues) {
            var prop = sizeValuePair.Key;
            var colSizeAttr = sizeValuePair.Value;
            var value = (int)(prop.GetValue(this) ?? 0);

            if (value < 0 || value > 12) {
                throw new ArgumentOutOfRangeException(prop.Name, "Value must be within 0 and 12 inclusively.");
            }
            if (value > 0) {
                if (!_sizes.TryGetValue(colSizeAttr.SizeClass, out var colSize)) {
                    colSize = new ColSize();
                    _sizes.Add(colSizeAttr.SizeClass, colSize);
                }
                typeof(ColSize).GetProperty(colSizeAttr.PropertyName)!.SetValue(colSize, value);
            }
        }
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        builder.OpenElement(0, "div");
        builder.AddMultipleAttributes(10, AdditionalAttributes);
        builder.AddAttribute(20, "class", Class);
        builder.AddAttribute(30, "style", Style);
        builder.AddContent(40, ChildContent);
        builder.CloseElement();
    }

    private string GetGridClass() {
        var strBuilder = new StringBuilder();

        if (_sizes.Count == 0) {
            strBuilder.Append(' ').Append("s12");
        }
        else {
            foreach (var sizePair in _sizes) {
                var sizeClass = sizePair.Key;
                var value = sizePair.Value;

                if (value.Size > 0) {
                    strBuilder.Append(' ').Append($"{sizeClass}{value.Size}");

                    if (value.Offset > 0) {
                        strBuilder.Append(' ').Append($"{sizeClass}{value.Offset}-offset");
                    }

                    if (value.Push > 0) {
                        strBuilder.Append(' ').Append($"{sizeClass}{value.Push}-push");
                    }

                    if (value.Pull > 0) {
                        strBuilder.Append(' ').Append($"{sizeClass}{value.Pull}-pull");
                    }
                }
            }
        }

        return strBuilder.ToString();
    }

    private static IReadOnlyDictionary<PropertyInfo, ColSizeAttribute> GetSizeProperties() {
        var sizeValues = new Dictionary<PropertyInfo, ColSizeAttribute>();
        foreach (var prop in typeof(TnTColumn).GetProperties()) {
            var colSize = prop.GetCustomAttribute<ColSizeAttribute>();
            if (colSize is not null) {
                sizeValues.Add(prop, colSize);
            }
        }
        return sizeValues;
    }

    private class ColSize {
        public int Offset { get; set; }
        public int Pull { get; set; }
        public int Push { get; set; }
        public int Size { get; set; }
    }

    [AttributeUsage(AttributeTargets.Property)]
    private class ColSizeAttribute : Attribute {
        public string PropertyName { get; set; } = default!;
        public string SizeClass { get; set; } = default!;
    }
}
