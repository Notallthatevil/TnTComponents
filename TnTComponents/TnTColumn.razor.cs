using Microsoft.AspNetCore.Components;
using System.Reflection;
using System.Text;
using TnTComponents.Core;
using TnTComponents.Enum;

namespace TnTComponents;

public partial class TnTColumn : ITnTComponentBase {

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    [Parameter]
    public string? Class { get; set; } = "tnt-col";

    [Parameter]
    public object? Data { get; set; }

    public ElementReference Element { get; protected set; }

    [Parameter]
    public AlignContent FlexAlignContent { get; set; }

    [Parameter]
    public AlignItems FlexAlignItems { get; set; }

    [Parameter]
    public JustifyContent FlexJustifyContent { get; set; }

    [Parameter]
    public WrapStyle FlexWrap { get; set; }

    [Parameter]
    public string? Id { get; set; }

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

    [Parameter]
    public string? Style { get; set; }

    [Parameter]
    public string? Theme { get; set; }

    [Parameter, ColSize(SizeClass = "xl", PropertyName = nameof(ColSize.Size))]
    public int XL { get; set; }

    [Parameter, ColSize(SizeClass = "xl", PropertyName = nameof(ColSize.Offset))]
    public int XLOffset { get; set; }

    [Parameter, ColSize(SizeClass = "xl", PropertyName = nameof(ColSize.Pull))]
    public int XLPull { get; set; }

    [Parameter, ColSize(SizeClass = "xl", PropertyName = nameof(ColSize.Push))]
    public int XLPush { get; set; }
    public bool? AutoFocus { get; set; }
    public bool? Disabled { get; set; }

    private static readonly IReadOnlyDictionary<PropertyInfo, ColSizeAttribute> _sizeValues = GetSizeProperties();

    private Dictionary<string, ColSize> _sizes = new Dictionary<string, ColSize>();

    public string GetClass() {
        var strBuilder = new StringBuilder(this.GetClassDefault());

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

    protected string? GetStyle() {
        var strBuilder = new StringBuilder(FlexWrap.ToStyle())
            .Append(' ').Append(FlexAlignContent.ToStyle())
            .Append(' ').Append(FlexJustifyContent.ToStyle())
            .Append(' ').Append(FlexAlignItems.ToStyle());

        if (AdditionalAttributes?.TryGetValue("style", out var style) ?? false) {
            strBuilder.AppendJoin(' ', style);
        }
        var result = strBuilder.ToString();
        return !string.IsNullOrWhiteSpace(result) ? result : null;
    }

    protected override void OnInitialized() {
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

        base.OnInitialized();
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