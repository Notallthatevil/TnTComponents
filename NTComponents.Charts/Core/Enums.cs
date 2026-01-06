namespace NTComponents.Charts.Core;

/// <summary>
/// Specifies the position of an element (like a legend) relative to the chart.
/// </summary>
public enum ChartPosition {
   Top,
   Bottom,
   Left,
   Right
}

/// <summary>
/// Specifies the orientation of an axis or other elements.
/// </summary>
public enum ChartOrientation {
   Horizontal,
   Vertical
}

/// <summary>
/// Specifies the scale type for an axis.
/// </summary>
public enum AxisScaleType {
   Linear,
   Logarithmic,
   Category,
   Time
}
