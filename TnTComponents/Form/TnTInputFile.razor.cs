using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;
using System.Reflection;
using TnTComponents.Core;
using TnTComponents.Ext;
using TnTComponents.Form;
using TnTComponents.Interfaces;

namespace TnTComponents;

/// <summary>
///     a
/// </summary>
public class FluentInputFileBuffer {

    /// <summary>
    ///     Number of bytes read.
    /// </summary>
    public int BytesRead { get; }

    /// <summary>
    ///     Buffer data read.
    /// </summary>
    public byte[] Data { get; }

    /// <summary>
    ///     a
    /// </summary>
    /// <param name="data">     </param>
    /// <param name="bytesRead"></param>
    public FluentInputFileBuffer(byte[] data, int bytesRead) {
        Data = data;
        BytesRead = bytesRead;
    }

    /// <summary>
    ///     Append the current buffer (Data) to this file.
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    public async Task AppendToFileAsync(string file) {
        using var stream = new FileStream(file, FileMode.Append);
        await stream.WriteAsync(Data.AsMemory(0, BytesRead));
    }

    /// <summary>
    ///     Append the current buffer (Data) to this file.
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    public Task AppendToFileAsync(FileInfo file) {
        return AppendToFileAsync(file.FullName);
    }
}

/// <summary>
///     A
/// </summary>
public class FluentInputFileEventArgs : EventArgs {

    /// <summary>
    ///     Gets a list of all files currently in an upload process.
    /// </summary>
    public IEnumerable<UploadedFileDetails> AllFiles { get; internal set; } = default!;

    /// <summary>
    ///     Gets a small buffer data of the current file in an upload process. Only if Mode = Buffer.
    /// </summary>
    public FluentInputFileBuffer Buffer { get; internal set; } = default!;

    /// <summary>
    ///     Gets the content type of the current file in an upload process.
    /// </summary>
    public string ContentType { get; internal set; } = string.Empty;

    /// <summary>
    ///     Gets the error message (or null if no error occurred).
    /// </summary>
    public string? ErrorMessage { get; internal set; }

    /// <summary>
    ///     Gets the index of the current file in an upload process.
    /// </summary>
    public int Index { get; internal set; } = 0;

    /// <summary>
    ///     Set this property to True to cancel the current upload file.
    /// </summary>
    public bool IsCancelled { get; set; } = false;

    /// <summary>
    ///     Gets the last modified date of the current file in an upload process.
    /// </summary>
    public DateTimeOffset LastModified { get; internal set; } = default!;

    /// <summary>
    ///     Gets the local file of the current file in an upload process. Only if Mode = SaveToTemporaryFolder (otherwise, this value is null).
    /// </summary>
    public FileInfo? LocalFile { get; internal set; }

    /// <summary>
    ///     Gets the name of the current file in an upload process.
    /// </summary>
    public string Name { get; internal set; } = string.Empty;

    /// <summary>
    ///     Gets the global percent value in an upload process.
    /// </summary>
    public int ProgressPercent { get; internal set; } = 0;

    /// <summary>
    ///     Gets the label to display in an upload process.
    /// </summary>
    public string ProgressTitle { get; internal set; } = string.Empty;

    /// <summary>
    ///     Gets the size (in bytes) of the current file in an upload process.
    /// </summary>
    public long Size { get; internal set; } = 0;

    /// <summary>
    ///     Gets a reference to the current stream in an upload process. Only if Mode = Stream (otherwise, this value is null). The OnProgressChange event will not be triggered.
    /// </summary>
    public Stream? Stream { get; internal set; }
}

/// <summary>
///     Represents a custom input file component with additional features and styling options.
/// </summary>
public partial class TnTInputFile {

    /// <summary>
    ///     Gets or sets the appearance of the input file component.
    /// </summary>
    [Parameter]
    public FormAppearance Appearance { get; set; }

    /// <inheritdoc />
    [Parameter]
    public bool? AutoFocus { get; set; }

    /// <summary>
    ///     Gets or sets the background color of the input.
    /// </summary>
    [Parameter]
    public TnTColor BackgroundColor { get; set; } = TnTColor.SurfaceContainerHighest;

    /// <summary>
    ///     Gets or sets the sze of buffer to read bytes from uploaded file (in bytes). Default value is 10 KiB.
    /// </summary>
    [Parameter]
    public uint BufferSize { get; set; } = 10 * 1024;

    /// <inheritdoc />
    [Parameter]
    public bool Disabled { get; set; }

