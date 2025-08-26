using System;
using System.Threading;
using System.Threading.Tasks;
using TnTComponents.Core;
using Xunit;

namespace TnTComponents.Tests.Core {
   public class TnTDebouncerTests {
      [Fact]
      public async Task Debounce_ExecutesOnlyLastAction_WhenSecondCallWithinDelay() {
         var debouncer = new TnTDebouncer(millisecondsDelay: 120);
         int callCount = 0;
         string? last = null;

         var first = debouncer.DebounceAsync(_ => { callCount++; last = "A"; return Task.CompletedTask; });
         await Task.Delay(20, Xunit.TestContext.Current.CancellationToken);
         var second = debouncer.DebounceAsync(_ => { callCount++; last = "B"; return Task.CompletedTask; });

         await Task.WhenAll(first, second);

         callCount.Should().Be(1);
         last.Should().Be("B");
      }

      [Fact]
      public async Task Debounce_ExecutesEachAction_WhenCallsSpacedBeyondDelay() {
         var debouncer = new TnTDebouncer(millisecondsDelay: 25);
         int callCount = 0;

         await debouncer.DebounceAsync(_ => { callCount++; return Task.CompletedTask; });
         await Task.Delay(40, Xunit.TestContext.Current.CancellationToken);
         await debouncer.DebounceAsync(_ => { callCount++; return Task.CompletedTask; });

         callCount.Should().Be(2);
      }

      [Fact]
      public async Task DebounceForResult_ReturnsResult_WhenNotCanceled() {
         var debouncer = new TnTDebouncer(millisecondsDelay: 30);

         var result = await debouncer.DebounceForResultAsync<int>(_ => Task.FromResult(42));

         result.Should().Be(42);
      }

      [Fact]
      public async Task DebounceForResult_ReturnsDefault_WhenCanceledByNextCall() {
         var debouncer = new TnTDebouncer(millisecondsDelay: 40);

         var first = debouncer.DebounceForResultAsync<string>(_ => Task.FromResult("first"));
         var second = debouncer.DebounceForResultAsync<string>(_ => Task.FromResult("second"));

         var results = await Task.WhenAll(first, second);

         results[0].Should().BeNull();
         results[1].Should().Be("second");
      }

      [Fact]
      public async Task CancelAsync_PreventsPendingExecution() {
         var debouncer = new TnTDebouncer(millisecondsDelay: 60);
         bool executed = false;

         var task = debouncer.DebounceAsync(_ => { executed = true; return Task.CompletedTask; });

         await debouncer.CancelAsync();
         await task; // should complete without running the action

         await Task.Delay(80, Xunit.TestContext.Current.CancellationToken); // ensure enough time has passed beyond delay
         executed.Should().BeFalse();
      }

      [Fact]
      public async Task Dispose_CancelsPendingDelay_AndPreventsExecution() {
         var debouncer = new TnTDebouncer(millisecondsDelay: 60);
         bool executed = false;

         var task = debouncer.DebounceAsync(_ => { executed = true; return Task.CompletedTask; });

         debouncer.Dispose();
         await task; // swallow cancellation internally

         await Task.Delay(80, Xunit.TestContext.Current.CancellationToken);
         executed.Should().BeFalse();
      }

      [Fact]
      public async Task ActionToken_IsCanceled_WhenSubsequentCallOccurs() {
         var debouncer = new TnTDebouncer(millisecondsDelay: 20);
         var actionStarted = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
         bool wasCanceled = false;

         var first = debouncer.DebounceAsync(async token => {
            actionStarted.TrySetResult();
            try {
               await Task.Delay(TimeSpan.FromMinutes(1), token);
            }
            catch (OperationCanceledException) {
               wasCanceled = true;
            }
         });

         await actionStarted.Task; // ensure first action started

         // Trigger a new debounce call which cancels the previous token immediately
         var second = debouncer.DebounceAsync(_ => Task.CompletedTask);

         await Task.WhenAll(first, second);

         wasCanceled.Should().BeTrue();
      }

      [Fact]
      public async Task ZeroDelay_RunsImmediately() {
         var debouncer = new TnTDebouncer(millisecondsDelay: 0);
         bool executed = false;

         await debouncer.DebounceAsync(_ => { executed = true; return Task.CompletedTask; });

         executed.Should().BeTrue();
      }
   }
}
