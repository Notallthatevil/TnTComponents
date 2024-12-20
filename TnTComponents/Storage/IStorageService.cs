namespace TnTComponents.Storage;
/*
This implementation was heavliy influenced by Blazored.LocalStorage and Blazored.SessionStorage

MIT License

Copyright (c) 2019 Blazored

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
 */

public interface IStorageService {

    event EventHandler<ChangedEventArgs> Changed;

    event EventHandler<ChangingEventArgs> Changing;

    /// <summary>
    ///     Clears all data from session storage.
    /// </summary>
    /// <returns>A <see cref="ValueTask" /> representing the completion of the operation.</returns>
    ValueTask ClearAsync(CancellationToken cancellationToken = default);

    /// <summary>
    ///     Checks if the <paramref name="key" /> exists in session storage, but does not check its value.
    /// </summary>
    /// <param name="key">              
    ///     A <see cref="string" /> value specifying the name of the storage slot to use
    /// </param>
    /// <param name="cancellationToken">
    ///     A cancellation token to signal the cancellation of the operation. Specifying this
    ///     parameter will override any default cancellations such as due to timeouts ( <see
    ///     cref="JSRuntime.DefaultAsyncTimeout" />) from being applied.
    /// </param>
    /// <returns>A <see cref="ValueTask" /> representing the completion of the operation.</returns>
    ValueTask<bool> ContainKeyAsync(string key, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Retrieve the specified data from session storage as a <see cref="string" />.
    /// </summary>
    /// <param name="key">              
    ///     A <see cref="string" /> value specifying the name of the storage slot to use
    /// </param>
    /// <param name="cancellationToken">
    ///     A cancellation token to signal the cancellation of the operation. Specifying this
    ///     parameter will override any default cancellations such as due to timeouts ( <see
    ///     cref="JSRuntime.DefaultAsyncTimeout" />) from being applied.
    /// </param>
    /// <returns>A <see cref="ValueTask" /> representing the completion of the operation.</returns>
    ValueTask<string?> GetItemAsStringAsync(string key, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Retrieve the specified data from session storage and deseralise it to the specfied type.
    /// </summary>
    /// <param name="key">              
    ///     A <see cref="string" /> value specifying the name of the session storage slot to use
    /// </param>
    /// <param name="cancellationToken">
    ///     A cancellation token to signal the cancellation of the operation. Specifying this
    ///     parameter will override any default cancellations such as due to timeouts ( <see
    ///     cref="JSRuntime.DefaultAsyncTimeout" />) from being applied.
    /// </param>
    /// <returns>A <see cref="ValueTask" /> representing the completion of the operation.</returns>
    ValueTask<T?> GetItemAsync<T>(string key, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Return the name of the key at the specified <paramref name="index" />.
    /// </summary>
    /// <param name="index">            </param>
    /// <param name="cancellationToken">
    ///     A cancellation token to signal the cancellation of the operation. Specifying this
    ///     parameter will override any default cancellations such as due to timeouts ( <see
    ///     cref="JSRuntime.DefaultAsyncTimeout" />) from being applied.
    /// </param>
    /// <returns>A <see cref="ValueTask" /> representing the completion of the operation.</returns>
    ValueTask<string> KeyAsync(int index, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Returns a collection of strings representing the names of the keys in the Session storage.
    /// </summary>
    /// <param name="cancellationToken">
    ///     A cancellation token to signal the cancellation of the operation. Specifying this
    ///     parameter will override any default cancellations such as due to timeouts ( <see
    ///     cref="JSRuntime.DefaultAsyncTimeout" />) from being applied.
    /// </param>
    /// <returns>A <see cref="ValueTask" /> representing the completion of the operation.</returns>
    ValueTask<IEnumerable<string>> KeysAsync(CancellationToken cancellationToken = default);

    /// <summary>
    ///     The number of items stored in session storage.
    /// </summary>
    /// <param name="cancellationToken">
    ///     A cancellation token to signal the cancellation of the operation. Specifying this
    ///     parameter will override any default cancellations such as due to timeouts ( <see
    ///     cref="JSRuntime.DefaultAsyncTimeout" />) from being applied.
    /// </param>
    /// <returns>A <see cref="ValueTask" /> representing the completion of the operation.</returns>
    ValueTask<int> LengthAsync(CancellationToken cancellationToken = default);

    /// <summary>
    ///     Remove the data with the specified <paramref name="key" />.
    /// </summary>
    /// <param name="key">              
    ///     A <see cref="string" /> value specifying the name of the storage slot to use
    /// </param>
    /// <param name="cancellationToken">
    ///     A cancellation token to signal the cancellation of the operation. Specifying this
    ///     parameter will override any default cancellations such as due to timeouts ( <see
    ///     cref="JSRuntime.DefaultAsyncTimeout" />) from being applied.
    /// </param>
    /// <returns>A <see cref="ValueTask" /> representing the completion of the operation.</returns>
    ValueTask RemoveItemAsync(string key, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Removes a collection of <paramref name="keys" />.
    /// </summary>
    /// <param name="keys">             
    ///     A IEnumerable collection of strings specifying the name of the storage slot to remove
    /// </param>
    /// <param name="cancellationToken">
    ///     A cancellation token to signal the cancellation of the operation. Specifying this
    ///     parameter will override any default cancellations such as due to timeouts ( <see
    ///     cref="JSRuntime.DefaultAsyncTimeout" />) from being applied.
    /// </param>
    /// <returns>A <see cref="ValueTask" /> representing the completion of the operation.</returns>
    ValueTask RemoveItemsAsync(IEnumerable<string> keys, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Sets or updates the <paramref name="data" /> in session storage with the specified
    ///     <paramref name="key" />. Does not serialize the value before storing.
    /// </summary>
    /// <param name="key">              
    ///     A <see cref="string" /> value specifying the name of the storage slot to use
    /// </param>
    /// <param name="data">             The string to be saved</param>
    /// <param name="cancellationToken">
    ///     A cancellation token to signal the cancellation of the operation. Specifying this
    ///     parameter will override any default cancellations such as due to timeouts ( <see
    ///     cref="JSRuntime.DefaultAsyncTimeout" />) from being applied.
    /// </param>
    /// <returns>A <see cref="ValueTask" /> representing the completion of the operation.</returns>
    ValueTask SetItemAsStringAsync(string key, string data, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Sets or updates the <paramref name="data" /> in session storage with the specified
    ///     <paramref name="key" />.
    /// </summary>
    /// <param name="key">              
    ///     A <see cref="string" /> value specifying the name of the storage slot to use
    /// </param>
    /// <param name="data">             The data to be saved</param>
    /// <param name="cancellationToken">
    ///     A cancellation token to signal the cancellation of the operation. Specifying this
    ///     parameter will override any default cancellations such as due to timeouts ( <see
    ///     cref="JSRuntime.DefaultAsyncTimeout" />) from being applied.
    /// </param>
    /// <returns>A <see cref="ValueTask" /> representing the completion of the operation.</returns>
    ValueTask SetItemAsync<T>(string key, T data, CancellationToken cancellationToken = default);
}