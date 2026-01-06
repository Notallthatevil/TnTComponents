namespace NTComponents.Charts.Core.Animations;

/// <summary>
/// Represents a single animation.
/// </summary>
public class ChartAnimation {
   private readonly long _duration;
   private readonly Func<float, float> _easing;
   private long _elapsed;

   public ChartAnimation(long durationMilliseconds, Func<float, float>? easing = null) {
      _duration = durationMilliseconds;
      _easing = easing ?? Easing.Linear;
   }

   /// <summary>
   /// Gets the current progress of the animation (0 to 1).
   /// </summary>
   public float Progress { get; private set; }

   /// <summary>
   /// Gets a value indicating whether the animation is completed.
   /// </summary>
   public bool IsCompleted => _elapsed >= _duration;

   /// <summary>
   /// Updates the animation progress based on elapsed time.
   /// </summary>
   /// <param name="elapsedMilliseconds">The time elapsed since the last update.</param>
   public void Update(long elapsedMilliseconds) {
      if (IsCompleted) return;

      _elapsed += elapsedMilliseconds;
      var t = Math.Clamp((float)_elapsed / _duration, 0, 1);
      Progress = _easing(t);
   }

   /// <summary>
   /// Resets the animation to the beginning.
   /// </summary>
   public void Reset() {
      _elapsed = 0;
      Progress = 0;
   }
}