    /// <summary>
    ///     Gets the reference to the DotNet object associated with the component.
    /// </summary>
    public DotNetObjectReference<TnTInputFile>? DotNetObjectRef { get; private set; }

    /// <inheritdoc />
    public string? ElementClass => CssClassBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes?.ToDictionary())
        .AddClass("tnt-input")
        .AddClass("tnt-form-filled", _tntForm is not null ? _tntForm.Appearance == FormAppearance.Filled : Appearance == FormAppearance.Filled)
        .AddClass("tnt-form-outlined", _tntForm is not null ? _tntForm.Appearance == FormAppearance.Outlined : Appearance == FormAppearance.Outlined)
        .AddRipple(EnableRipple)
        .AddDisabled(FieldDisabled)
        .Build();

    /// <inheritdoc />
    [Parameter]
    public string? ElementId { get; set; }

    /// <inheritdoc />
    [Parameter]
    public string? ElementLang { get; set; }

    /// <inheritdoc />
    [Parameter]
    public string? ElementName { get; set; }

    /// <inheritdoc />
    public string? ElementStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes?.ToDictionary())
        .AddVariable("tnt-input-on-tint-color", OnTintColor.ToCssTnTColorVariable())
        .AddVariable("tnt-input-tint-color", TintColor.ToCssTnTColorVariable())
        .AddVariable("tnt-input-background-color", BackgroundColor.ToCssTnTColorVariable())
        .AddVariable("tnt-input-text-color", TextColor.ToCssTnTColorVariable())
        .AddVariable("tnt-input-error-color", ErrorColor.ToCssTnTColorVariable())
        .Build();

    /// <inheritdoc />
    [Parameter]
    public string? ElementTitle { get; set; }

    /// <inheritdoc />
    [Parameter]
    public bool EnableRipple { get; set; }

    /// <summary>
    ///     Gets or sets the end icon of the input.
    /// </summary>
    [Parameter]
    public TnTIcon? EndIcon { get; set; }

    /// <summary>
    ///     The color used for the error state of the input.
    /// </summary>
    [Parameter]
    public TnTColor ErrorColor { get; set; } = TnTColor.Error;

    /// <summary>
    ///     Gets a value indicating whether the input file component is disabled, considering the form's state.
    /// </summary>
    public bool FieldDisabled => _tntForm?.Disabled is not null ? _tntForm.Disabled : Disabled;

    /// <summary>
    ///     Gets a value indicating whether the input file component is read-only, considering the form's state.
    /// </summary>
    public bool FieldReadonly => _tntForm?.ReadOnly is not null ? _tntForm.ReadOnly : ReadOnly;

    /// <summary>
    ///     Gets the reference to the isolated JavaScript module.
    /// </summary>
    public IJSObjectReference? IsolatedJsModule { get; private set; }

    /// <summary>
    ///     Gets the path of the JavaScript module.
    /// </summary>
    public string? JsModulePath => "./_content/TnTComponents/Form/TnTInputFile.razor.js";

    /// <summary>
    ///     The label for this input.
    /// </summary>
    [Parameter]
    public string? Label { get; set; }

    /// <summary>
    ///     Gets or sets the maximum number of files that can be selected.
    /// </summary>
    [Parameter]
    public int MaximumFileCount { get; set; } = 1;

    /// <summary>
    ///     Gets or sets the maximum size of a file to be uploaded (in bytes). Default value is 10 MiB.
    /// </summary>
    [Parameter]
    public long MaximumFileSize { get; set; } = 10 * 1024 * 1024;

    /// <summary>
    ///     Gets or sets the type of file reading. For SaveToTemporaryFolder, use <see cref="FluentInputFileEventArgs.LocalFile" /> to retrieve the file. For Buffer, use <see
    ///     cref="FluentInputFileEventArgs.Buffer" /> to retrieve bytes. For Stream, use <see cref="FluentInputFileEventArgs.Stream" /> to have full control over retrieving the file.
    /// </summary>
    [Parameter]
    public InputFileMode Mode { get; set; } = InputFileMode.SaveToTemporaryFolder;

    /// <summary>
    ///     Raise when all files are completely uploaded.
    /// </summary>
    [Parameter]
    public EventCallback<IEnumerable<FluentInputFileEventArgs>> OnCompleted { get; set; }

    /// <summary>
    ///     Raised when the <see cref="MaximumFileCount" /> is exceeded. The return parameter specifies the total number of files that were attempted for upload.
    /// </summary>
    [Parameter]
    public EventCallback<int> OnFileCountExceeded { get; set; }

    /// <summary>
    ///     Raise when a file raised an error. Not yet used.
    /// </summary>
    [Parameter]
    public EventCallback<FluentInputFileEventArgs> OnFileError { get; set; }

    /// <summary>
    ///     Raise when a file is completely uploaded.
    /// </summary>
    [Parameter]
    public EventCallback<FluentInputFileEventArgs> OnFileUploaded { get; set; }

    /// <summary>
    ///     Use the native event raised by the <seealso href="https://docs.microsoft.com/en-us/aspnet/core/blazor/file-uploads">InputFile</seealso> component. If you use this event, the <see
    ///     cref="OnFileUploaded" /> will never be raised.
    /// </summary>
    [Parameter]
    public EventCallback<InputFileChangeEventArgs> OnInputFileChange { get; set; }

    /// <summary>
    ///     Raise when a progression step is updated.
    /// </summary>
    [Parameter]
    public EventCallback<FluentInputFileEventArgs> OnProgressChange { get; set; }

    /// <inheritdoc />
    [Parameter]
    public TnTColor OnTintColor { get; set; } = TnTColor.OnPrimary;

    /// <summary>
    ///     Gets or sets the current global value of the percentage of a current upload.
    /// </summary>
    [Parameter]
    public int ProgressPercent { get; set; } = 0;

    /// <summary>
    ///     Gets or sets a callback that updates the <see cref="ProgressPercent" />.
    /// </summary>
    [Parameter]
    public EventCallback<int> ProgressPercentChanged { get; set; }

    /// <summary>
    ///     Gets or sets the progress content of the component.
    /// </summary>
    [Parameter]
    public RenderFragment<ProgressFileDetails>? ProgressTemplate { get; set; }

    /// <summary>
    ///     Gets the current label display when an upload is in progress.
    /// </summary>
    public string ProgressTitle { get; private set; } = string.Empty;

    /// <summary>
    ///     Gets or sets a value indicating whether the input file component is read-only.
    /// </summary>
    [Parameter]
    public bool ReadOnly { get; set; }

    /// <summary>
    ///     Gets or sets the start icon of the input.
    /// </summary>
    [Parameter]
    public TnTIcon? StartIcon { get; set; }

    /// <summary>
    ///     Gets or sets the text color of the input.
    /// </summary>
    [Parameter]
    public TnTColor TextColor { get; set; } = TnTColor.OnSurface;

    /// <inheritdoc />
    [Parameter]
    public TnTColor TintColor { get; set; } = TnTColor.Primary;

    /// <summary>
    ///     A
    /// </summary>
    public static string ResourceLoadingBefore = "Loading...";

    /// <summary>
    ///     A
    /// </summary>
    public static string ResourceLoadingCanceled = "Canceled";

    /// <summary>
    ///     A
    /// </summary>
    public static string ResourceLoadingCompleted = "Completed";

    /// <summary>
    ///     A
    /// </summary>
    public static string ResourceLoadingInProgress = "Loading {0}/{1} - {2}";

    /// <inheritdoc />
    IReadOnlyDictionary<string, object>? ITnTComponentBase.AdditionalAttributes { get => base.AdditionalAttributes?.ToDictionary(); set => base.AdditionalAttributes = value is null ? null : new Dictionary<string, object>(value!); }

    /// <inheritdoc />
    ElementReference ITnTComponentBase.Element => base.Element ?? default;

    /// <summary>
    ///     The JSRuntime instance used for JavaScript interop.
    /// </summary>
    [Inject]
    protected IJSRuntime JSRuntime { get; private set; } = default!;

    /// <summary>
    ///     Gets or sets the cascading parameter for the form.
    /// </summary>
    [CascadingParameter]
    private ITnTForm? _tntForm { get; set; }

    private ProgressFileDetails ProgressFileDetails { get; set; }

    private string ProgressStyle => ProgressTemplate == null ? $"visibility: {(ProgressPercent > 0 ? "visible" : "hidden")};" : string.Empty;

    private IJSObjectReference? _containerInstance;

    private ElementReference _labelElement;

    private TnTInputFile() => DotNetObjectRef = DotNetObjectReference.Create(this);

    /// <inheritdoc />
    public void Dispose() {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync() {
        await DisposeAsyncCore().ConfigureAwait(false);
        Dispose(disposing: false);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    ///     Open the dialog-box to select files. ⚠️ This method doesn't work on Safari and iOS.
    /// </summary>
    /// <returns></returns>
    public async Task ShowFilesDialogAsync() {
        IsolatedJsModule ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", JsModulePath);

        await IsolatedJsModule.InvokeVoidAsync("raiseFluentInputFile", ElementId);
    }

    /// <inheritdoc />
    protected virtual void Dispose(bool disposing) {
        if (disposing) {
            DotNetObjectRef?.Dispose();
            DotNetObjectRef = null;
            // Do not dispose IsolatedJsModule here; it should be disposed asynchronously in DisposeAsyncCore.
        }
    }

    /// <inheritdoc />
    protected virtual async ValueTask DisposeAsyncCore() {
        if (IsolatedJsModule is not null) {
            try {
                if (_containerInstance is not null) {
                    await _containerInstance.InvokeVoidAsync("dispose");
                    await _containerInstance.DisposeAsync().ConfigureAwait(false);
                }
                await IsolatedJsModule.InvokeVoidAsync("onDispose", Element, DotNetObjectRef);
                await IsolatedJsModule.DisposeAsync().ConfigureAwait(false);
            }
            catch (JSDisconnectedException) {
                // JS runtime was disconnected, safe to ignore during disposal.
            }
            IsolatedJsModule = null;
            _containerInstance = null;
        }
    }

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender) {
        await base.OnAfterRenderAsync(firstRender);
        try {
            if (firstRender) {
                IsolatedJsModule ??= await JSRuntime.ImportIsolatedJs(this, JsModulePath);
                await IsolatedJsModule.InvokeVoidAsync("onLoad", Element, DotNetObjectRef);

                _containerInstance = await IsolatedJsModule.InvokeAsync<IJSObjectReference>("initializeFileDropZone", _labelElement, Element);
            }

            await (IsolatedJsModule?.InvokeVoidAsync("onUpdate", Element, DotNetObjectRef) ?? ValueTask.CompletedTask);
        }
        catch (JSDisconnectedException) {
            // JS runtime was disconnected, safe to ignore during render.
        }
    }

    /// <inheritdoc />
    protected override void OnInitialized() {
        base.OnInitialized();
        OnChange = EventCallback.Factory.Create(this, async (InputFileChangeEventArgs args) => await OnUploadFilesHandlerAsync(args));
    }

    /// <inheritdoc />
    protected override void OnParametersSet() {
        base.OnParametersSet();

        var dict = AdditionalAttributes?.ToDictionary() ?? new Dictionary<string, object>();
        dict.TryAdd("disabled", FieldDisabled);
        dict.TryAdd("readonly", FieldReadonly);
        AdditionalAttributes = dict;
    }

    private async Task OnUploadFilesHandlerAsync(InputFileChangeEventArgs e) {
        if (e.FileCount > MaximumFileCount) {
            if (OnFileCountExceeded.HasDelegate) {
                await OnFileCountExceeded.InvokeAsync(e.FileCount);
            }
            return;
        }

        // Use the native Blazor event
        if (OnInputFileChange.HasDelegate) {
            await OnInputFileChange.InvokeAsync(e);
            return;
        }

        // Start
        await UpdateProgressAsync(0, ResourceLoadingBefore);

        List<FluentInputFileEventArgs>? uploadedFiles = [];
        IReadOnlyList<IBrowserFile>? allFiles = e.GetMultipleFiles(MaximumFileCount);
        var allFilesSummary = allFiles.Select(i => new UploadedFileDetails(i.Name, i.Size, i.ContentType)).ToList();
        var totalFileSizes = allFiles.Sum(i => i.Size);
        var totalRead = 0L;
        var fileNumber = 0;

        foreach (IBrowserFile file in allFiles) {
            ProgressFileDetails = new ProgressFileDetails(fileNumber, file.Name, 0);
            // Keep a trace of this file
            FluentInputFileEventArgs? fileDetails = new() {
                AllFiles = allFilesSummary,
                Index = fileNumber,
                Name = file.Name,
                ContentType = file.ContentType,
                Size = file.Size,
                LastModified = file.LastModified,
                IsCancelled = false,
            };
            uploadedFiles.Add(fileDetails);

            // Max size => ERROR
            if (file.Size > MaximumFileSize) {
                fileDetails.ErrorMessage = "The maximum size allowed is reached";
                continue;
            }

            // Progress
            var title = string.Format(ResourceLoadingInProgress, fileNumber + 1, allFiles.Count, file.Name) ?? string.Empty;
            fileDetails.ProgressTitle = title;

            switch (Mode) {
                case InputFileMode.Buffer:

                    // Read all buffers and raise the event OnProgressChange
                    await ReadFileToBufferAndRaiseProgressEventAsync(file, fileDetails, (buffer, bytesRead) => {
                        totalRead += bytesRead;

                        fileDetails.Buffer = new FluentInputFileBuffer(buffer, bytesRead);

                        return UpdateProgressAsync(totalRead, totalFileSizes, title);
                    });

                    break;

                case InputFileMode.SaveToTemporaryFolder:

                    // Save to temporary file
                    var tempFileName = Path.GetTempFileName();
                    fileDetails.LocalFile = new FileInfo(tempFileName);

                    // Create a local file and write all read buffers
                    await using (FileStream writeStream = new(tempFileName, FileMode.Create)) {
                        await ReadFileToBufferAndRaiseProgressEventAsync(file, fileDetails, async (buffer, bytesRead) => {
                            totalRead += bytesRead;

                            await writeStream.WriteAsync(buffer.AsMemory(0, bytesRead));

                            await UpdateProgressAsync(totalRead, totalFileSizes, title);
                        });
                    }

                    break;

                case InputFileMode.Stream:

                    var fileSizePart1 = file.Size / 2;
                    var fileSizePart2 = file.Size - fileSizePart1;

                    // Get a reference to the current file Stream
                    fileDetails.Stream = file.OpenReadStream(MaximumFileSize);

                    // Progression percent (first 50%)
                    totalRead += fileSizePart1;
                    await UpdateProgressAsync(totalRead, totalFileSizes, title);

                    // Uploaded event
                    if (OnFileUploaded.HasDelegate) {
                        fileDetails.ProgressPercent = ProgressPercent;
                        await OnFileUploaded.InvokeAsync(fileDetails);
                    }

                    // Progression percent (last 50%)
                    totalRead += fileSizePart2;
                    await UpdateProgressAsync(totalRead, totalFileSizes, title);

                    break;

                default:
                    throw new ArgumentException("Invalid Mode value.");
            }

            if (fileDetails.IsCancelled) {
                break;
            }

            fileNumber++;
        }

        // Canceled or Completed
        if (uploadedFiles.Any(i => i.IsCancelled)) {
            await UpdateProgressAsync(100, ResourceLoadingCanceled);
        }
        else {
            await UpdateProgressAsync(100, ResourceLoadingCompleted);
        }

        if (OnCompleted.HasDelegate) {
            await OnCompleted.InvokeAsync(uploadedFiles.ToArray());
        }
    }

    private async Task ReadFileToBufferAndRaiseProgressEventAsync(IBrowserFile file, FluentInputFileEventArgs fileDetails, Func<byte[], int, Task> action) {
        using Stream readStream = file.OpenReadStream(MaximumFileSize);
        var bytesRead = 0;
        var buffer = new byte[BufferSize];

        // Read file
        while ((bytesRead = await readStream.ReadAsync(buffer)) != 0) {
            await action(buffer, bytesRead);

            if (ProgressPercent <= 0) {
                ProgressPercent = 1;
            }

            if (OnProgressChange.HasDelegate) {
                fileDetails.ProgressPercent = ProgressPercent;
                await OnProgressChange.InvokeAsync(fileDetails);

                if (fileDetails.IsCancelled) {
                    break;
                }
            }

            StateHasChanged();
        }

        // Uploaded event
        if (OnFileUploaded.HasDelegate) {
            fileDetails.ProgressPercent = ProgressPercent;
            await OnFileUploaded.InvokeAsync(fileDetails);
        }
    }

    private Task UpdateProgressAsync(long current, long size, string title) {
        return UpdateProgressAsync(Convert.ToInt32(decimal.Divide(current, size <= 0 ? 1 : size) * 100), title);
    }

    private async Task UpdateProgressAsync(int percent, string title) {
        if (ProgressPercent != percent) {
            ProgressPercent = percent;

            if (ProgressPercentChanged.HasDelegate) {
                await ProgressPercentChanged.InvokeAsync(percent);
            }
        }

        if (ProgressTitle != title) {
            ProgressTitle = title;
        }
    }
}

/// <summary>
///     a
/// </summary>
/// <param name="Index">     </param>
/// <param name="Name">      </param>
/// <param name="Percentage"></param>
public record struct ProgressFileDetails(int Index, string Name, int Percentage);
/// <summary>
///     A
/// </summary>
/// <param name="Name">       </param>
/// <param name="Size">       </param>
/// <param name="ContentType"></param>
public record struct UploadedFileDetails(string Name, long Size, string ContentType);