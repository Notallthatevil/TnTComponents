namespace TnTComponents;

internal enum StorageType {
    SessionStorage,
    LocalStorage
}

internal static class StorageTypeExt {

    public static string GetStorageType(this StorageType storageType) {
        return storageType switch {
            StorageType.SessionStorage => "sessionStorage",
            StorageType.LocalStorage => "localStorage",
            _ => throw new ArgumentOutOfRangeException(nameof(storageType), storageType, null)
        };
    }
}