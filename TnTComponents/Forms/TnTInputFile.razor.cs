using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using TnTComponents.Enum;

namespace TnTComponents.Forms;

public partial class TnTInputFile {

    /// <summary>
    /// Gets or sets the sze of buffer to read bytes from uploaded file (in bytes). Default value is
    /// 10 KB.
    /// </summary>
    [Parameter]
    public uint BufferSize { get; set; } = 10 * 1024;

    [Parameter]
    public string ContainerClass { get; set; } = TnTInputBase<int>.DefaultContainerClass;

    [Parameter]
    public string? CustomInputId { get; set; }

    [Parameter]
    public bool Disabled { get; set; }

    [Parameter]
    public string? EndIcon { get; set; }

    [Parameter]
    public IconType IconType { get; set; }

    [Parameter]
    public string? Label { get; set; }

    [Parameter]
    public string LabelClass { get; set; } = TnTInputBase<int>.DefaultLabelClass;

    [Parameter]
    public string LabelTextClass { get; set; } = TnTInputBase<int>.DefaultLabelTextClass;

    [Parameter]
    public int MaximumFileCount { get; set; } = 10;

    /// <summary>
    /// Gets or sets the maximum size of a file to be uploaded (in bytes). Default value is 10 MB.
    /// </summary>
    [Parameter]
    public long MaximumFileSize { get; set; } = 10 * 1024 * 1024;

    [Parameter]
    public InputFileMode Mode { get; set; }

    [Parameter]
    public bool Multiple { get; set; }

    [Parameter]
    public string? Name { get; set; }

    /// <summary>
    /// Raise when all files are completely uploaded.
    /// </summary>
    [Parameter]
    public EventCallback<IEnumerable<TnTInputFileEventArgs>> OnCompleted { get; set; }

    /// <summary>
    /// Raise when a file raised an error. Not yet used.
    /// </summary>
    [Parameter]
    public EventCallback<TnTInputFileEventArgs> OnFileError { get; set; }

    [Parameter]
    public EventCallback<TnTInputFileEventArgs> OnFileUploaded { get; set; }

    [Parameter]
    public EventCallback<InputFileChangeEventArgs> OnInputFileChange { get; set; }

    /// <summary>
    /// Raise when a progression step is updated. You can use <see cref="ProgressPercent" /> and
    /// <see cref="ProgressTitle" /> to have more detail on the progression.
    /// </summary>
    [Parameter]
    public EventCallback<TnTInputFileEventArgs> OnProgressChange { get; set; }

    [Parameter]
    public int ProgressPercent { get; set; } = 0;

    /// <summary>
    /// Gets or sets a callback that updates the <see cref="ProgressPercent" />.
    /// </summary>
    [Parameter]
    public EventCallback<int> ProgressPercentChanged { get; set; }

    /// <summary>
    /// Gets the current label display when an upload is in progress.
    /// </summary>
    public string ProgressTitle { get; private set; } = string.Empty;

    [Parameter]
    public bool ReadOnly { get; set; }

    [Parameter]
    public bool Required { get; set; }

    [Parameter]
    public string? StartIcon { get; set; }

    [Parameter]
    public string? SupportingText { get; set; }

    [Parameter]
    public string SupportingTextClass { get; set; } = TnTInputBase<int>.DefaultSupportingTextClass;

    [Parameter]
    public string Title { get; set; } = default!;

    private ProgressFileDetails _progressFileDetails { get; set; }
    private bool _dropOver = false;

