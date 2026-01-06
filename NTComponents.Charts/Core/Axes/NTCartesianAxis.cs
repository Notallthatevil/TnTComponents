using NTComponents.Charts.Core;
using NTComponents.Charts.Core.Series;
using SkiaSharp;
using System.Linq;

namespace NTComponents.Charts.Core.Axes;

/// <summary>
///     An axis for cartesian charts.
/// </summary>
public class NTCartesianAxis<TData> : NTAxis<TData> where TData : class
{

   /// <inheritdoc />
   public override SKRect Render(SKCanvas canvas, SKRect renderArea)
   {
      if (string.IsNullOrEmpty(Title))
      {
         return renderArea;
      }

      using var paint = new SKPaint
      {
         Color = SKColors.Black,
         TextSize = 16,
         IsAntialias = true,
         TextAlign = SKTextAlign.Center
      };

      if (Direction == AxisDirection.X)
      {
         var x = renderArea.Left + (renderArea.Width / 2);
         var y = renderArea.Bottom - 5;
         canvas.DrawText(Title, x, y, paint);
         return new SKRect(renderArea.Left, renderArea.Top, renderArea.Right, renderArea.Bottom - 25);
      }
      else
      {
         var x = renderArea.Left + 15;
         var y = renderArea.Top + (renderArea.Height / 2);

         canvas.Save();
         canvas.RotateDegrees(-90, x, y);
         canvas.DrawText(Title, x, y, paint);
         canvas.Restore();

         return new SKRect(renderArea.Left + 25, renderArea.Top, renderArea.Right, renderArea.Bottom);
      }
   }
}
