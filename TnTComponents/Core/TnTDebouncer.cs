//MIT License

//Copyright(c) 2019 - 2024 HAVIT
//
//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:
//
//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.
//
//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.
// https://github.com/havit/Havit.Blazor/blob/master/Havit.Blazor.Components.Web/Debouncer.cs

namespace TnTComponents.Core;

/// <summary>
/// Debouncer helps you to debounce asynchronous actions.
/// You can use it in your callbacks to prevent multiple calls of the same action in a short period of time.
/// </summary>
public class TnTDebouncer : IDisposable {
    private CancellationTokenSource _debounceCancellationTokenSource = new();

    private async Task DebounceAsync(int millisecondsDelay) {
        await _debounceCancellationTokenSource.CancelAsync();
        _debounceCancellationTokenSource.Dispose();
        _debounceCancellationTokenSource = new();

        await Task.Delay(millisecondsDelay, _debounceCancellationTokenSource.Token);
    }

    /// <summary>
    /// Starts the debouncing.
    /// </summary>
    /// <param name="millisecondsDelay">The delay in milliseconds for debouncing.</param>
    /// <param name="actionAsync">The asynchronous action to be executed. The <see cref="CancellationToken"/> gets canceled if the method is called again.</param>
    public async Task DebounceAsync(int millisecondsDelay, Func<CancellationToken, Task> actionAsync) {
        try {
            await DebounceAsync(millisecondsDelay);
           
            await actionAsync(_debounceCancellationTokenSource.Token);
        }
        catch (TaskCanceledException) { }
    }


    public async Task<T> DebounceForResultAsync<T>(int millisecondsDelay, Func<CancellationToken, Task<T>> funcAsync) {
        try {
            await DebounceAsync(millisecondsDelay);

            return await funcAsync(_debounceCancellationTokenSource.Token);
        }
        catch (TaskCanceledException) { }
        return default!;
    }

    public Task CancelAsync() {
        return _debounceCancellationTokenSource.CancelAsync();
    }

    public void Dispose() {
        GC.SuppressFinalize(this);
        _debounceCancellationTokenSource.Cancel();
        _debounceCancellationTokenSource.Dispose();
    }
}