    protected async Task OnUploadAsync(InputFileChangeEventArgs e) {
        if (e.FileCount > MaximumFileCount) {
            throw new ApplicationException($"The maximum number of files accepted is {MaximumFileCount}, but {e.FileCount} were supplied.");
        }

        _dropOver = false;

        // Use the native Blazor event
        if (OnInputFileChange.HasDelegate) {
            await OnInputFileChange.InvokeAsync(e);
            return;
        }

        // Start
        await UpdateProgressAsync(0, "Loading...");

        List<TnTInputFileEventArgs> uploadedFiles = [];
        var allFiles = e.GetMultipleFiles(MaximumFileCount);
        var allFilesSummary = allFiles.Select(i => new UploadedFileDetails(i.Name, i.Size, i.ContentType)).ToList();
        long totalFileSizes = allFiles.Sum(i => i.Size);
        long totalRead = 0L;
        int fileNumber = 0;

        foreach (IBrowserFile file in allFiles) {
            _progressFileDetails = new ProgressFileDetails(fileNumber, file.Name, 0);

            // Keep a trace of this file
            var fileDetails = new TnTInputFileEventArgs() {
                AllFiles = allFilesSummary,
                Index = fileNumber,
                Name = file.Name,
                ContentType = file.ContentType,
                Size = file.Size,
                IsCancelled = false,
            };
            uploadedFiles.Add(fileDetails);

            // Max size => ERROR
            if (file.Size > MaximumFileSize) {
                fileDetails.ErrorMessage = "The maximum size allowed is reached";
                continue;
            }

            // Progress
            var title = $"Loading {fileNumber + 1}/{allFiles.Count} - {file.Name}";
            fileDetails.ProgressTitle = title;

            switch (Mode) {
                case InputFileMode.Buffer:

                    // Read all buffers and raise the event OnProgressChange
                    await ReadFileToBufferAndRaiseProgressEventAsync(file, fileDetails, async (buffer, bytesRead) => {
                        totalRead += bytesRead;

                        fileDetails.Buffer = new TnTInputFileBuffer(buffer, bytesRead);

                        await UpdateProgressAsync(totalRead, totalFileSizes, title);
                    });

                    break;

                case InputFileMode.SaveToTemporaryFolder:

                    // Save to temporary file
                    string? tempFileName = Path.GetTempFileName();
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

                    long fileSizePart1 = file.Size / 2;
                    long fileSizePart2 = file.Size - fileSizePart1;

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
            await UpdateProgressAsync(100, "Canceled");
        }
        else {
            await UpdateProgressAsync(100, "Completed");
        }

        await OnCompleted.InvokeAsync(uploadedFiles.ToArray());
    }

    private async Task ReadFileToBufferAndRaiseProgressEventAsync(IBrowserFile file, TnTInputFileEventArgs fileDetails, Func<byte[], int, Task> action) {
        using Stream readStream = file.OpenReadStream(MaximumFileSize);
        int bytesRead = 0;
        byte[]? buffer = new byte[BufferSize];

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
        return UpdateProgressAsync(Convert.ToInt32(decimal.Divide(current, size) * 100), title);
    }

    private async Task UpdateProgressAsync(int percent, string title) {
        if (ProgressPercent != percent) {
            ProgressPercent = percent;

            await ProgressPercentChanged.InvokeAsync(percent);
        }

        if (ProgressTitle != title) {
            ProgressTitle = title;
        }
    }
}

public class TnTInputFileBuffer(byte[] data, int bytesRead) {

    /// <summary>
    /// Number of bytes read.
    /// </summary>
    public int BytesRead { get; } = bytesRead;

    /// <summary>
    /// Buffer data read.
    /// </summary>
    public byte[] Data { get; } = data;

    /// <summary>
    /// Append the current buffer (Data) to this file.
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    public async Task AppendToFileAsync(string file) {
        using var stream = new FileStream(file, FileMode.Append);
        await stream.WriteAsync(Data.AsMemory(0, BytesRead));
    }

    /// <summary>
    /// Append the current buffer (Data) to this file.
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    public Task AppendToFileAsync(FileInfo file) {
        return AppendToFileAsync(file.FullName);
    }
}

public class TnTInputFileEventArgs : EventArgs {

    /// <summary>
    /// Gets a list of all files currently in an upload process.
    /// </summary>
    public IEnumerable<UploadedFileDetails> AllFiles { get; internal set; } = default!;

    /// <summary>
    /// Gets a small buffer data of the current file in an upload process. Only if Mode = Buffer.
    /// </summary>
    public TnTInputFileBuffer Buffer { get; internal set; } = default!;

    /// <summary>
    /// Gets the content type of the current file in an upload process.
    /// </summary>
    public string ContentType { get; internal set; } = string.Empty;

    /// <summary>
    /// Gets the error message (or null if no error occurred).
    /// </summary>
    public string? ErrorMessage { get; internal set; }

    /// <summary>
    /// Gets the index of the current file in an upload process.
    /// </summary>
    public int Index { get; internal set; } = 0;

    /// <summary>
    /// Set this property to True to cancel the current upload file.
    /// </summary>
    public bool IsCancelled { get; set; } = false;

    /// <summary>
    /// Gets the local file of the current file in an upload process. Only if Mode =
    /// SaveToTemporaryFolder (otherwise, this value is null).
    /// </summary>
    public FileInfo? LocalFile { get; internal set; }

    /// <summary>
    /// Gets the name of the current file in an upload process.
    /// </summary>
    public string Name { get; internal set; } = string.Empty;

    /// <summary>
    /// Gets the global percent value in an upload process.
    /// </summary>
    public int ProgressPercent { get; internal set; } = 0;

    /// <summary>
    /// Gets the label to display in an upload process.
    /// </summary>
    public string ProgressTitle { get; internal set; } = string.Empty;

    /// <summary>
    /// Gets the size (in bytes) of the current file in an upload process.
    /// </summary>
    public long Size { get; internal set; } = 0;

    /// <summary>
    /// Gets a reference to the current stream in an upload process. Only if Mode = Stream
    /// (otherwise, this value is null). The OnProgressChange event will not be triggered.
    /// </summary>
    public Stream? Stream { get; internal set; }
}

public record struct ProgressFileDetails(int Index, string Name, int Percentage);

public record struct UploadedFileDetails(string Name, long Size, string ContentType);