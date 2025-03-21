﻿using Microsoft.JSInterop;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TnTComponents.Storage;

/// <summary>
///     Service for interacting with local storage.
/// </summary>
internal class LocalStorageService(IJSRuntime jsRuntime) : StorageService(jsRuntime), ILocalStorageService {
    internal override StorageType StorageType => StorageType.LocalStorage;
}

/// <summary>
///     Service for interacting with session storage.
/// </summary>
internal class SessionStorageService(IJSRuntime jsRuntime) : StorageService(jsRuntime), ISessionStorageService {
    internal override StorageType StorageType => StorageType.SessionStorage;
}

/// <summary>
///     Abstract base class for storage services.
/// </summary>
internal abstract class StorageService(IJSRuntime _jsRuntime) : IStorageService {

    /// <inheritdoc />
    public event EventHandler<ChangedEventArgs> Changed = default!;

    /// <inheritdoc />
    public event EventHandler<ChangingEventArgs> Changing = default!;

    /// <inheritdoc />
    internal abstract StorageType StorageType { get; }

    private string _storageType => StorageType.GetStorageType();
    private const string StorageNotAvailableMessage = "Unable to access the browser storage. This is most likely due to the browser settings.";

    /// <inheritdoc />
    public async ValueTask ClearAsync(CancellationToken cancellationToken = default) {
        try {
            await _jsRuntime.InvokeVoidAsync($"{_storageType}.clear", cancellationToken);
        }
        catch (Exception exception) {
            if (IsStorageDisabledException(exception)) {
                throw new BrowserStorageDisabledException(StorageNotAvailableMessage, exception);
            }

            throw;
        }
    }

    /// <inheritdoc />
    public async ValueTask<bool> ContainKeyAsync(string key, CancellationToken cancellationToken = default) {
        try {
            return await _jsRuntime.InvokeAsync<bool>($"{_storageType}.hasOwnProperty", cancellationToken, key);
        }
        catch (Exception exception) {
            if (IsStorageDisabledException(exception)) {
                throw new BrowserStorageDisabledException(StorageNotAvailableMessage, exception);
            }

            throw;
        }
    }

    /// <inheritdoc />
    public ValueTask<string?> GetItemAsStringAsync(string key, CancellationToken cancellationToken = default) {
        if (string.IsNullOrWhiteSpace(key)) {
            throw new ArgumentNullException(nameof(key));
        }

        return GetItemAsync<string>(key, DefaultJsonSerializerContext.Default, cancellationToken);
    }

    /// <inheritdoc />
    public async ValueTask<T?> GetItemAsync<T>(string key, JsonSerializerContext? serializerContext = null, CancellationToken cancellationToken = default) {
        if (string.IsNullOrWhiteSpace(key)) {
            throw new ArgumentNullException(nameof(key));
        }

        try {
            var serializedData = await _jsRuntime.InvokeAsync<string>($"{_storageType}.getItem", cancellationToken, key);

            if (string.IsNullOrWhiteSpace(serializedData)) {
                return default;
            }
            try {
                serializerContext ??= DefaultJsonSerializerContext.Default;

                var result = JsonSerializer.Deserialize(serializedData, typeof(T), serializerContext);
                return (T?)result;
            }
            catch (JsonException e) when (e.Path == "$" && typeof(T) == typeof(string)) {
                // For backward compatibility return the plain string. On the next save a correct
                // value will be stored and this Exception will not happen again, for this 'key'
                return (T)(object)serializedData;
            }
        }
        catch (Exception exception) {
            if (IsStorageDisabledException(exception)) {
                throw new BrowserStorageDisabledException(StorageNotAvailableMessage, exception);
            }

            throw;
        }
    }

    /// <inheritdoc />
    public async ValueTask<string> KeyAsync(int index, CancellationToken cancellationToken = default) {
        try {
            return await _jsRuntime.InvokeAsync<string>($"{_storageType}.key", cancellationToken, index);
        }
        catch (Exception exception) {
            if (IsStorageDisabledException(exception)) {
                throw new BrowserStorageDisabledException(StorageNotAvailableMessage, exception);
            }
            throw;
        }
    }

    /// <inheritdoc />
    public async ValueTask<IEnumerable<string>> KeysAsync(CancellationToken cancellationToken = default) {
        try {
            return await _jsRuntime.InvokeAsync<IEnumerable<string>>("eval", cancellationToken, $"Object.keys({_storageType})");
        }
        catch (Exception exception) {
            if (IsStorageDisabledException(exception)) {
                throw new BrowserStorageDisabledException(StorageNotAvailableMessage, exception);
            }

            throw;
        }
    }

