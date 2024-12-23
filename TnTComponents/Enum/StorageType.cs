namespace TnTComponents;

/// <summary>
///     Specifies the type of storage to be used.
/// </summary>
internal enum StorageType {

    /// <summary>
    ///     Represents session storage.
    /// </summary>
    SessionStorage,

    /// <summary>
    ///     Represents local storage.
    /// </summary>
    LocalStorage
}

/// <summary>
///     Provides extension methods for the <see cref="StorageType" /> enum.
/// </summary>
internal static class StorageTypeExt {

    /// <summary>
    ///     Gets the string representation of the specified <see cref="StorageType" />.
    /// </summary>
    /// <param name="storageType">The storage type.</param>
    /// <returns>A string that represents the storage type.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown when the storage type is not recognized.
    /// </exception>
    public static string GetStorageType(this StorageType storageType) {
        return storageType switch {
            StorageType.SessionStorage => "sessionStorage",
            StorageType.LocalStorage => "localStorage",
            _ => throw new ArgumentOutOfRangeException(nameof(storageType), storageType, null)
        };
    }
}