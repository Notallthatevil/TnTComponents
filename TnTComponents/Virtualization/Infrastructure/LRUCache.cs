using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TnTComponents.Virtualization.Infrastructure;

/// <summary>
///     Represents a Least Recently Used (LRU) cache.
/// </summary>
/// <typeparam name="TKey">The type of the keys in the cache.</typeparam>
/// <typeparam name="TValue">The type of the values in the cache.</typeparam>
internal class LRUCache<TKey, TValue>(int capacity) : IDictionary<TKey, TValue> where TKey : notnull {

    /// <summary>
    ///     Gets the capacity of the cache.
    /// </summary>
    public int Capacity { get; } = capacity;

    public ICollection<TKey> Keys => _cacheMap.Keys;

    public ICollection<TValue> Values => [.. _cacheMap.Values.Select(node => node.Value.Value)];

    public int Count => _cacheMap.Count;

    public bool IsReadOnly => false;

    public TValue this[TKey key] { get => Get(key).Item2!; set => Add(key, value); }

    private readonly Dictionary<TKey, LinkedListNode<KeyValuePair<TKey, TValue>>> _cacheMap = [];
    private readonly LinkedList<KeyValuePair<TKey, TValue>> _lruList = new();

    /// <summary>
    ///     Adds a key-value pair to the cache.
    /// </summary>
    /// <param name="key">The key of the element to add.</param>
    /// <param name="val">The value of the element to add.</param>
    public void Add(TKey key, TValue val) {
        if (_cacheMap.TryGetValue(key, out var existingNode)) {
            _lruList.Remove(existingNode);
        }
        else if (_cacheMap.Count >= Capacity) {
            RemoveFirst();
        }

        var cacheItem = new KeyValuePair<TKey, TValue>(key, val);
        var node = new LinkedListNode<KeyValuePair<TKey, TValue>>(cacheItem);
        _lruList.AddLast(node);
        _cacheMap[key] = node;
    }

    /// <summary>
    ///     Gets the value associated with the specified key.
    /// </summary>
    /// <param name="key">The key of the value to get.</param>
    /// <returns>
    ///     A tuple containing a boolean indicating whether the key was found and the value
    ///     associated with the key.
    /// </returns>
    /// <remarks>Calling this method, resets the items last access in the cache.</remarks>
    private (bool, TValue?) Get(TKey key) {
        if (_cacheMap.TryGetValue(key, out var node) && node is not null) {
            var value = node.Value.Value;
            _lruList.Remove(node);
            _lruList.AddLast(node);
            return new(true, value);
        }
        return new(false, default);
    }

    /// <summary>
    ///     Removes the first element from the cache.
    /// </summary>
    private void RemoveFirst() {
        var node = _lruList.First;
        _lruList.RemoveFirst();

        if (node is not null) {
            _cacheMap.Remove(node.Value.Key);
        }
    }

    public bool ContainsKey(TKey key) => _cacheMap.ContainsKey(key);
    public bool Remove(TKey key) => _cacheMap.Remove(key);
    public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value) {
        var result = Get(key);
        if (result.Item1) {
            value = result.Item2!;
            return true;
        }
        value = default;
        return false;
    }
    public void Add(KeyValuePair<TKey, TValue> item) => Add(item.Key, item.Value);
    public void Clear() {
        _lruList.Clear();
        _cacheMap.Clear();
    }
    public bool Contains(KeyValuePair<TKey, TValue> item) {
        if (_cacheMap.TryGetValue(item.Key, out var node)) {
            return EqualityComparer<TValue>.Default.Equals(node.Value.Value, item.Value);
        }
        return false;
    }
    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => ((IDictionary<TKey, TValue>)_cacheMap.ToDictionary(kv => kv.Key, kv => kv.Value.Value.Value)).CopyTo(array, arrayIndex);
    public bool Remove(KeyValuePair<TKey, TValue> item) {
        if (Contains(item)) {
            Remove(item.Key);
            return true;
        }
        return false;
    }
    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _cacheMap.Select(kv => new KeyValuePair<TKey, TValue>(kv.Key, kv.Value.Value.Value)).GetEnumerator(); 
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}