    /// <inheritdoc />
    public async ValueTask<int> LengthAsync(CancellationToken cancellationToken = default) {
        try {
            return await _jsRuntime.InvokeAsync<int>("eval", cancellationToken, $"{_storageType}.length");
        }
        catch (Exception exception) {
            if (IsStorageDisabledException(exception)) {
                throw new BrowserStorageDisabledException(StorageNotAvailableMessage, exception);
            }

            throw;
        }
    }

    /// <inheritdoc />
    public async ValueTask RemoveItemAsync(string key, CancellationToken cancellationToken = default) {
        if (string.IsNullOrWhiteSpace(key)) {
            throw new ArgumentNullException(nameof(key));
        }

        try {
            await _jsRuntime.InvokeVoidAsync($"{_storageType}.removeItem", cancellationToken, key);
        }
        catch (Exception exception) {
            if (IsStorageDisabledException(exception)) {
                throw new BrowserStorageDisabledException(StorageNotAvailableMessage, exception);
            }

            throw;
        }
    }

    /// <inheritdoc />
    public async ValueTask RemoveItemsAsync(IEnumerable<string> keys, CancellationToken cancellationToken = default) {
        try {
            foreach (var key in keys) {
                await _jsRuntime.InvokeVoidAsync($"{_storageType}.removeItem", cancellationToken, key);
            }
        }
        catch (Exception exception) {
            if (IsStorageDisabledException(exception)) {
                throw new BrowserStorageDisabledException(StorageNotAvailableMessage, exception);
            }

            throw;
        }
    }

    /// <inheritdoc />
    public async ValueTask SetItemAsStringAsync(string key, string data, CancellationToken cancellationToken = default) {
        if (string.IsNullOrWhiteSpace(key)) {
            throw new ArgumentNullException(nameof(key));
        }
        ArgumentNullException.ThrowIfNull(data);

        var oldValue = GetItemAsStringAsync(key, cancellationToken);
        var changingArgs = new ChangingEventArgs { Key = key, OldValue = oldValue, NewValue = data };
        Changing?.Invoke(this, changingArgs);

        if (changingArgs.Cancel) {
            return;
        }

        try {
            await _jsRuntime.InvokeVoidAsync($"{_storageType}.setItem", cancellationToken, key, data);
            Changed?.Invoke(this, new ChangedEventArgs { Key = key, OldValue = changingArgs.OldValue, NewValue = data });
        }
        catch (Exception exception) {
            if (IsStorageDisabledException(exception)) {
                throw new BrowserStorageDisabledException(StorageNotAvailableMessage, exception);
            }

            throw;
        }

        Changed?.Invoke(this, new ChangedEventArgs { Key = key, OldValue = changingArgs.OldValue, NewValue = data });
    }

    /// <inheritdoc />
    public async ValueTask SetItemAsync<T>(string key, T data, JsonSerializerContext? serializerContext = null, CancellationToken cancellationToken = default) {
        if (string.IsNullOrWhiteSpace(key)) {
            throw new ArgumentNullException(nameof(key));
        }

        var oldValue = GetItemAsStringAsync(key, cancellationToken);
        var changingArgs = new ChangingEventArgs { Key = key, OldValue = oldValue, NewValue = data };
        Changing?.Invoke(this, changingArgs);

        if (changingArgs.Cancel) {
            return;
        }

        serializerContext ??= DefaultJsonSerializerContext.Default;

        var serializedData = JsonSerializer.Serialize(data, typeof(T), serializerContext);
        try {
            await _jsRuntime.InvokeVoidAsync($"{_storageType}.setItem", cancellationToken, key, serializedData);
            Changed?.Invoke(this, new ChangedEventArgs { Key = key, OldValue = changingArgs.OldValue, NewValue = serializedData });
        }
        catch (Exception exception) {
            if (IsStorageDisabledException(exception)) {
                throw new BrowserStorageDisabledException(StorageNotAvailableMessage, exception);
            }

            throw;
        }
    }

    /// <summary>
    ///     Determines whether the exception is caused by storage being disabled.
    /// </summary>
    /// <param name="exception">The exception to check.</param>
    /// <returns>
    ///     <c>true</c> if the exception is caused by storage being disabled; otherwise, <c>false</c>.
    /// </returns>
    private bool IsStorageDisabledException(Exception exception)
        => exception.Message.Contains($"Failed to read the '{StorageType}' property from 'Window'");
}

[JsonSerializable(typeof(string))]
[JsonSerializable(typeof(char))]
[JsonSerializable(typeof(byte))]
[JsonSerializable(typeof(short))]
[JsonSerializable(typeof(ushort))]
[JsonSerializable(typeof(int))]
[JsonSerializable(typeof(uint))]
[JsonSerializable(typeof(long))]
[JsonSerializable(typeof(ulong))]
[JsonSerializable(typeof(BigInteger))]
[JsonSerializable(typeof(float))]
[JsonSerializable(typeof(double))]
[JsonSerializable(typeof(decimal))]
internal partial class DefaultJsonSerializerContext : JsonSerializerContext;
