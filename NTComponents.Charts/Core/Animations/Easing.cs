namespace NTComponents.Charts.Core.Animations;

/// <summary>
/// Provides common easing functions for animations.
/// </summary>
public static class Easing {
   public static float Linear(float t) => t;
   public static float InQuad(float t) => t * t;
   public static float OutQuad(float t) => t * (2 - t);
   public static float InOutQuad(float t) => t < 0.5f ? 2 * t * t : -1 + (4 - 2 * t) * t;
   public static float InCubic(float t) => t * t * t;
   public static float OutCubic(float t) => (--t) * t * t + 1;
   public static float InOutCubic(float t) => t < 0.5f ? 4 * t * t * t : (t - 1) * (2 * t - 2) * (2 * t - 2) + 1;
}
