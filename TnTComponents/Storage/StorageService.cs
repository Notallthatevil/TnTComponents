using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using TnTComponents.Enum;

namespace TnTComponents.Storage;

internal class SessionStorageService(IJSRuntime jsRuntime) : StorageService(jsRuntime), ISessionStorageService {
    internal override StorageType StorageType => StorageType.SessionStorage;
}

internal class LocalStorageService(IJSRuntime jsRuntime) : StorageService(jsRuntime), ILocalStorageService {
    internal override StorageType StorageType => StorageType.LocalStorage;
}

internal abstract class StorageService(IJSRuntime _jsRuntime) : IStorageService {

    public event EventHandler<ChangedEventArgs> Changed;

    public event EventHandler<ChangingEventArgs> Changing;

    internal abstract StorageType StorageType { get; }
    private string _storageType => StorageType.GetStorageType();
    private const string StorageNotAvailableMessage = "Unable to access the browser storage. This is most likely due to the browser settings.";

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

    public ValueTask<string?> GetItemAsStringAsync(string key, CancellationToken cancellationToken = default) {
        if (string.IsNullOrWhiteSpace(key)) {
            throw new ArgumentNullException(nameof(key));
        }

        return GetItemAsync<string>(key, cancellationToken);
    }

    public async ValueTask<T?> GetItemAsync<T>(string key, CancellationToken cancellationToken = default) {
        if (string.IsNullOrWhiteSpace(key)) {
            throw new ArgumentNullException(nameof(key));
        }

        try {
            var serializedData = await _jsRuntime.InvokeAsync<string>($"{_storageType}.getItem", cancellationToken, key);

            if (string.IsNullOrWhiteSpace(serializedData)) {
                return default;
            }
            try {
                return JsonSerializer.Deserialize<T>(serializedData);
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

    public async ValueTask SetItemAsync<T>(string key, T data, CancellationToken cancellationToken = default) {
        if (string.IsNullOrWhiteSpace(key)) {
            throw new ArgumentNullException(nameof(key));
        }

        var oldValue = GetItemAsStringAsync(key, cancellationToken);
        var changingArgs = new ChangingEventArgs { Key = key, OldValue = oldValue, NewValue = data };
        Changing?.Invoke(this, changingArgs);

        if (changingArgs.Cancel) {
            return;
        }

        var serializedData = JsonSerializer.Serialize(data);
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

    private bool IsStorageDisabledException(Exception exception)
        => exception.Message.Contains($"Failed to read the '{StorageType}' property from 'Window'");
}