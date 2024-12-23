namespace TnTComponents;

/// <summary>
///     Specifies the mode for handling input files.
/// </summary>
public enum InputFileMode {

    /// <summary>
    ///     Uploaded files are saved to a temporary folder.
    /// </summary>
    SaveToTemporaryFolder,

    /// <summary>
    ///     Files are read into a buffer. Use <see cref="Forms.TnTInputFileEventArgs.Buffer" /> to
    ///     retrieve bytes.
    /// </summary>
    Buffer,

    /// <summary>
    ///     In Blazor Server, file data is streamed over the SignalR connection into .NET code on
    ///     the server as the file is read. In Blazor WebAssembly, file data is streamed directly
    ///     into the .NET code within the browser.
    /// </summary>
    Stream,
}