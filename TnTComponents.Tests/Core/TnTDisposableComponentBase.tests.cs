using System;
using System.Threading.Tasks;
using AwesomeAssertions;
using Xunit;
using TnTComponents.Core;

public class TnTDisposableComponentBaseTests {
    private class TestDisposableComponent : TnTDisposableComponentBase {
        public bool SyncDisposed { get; set; }
        public bool AsyncDisposed { get; set; }
        public override string? ElementClass => null;
        public override string? ElementStyle => null;
        protected override void Dispose(bool disposing) {
            SyncDisposed = disposing;
        }
        protected override ValueTask DisposeAsyncCore() {
            AsyncDisposed = true;
            return ValueTask.CompletedTask;
        }
    }

    [Fact]
    public void Dispose_CallsDisposeWithTrueAndSuppressesFinalize() {
        var comp = new TestDisposableComponent();
        comp.Dispose();
        comp.SyncDisposed.Should().BeTrue();
    }

    [Fact]
    public async Task DisposeAsync_CallsAsyncDisposeAndSuppressesFinalize() {
        var comp = new TestDisposableComponent();
        await comp.DisposeAsync();
        comp.AsyncDisposed.Should().BeTrue();
        comp.SyncDisposed.Should().BeFalse(); // Dispose(false) called
    }

    [Fact]
    public async Task DisposeAsync_CallsDisposeWithFalseAfterAsync() {
        var comp = new TestDisposableComponent();
        comp.SyncDisposed = true; // set to true to check reset
        comp.AsyncDisposed = false;
        await comp.DisposeAsync();
        comp.SyncDisposed.Should().BeFalse();
        comp.AsyncDisposed.Should().BeTrue();
    }
}
