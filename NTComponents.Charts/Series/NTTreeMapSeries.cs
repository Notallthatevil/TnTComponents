using Microsoft.AspNetCore.Components;
using NTComponents.Charts.Core;
using NTComponents.Charts.Core.Series;
using SkiaSharp;

namespace NTComponents.Charts;

/// <summary>
///     Represents a treemap series that visualizes data as nested rectangles.
/// </summary>
/// <typeparam name="TData">The type of the data.</typeparam>
public class NTTreeMapSeries<TData> : NTBaseSeries<TData> where TData : class
{
   [Parameter]
   public Func<TData, double> ValueSelector { get; set; } = _ => 0;

   [Parameter]
   public Func<TData, string> LabelSelector { get; set; } = _ => string.Empty;

   [Parameter]
   public float ItemPadding { get; set; } = 2f;

   [Parameter]
   public string DataLabelFormat { get; set; } = "{0:N0}";

   [Parameter]
   public bool ShowLabels { get; set; } = true;

   public override ChartCoordinateSystem CoordinateSystem => ChartCoordinateSystem.None;

   private List<TreeMapRect> _lastRects = [];

   private record TreeMapRect(TData Data, SKRect Rect, SKColor Color, int Index);

   public override void Render(SKCanvas canvas, SKRect renderArea)
   {
      if (Data == null || !Data.Any()) return;

      var dataList = Data.ToList();
      var totalValue = dataList.Sum(ValueSelector);
      if (totalValue <= 0) return;

      var progress = GetAnimationProgress();
      var easedProgress = BackEase(progress);
      var visibilityFactor = VisibilityFactor;

      // Simple Squarified Treemap Layout (simplified version)
      _lastRects = LayoutTreeMap(dataList, renderArea);

      foreach (var item in _lastRects)
      {
         var rect = item.Rect;

         // Apply animation: scale from center of rect
         if (progress < 1)
         {
            float centerX = rect.MidX;
            float centerY = rect.MidY;
            float w = rect.Width * easedProgress;
            float h = rect.Height * easedProgress;
            rect = new SKRect(centerX - w / 2, centerY - h / 2, centerX + w / 2, centerY + h / 2);
         }

         var isHovered = Chart.HoveredSeries == this && Chart.HoveredPointIndex == item.Index;
         var hoverFactor = (isHovered) ? 1.0f : HoverFactor;
         var color = item.Color.WithAlpha((byte)(item.Color.Alpha * visibilityFactor * hoverFactor));

         using var paint = new SKPaint
         {
            Color = color,
            Style = SKPaintStyle.Fill,
            IsAntialias = true
         };

         canvas.DrawRect(rect, paint);

         if (ShowLabels && rect.Width > 40 && rect.Height > 20)
         {
            RenderLabel(canvas, rect, item.Data, color);
         }
      }
   }

   private void RenderLabel(SKCanvas canvas, SKRect rect, TData data, SKColor bgColor)
   {
      var label = LabelSelector(data);
      var value = ValueSelector(data);
      var valueText = string.Format(DataLabelFormat, value);

      // Determine text color based on background brightness
      var luminance = 0.2126f * bgColor.Red + 0.7152f * bgColor.Green + 0.0722f * bgColor.Blue;
      var textColor = luminance > 128 ? SKColors.Black : SKColors.White;

      using var paint = new SKPaint
      {
         Color = textColor,
         IsAntialias = true
      };

      using var font = new SKFont
      {
         Size = 12
      };

      // Clip text to rect
      canvas.Save();
      canvas.ClipRect(rect);

      float x = rect.Left + 5;
      float y = rect.Top + 15;

      canvas.DrawText(label, x, y, SKTextAlign.Left, font, paint);

      font.Size = 10;
      canvas.DrawText(valueText, x, y + 15, SKTextAlign.Left, font, paint);

      canvas.Restore();
   }

   private List<TreeMapRect> LayoutTreeMap(List<TData> data, SKRect area)
   {
      var result = new List<TreeMapRect>();
      var sortedData = data.Select((d, i) => new { Data = d, Value = ValueSelector(d), OriginalIndex = i })
                          .OrderByDescending(x => x.Value)
                          .ToList();

      var totalValue = sortedData.Sum(x => x.Value);

      // Use a simple Slice-and-Dice for now as it's more stable for animations
      // but we can refine it if needed.
      SliceAndDice(sortedData.Select(x => (object)x).ToList(), area, totalValue, result, true);

      return result;
   }

   private void SliceAndDice(List<object> items, SKRect area, double totalValue, List<TreeMapRect> result, bool horizontal)
   {
      if (!items.Any()) return;
      if (items.Count == 1)
      {
         dynamic item = items[0];
         var rect = area;
         if (rect.Width > ItemPadding * 2 && rect.Height > ItemPadding * 2)
         {
            rect.Inflate(-ItemPadding, -ItemPadding);
         }
         result.Add(new TreeMapRect(item.Data, rect, Chart.GetSeriesColor(this).WithAlpha((byte)(255 * (0.4 + 0.6 * (double)item.Value / (totalValue == 0 ? 1 : totalValue)))), item.OriginalIndex));
         return;
      }

      // Split items into two groups
      int mid = items.Count / 2;
      var leftItems = items.Take(mid).ToList();
      var rightItems = items.Skip(mid).ToList();

      double leftValue = leftItems.Sum(x => (double)((dynamic)x).Value);
      double rightValue = rightItems.Sum(x => (double)((dynamic)x).Value);
      double total = leftValue + rightValue;

      if (total <= 0) return;

      if (horizontal)
      {
         float leftWidth = (float)(area.Width * (leftValue / total));
         var leftArea = new SKRect(area.Left, area.Top, area.Left + leftWidth, area.Bottom);
         var rightArea = new SKRect(area.Left + leftWidth, area.Top, area.Right, area.Bottom);
         SliceAndDice(leftItems, leftArea, leftValue, result, !horizontal);
         SliceAndDice(rightItems, rightArea, rightValue, result, !horizontal);
      }
      else
      {
         float topHeight = (float)(area.Height * (leftValue / total));
         var topArea = new SKRect(area.Left, area.Top, area.Right, area.Top + topHeight);
         var bottomArea = new SKRect(area.Left, area.Top + topHeight, area.Right, area.Bottom);
         SliceAndDice(leftItems, topArea, leftValue, result, !horizontal);
         SliceAndDice(rightItems, bottomArea, rightValue, result, !horizontal);
      }
   }

   public override (int Index, TData? Data)? HitTest(SKPoint point, SKRect renderArea)
   {
      foreach (var item in _lastRects)
      {
         if (item.Rect.Contains(point))
         {
            return (item.Index, item.Data);
         }
      }
      return null;
   }

   internal override IEnumerable<LegendItemInfo<TData>> GetLegendItems()
   {
      if (Data == null) yield break;

      var dataList = Data.ToList();
      for (int i = 0; i < dataList.Count; i++)
      {
         var item = dataList[i];
         yield return new LegendItemInfo<TData>
         {
            Label = LabelSelector(item),
            Color = Chart.GetSeriesColor(this).WithAlpha((byte)(255 * (0.4 + 0.6 * ValueSelector(item) / dataList.Max(ValueSelector)))),
            Series = this,
            Index = i,
            IsVisible = Visible
         };
      }
   }
}
