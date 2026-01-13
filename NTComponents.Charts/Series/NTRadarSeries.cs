using Microsoft.AspNetCore.Components;
using NTComponents.Charts.Core.Series;
using SkiaSharp;

namespace NTComponents.Charts;

/// <summary>
///     Represents a radar series in a circular chart.
/// </summary>
/// <typeparam name="TData">The type of the data.</typeparam>
public class NTRadarSeries<TData> : NTCircularSeries<TData> where TData : class
{
   [Parameter]
   public float StrokeWidth { get; set; } = 2f;

   [Parameter]
   public float AreaOpacity { get; set; } = 0.2f;

   /// <summary>
   ///    Gets or sets the maximum value for the radar scale. If null, it will be calculated from the data.
   /// </summary>
   [Parameter]
   public double? MaxValue { get; set; }

   public override void Render(SKCanvas canvas, SKRect renderArea)
   {
      if (Data == null || !Data.Any()) return;

      var dataList = Data.ToList();
      int count = dataList.Count;
      if (count < 3) return;

      float centerX = renderArea.MidX;
      float centerY = renderArea.MidY;
      float radius = Math.Min(renderArea.Width, renderArea.Height) / 2f;

      double max = MaxValue ?? dataList.Max(ValueSelector);
      if (max <= 0) max = 1;

      var progress = GetAnimationProgress();
      var visibilityFactor = VisibilityFactor;
      var hoverFactor = HoverFactor;

      var color = Chart.GetSeriesColor(this);
      color = color.WithAlpha((byte)(color.Alpha * visibilityFactor * hoverFactor));

      var points = new SKPoint[count];
      for (int i = 0; i < count; i++)
      {
         float angle = (i * 360f / count) - 90f;
         float rad = angle * (float)Math.PI / 180f;

         double val = ValueSelector(dataList[i]) * progress;
         float r = (float)(val / max) * radius;

         points[i] = new SKPoint(
             centerX + (float)Math.Cos(rad) * r,
             centerY + (float)Math.Sin(rad) * r
         );
      }

      using var path = new SKPath();
      path.AddPoly(points, true);

      // Fill
      using (var fillPaint = new SKPaint
      {
         Style = SKPaintStyle.Fill,
         Color = color.WithAlpha((byte)(color.Alpha * AreaOpacity)),
         IsAntialias = true
      })
      {
         canvas.DrawPath(path, fillPaint);
      }

      // Stroke
      using (var strokePaint = new SKPaint
      {
         Style = SKPaintStyle.Stroke,
         Color = color,
         StrokeWidth = StrokeWidth,
         IsAntialias = true
      })
      {
         canvas.DrawPath(path, strokePaint);
      }

      // Points and Labels
      for (int i = 0; i < count; i++)
      {
         var item = dataList[i];
         var args = new NTDataPointRenderArgs<TData>
         {
            Data = item,
            Index = i,
            Color = color,
            GetThemeColor = Chart.GetThemeColor
         };
         OnDataPointRender?.Invoke(args);

         var pointColor = args.Color ?? color;
         var p = points[i];
         RenderPoint(canvas, p.X, p.Y, pointColor);
         
         if (ShowDataLabels)
         {
            var labelColor = args.DataLabelColor ?? pointColor;
            var labelSize = args.DataLabelSize ?? 12f;
            RenderDataLabel(canvas, p.X, p.Y - 10, ValueSelector(item), labelColor, labelSize);
         }
      }
   }

   private void RenderDataLabel(SKCanvas canvas, float x, float y, double value, SKColor color, float fontSize)
   {
      var text = string.Format("{0:N0}", value);
      using var paint = new SKPaint { Color = color, IsAntialias = true };
      using var font = new SKFont { Size = fontSize, Typeface = Chart.DefaultTypeface };
      canvas.DrawText(text, x, y, SKTextAlign.Center, font, paint);
   }

   private void RenderPoint(SKCanvas canvas, float x, float y, SKColor color)
   {
      using var paint = new SKPaint { Color = color, IsAntialias = true };
      canvas.DrawCircle(x, y, 4, paint);
   }

   public override (int Index, TData? Data)? HitTest(SKPoint point, SKRect renderArea)
   {
      // Radar hit testing is usually proximity based
      var dataList = Data.ToList();
      float centerX = renderArea.MidX;
      float centerY = renderArea.MidY;
      float radius = Math.Min(renderArea.Width, renderArea.Height) / 2f;
      double max = MaxValue ?? dataList.Max(ValueSelector);
      if (max <= 0) max = 1;

      for (int i = 0; i < dataList.Count; i++)
      {
         float angle = (i * 360f / dataList.Count) - 90f;
         float rad = angle * (float)Math.PI / 180f;
         double val = ValueSelector(dataList[i]);
         float r = (float)(val / max) * radius;

         var px = centerX + (float)Math.Cos(rad) * r;
         var py = centerY + (float)Math.Sin(rad) * r;

         float dx = point.X - px;
         float dy = point.Y - py;
         if (dx * dx + dy * dy < 100) return (i, dataList[i]);
      }
      return null;
   }
}
