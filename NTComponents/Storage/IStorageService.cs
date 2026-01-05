using System.Text.Json.Serialization;

namespace NTComponents.Storage;
/*
This implementation was heavily influenced by Blazored.LocalStorage and Blazored.SessionStorage

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

/// <summary>
///     Interface for storage service to handle storage operations.
/// </summary>
public interface IStorageService {

    /// <summary>
    ///     Event triggered when a storage item is changed.
    /// </summary>
    event EventHandler<ChangedEventArgs> Changed;

    /// <summary>
    ///     Event triggered when a storage item is about to change.
    /// </summary>
    event EventHandler<ChangingEventArgs> Changing;

    /// <summary>
    ///     Clears all items in the storage.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task that represents the asynchronous clear operation.</returns>
    ValueTask ClearAsync(CancellationToken cancellationToken = default);

    /// <summary>
    ///     Checks if a key exists in the storage.
    /// </summary>
    /// <param name="key">              The key to check.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains true if the key exists; otherwise, false.</returns>
    ValueTask<bool> ContainKeyAsync(string key, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Gets an item from the storage as a string.
    /// </summary>
    /// <param name="key">              The key of the item to get.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the item as a string.</returns>
    ValueTask<string?> GetItemAsStringAsync(string key, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Gets an item from the storage.
    /// </summary>
    /// <typeparam name="T">The type of the item to get.</typeparam>
    /// <param name="key">              The key of the item to get.</param>
    /// <param name="serializerContext">
    ///     The serializer context. This is required if trying to deserialize a custom type, other then a primitive. This is due to AOT and trimming. <see
    ///     href="https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/source-generation" />
    /// </param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the item.</returns>
    ValueTask<T?> GetItemAsync<T>(string key, JsonSerializerContext? serializerContext = null, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Gets the key at the specified index.
    /// </summary>
    /// <param name="index">            The index of the key to get.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the key.</returns>
    ValueTask<string> KeyAsync(int index, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Gets all keys in the storage.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the keys.</returns>
    ValueTask<IEnumerable<string>> KeysAsync(CancellationToken cancellationToken = default);

    /// <summary>
    ///     Gets the number of items in the storage.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the number of items.</returns>
    ValueTask<int> LengthAsync(CancellationToken cancellationToken = default);

    /// <summary>
    ///     Removes an item from the storage.
    /// </summary>
    /// <param name="key">              The key of the item to remove.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    ValueTask RemoveItemAsync(string key, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Removes multiple items from the storage.
    /// </summary>
    /// <param name="keys">             The keys of the items to remove.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    ValueTask RemoveItemsAsync(IEnumerable<string> keys, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Sets an item in the storage as a string.
    /// </summary>
    /// <param name="key">              The key of the item to set.</param>
    /// <param name="data">             The data to set.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    ValueTask SetItemAsStringAsync(string key, string data, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Sets an item in the storage.
    /// </summary>
    /// <typeparam name="T">The type of the item to set.</typeparam>
    /// <param name="key">              The key of the item to set.</param>
    /// <param name="data">             The data to set.</param>
    /// <param name="serializerContext">
    ///     The serializer context. This is required if trying to deserialize a custom type, other then a primitive. This is due to AOT and trimming. <see
    ///     href="https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/source-generation" />
    /// </param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    ValueTask SetItemAsync<T>(string key, T data, JsonSerializerContext? serializerContext = null, CancellationToken cancellationToken = default);
}