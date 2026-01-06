using Microsoft.AspNetCore.Components;
using SkiaSharp;
using System;

namespace NTComponents.Charts.Core.Axes;

/// <summary>
///     Base class for all chart axes.
/// </summary>
public abstract class NTAxis<TData> : ComponentBase, IDisposable where TData : class
{

   [CascadingParameter]
   protected NTChart<TData> Chart { get; set; } = default!;

   /// <summary>
   ///     Gets or sets the direction of the axis.
   /// </summary>
   [Parameter]
   public AxisDirection Direction { get; set; }

   /// <summary>
   ///     Gets or sets the title of the axis.
   /// </summary>
   [Parameter]
   public string? Title { get; set; }

   /// <summary>
   ///     Gets or sets whether the axis is visible.
   /// </summary>
   [Parameter]
   public bool Visible { get; set; } = true;

   protected override void OnInitialized()
   {
      base.OnInitialized();
      if (Chart is null)
      {
         throw new ArgumentNullException(nameof(Chart), $"Axis must be used within a {nameof(NTChart<TData>)}.");
      }
      Chart.AddAxis(this);
   }

   /// <summary>
   ///     Renders the axis on the canvas.
   /// </summary>
   /// <param name="canvas">The canvas to render on.</param>
   /// <param name="renderArea">The total area of the chart.</param>
   /// <returns>The remaining area for the chart content after rendering the axis.</returns>
   public abstract SKRect Render(SKCanvas canvas, SKRect renderArea);

   public void Dispose()
   {
      Chart?.RemoveAxis(this);
   }
}
