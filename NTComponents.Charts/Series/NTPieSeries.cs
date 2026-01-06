using Microsoft.AspNetCore.Components;
using NTComponents.Charts.Core.Series;
using SkiaSharp;

namespace NTComponents.Charts;

/// <summary>
///     Represents a pie/donut series in a circular chart.
/// </summary>
/// <typeparam name="TData">The type of the data.</typeparam>
public class NTPieSeries<TData> : NTCircularSeries<TData> where TData : class
{
    [Parameter]
    public float ExplosionDistance { get; set; } = 10f;

    public override void Render(SKCanvas canvas, SKRect renderArea)
    {
        CalculateSlices(renderArea);
        if (!SliceInfos.Any()) return;

        var progress = GetAnimationProgress();
        var myVisibilityFactor = VisibilityFactor;
        var totalSweep = progress * 360f;

        float radius = Math.Min(renderArea.Width, renderArea.Height) / 2f;
        float innerRadius = radius * InnerRadiusRatio;

        foreach (var slice in SliceInfos)
        {
            // Only draw slices that fall within the current animation progress
            if (slice.StartAngle + 90 > totalSweep) continue;

            float sweep = slice.SweepAngle;
            if (slice.StartAngle + 90 + sweep > totalSweep)
            {
                sweep = totalSweep - (slice.StartAngle + 90);
            }

            if (sweep <= 0) continue;

            var isPointHovered = Chart.HoveredSeries == this && Chart.HoveredPointIndex == slice.Index;
            var isAnyHovered = Chart.HoveredSeries != null;
            var isOtherSeriesHovered = isAnyHovered && Chart.HoveredSeries != this;
            var isOtherPointHovered = isAnyHovered && Chart.HoveredSeries == this && Chart.HoveredPointIndex != slice.Index;

            var baseColor = Chart.GetThemeColor(Chart.Palette[slice.Index % Chart.Palette.Count]);
            var color = baseColor;

            if (isOtherSeriesHovered || isOtherPointHovered)
            {
                color = color.WithAlpha((byte)(color.Alpha * 0.15f * myVisibilityFactor));
            }
            else
            {
                color = color.WithAlpha((byte)(color.Alpha * myVisibilityFactor));
            }

            using var paint = new SKPaint
            {
                Color = color,
                IsAntialias = true,
                Style = SKPaintStyle.Fill
            };

            float currentRadius = radius;
            float currentInnerRadius = innerRadius;
            float centerX = renderArea.MidX;
            float centerY = renderArea.MidY;

            if (isPointHovered)
            {
                // Explode the slice
                float midAngle = slice.StartAngle + slice.SweepAngle / 2f;
                float rad = midAngle * (float)Math.PI / 180f;
                centerX += (float)Math.Cos(rad) * ExplosionDistance;
                centerY += (float)Math.Sin(rad) * ExplosionDistance;
                paint.Color = baseColor.WithAlpha((byte)(255 * myVisibilityFactor));
            }

            using var path = new SKPath();
            var outerRect = new SKRect(centerX - currentRadius, centerY - currentRadius, centerX + currentRadius, centerY + currentRadius);
            var innerRect = new SKRect(centerX - currentInnerRadius, centerY - currentInnerRadius, centerX + currentInnerRadius, centerY + currentInnerRadius);

            path.ArcTo(outerRect, slice.StartAngle, sweep, true);
            if (currentInnerRadius > 0)
            {
                path.ArcTo(innerRect, slice.StartAngle + sweep, -sweep, false);
            }
            else
            {
                path.LineTo(centerX, centerY);
            }
            path.Close();

            canvas.DrawPath(path, paint);

            if (ShowDataLabels && progress >= 1.0f)
            {
                RenderSliceLabel(canvas, slice, centerX, centerY, currentRadius, currentInnerRadius, color);
            }
        }
    }

    private void RenderSliceLabel(SKCanvas canvas, PieSliceInfo slice, float centerX, float centerY, float radius, float innerRadius, SKColor color)
    {
        float midAngle = slice.StartAngle + slice.SweepAngle / 2f;
        float rad = midAngle * (float)Math.PI / 180f;
        
        float labelRadius = innerRadius + (radius - innerRadius) / 2f;
        if (innerRadius == 0) labelRadius = radius * 0.7f;

        float lx = centerX + (float)Math.Cos(rad) * labelRadius;
        float ly = centerY + (float)Math.Sin(rad) * labelRadius;

        var text = string.Format(DataLabelFormat, slice.Value);
        
        var labelColor = Chart.GetThemeColor(DataLabelColor ?? Chart.BackgroundColor);
        
        using var paint = new SKPaint
        {
            Color = labelColor,
            IsAntialias = true
        };
        using var font = new SKFont { Size = DataLabelSize };

        canvas.DrawText(text, lx, ly, SKTextAlign.Center, font, paint);
    }

    public override (int Index, TData? Data)? HitTest(SKPoint point, SKRect renderArea)
    {
        float centerX = renderArea.MidX;
        float centerY = renderArea.MidY;
        float radius = Math.Min(renderArea.Width, renderArea.Height) / 2f;
        float innerRadius = radius * InnerRadiusRatio;

        float dx = point.X - centerX;
        float dy = point.Y - centerY;
        float distSq = dx * dx + dy * dy;

        if (distSq > radius * radius || distSq < innerRadius * innerRadius) return null;

        float angle = (float)Math.Atan2(dy, dx) * 180f / (float)Math.PI;
        if (angle < -90) angle += 360; // Normalize to start from -90

        foreach (var slice in SliceInfos)
        {
            float start = slice.StartAngle;
            float end = slice.StartAngle + slice.SweepAngle;

            // Handle wrap around if necessary, but normalized angle should work
            if (angle >= start && angle < end)
            {
                return (slice.Index, slice.Data);
            }
        }

        return null;
    }
}
