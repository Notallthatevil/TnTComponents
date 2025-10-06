param(
    [string[]]$runtimes,
    [string[]]$frameworks
)

$rootDirectory = $PSScriptRoot

# Determine target frameworks: prefer explicit parameter, then TARGET_FRAMEWORK env var, then sensible defaults
if (-not $frameworks -or $frameworks.Length -eq 0) {
    if ($env:TARGET_FRAMEWORK) {
        # Allow comma- or semicolon-separated env var values
        $frameworks = $env:TARGET_FRAMEWORK -split '[,;]' | ForEach-Object { $_.Trim() } | Where-Object { $_ -ne '' }
    }
}

$targetFrameworks = @('net9.0', 'net10.0')
if ($frameworks -and $frameworks.Length -gt 0) {
    $targetFrameworks = $frameworks
}

if (-not $runtimes -or $runtimes.Length -eq 0) {
    # sensible default set of common RIDs to test; adjust if you need a different matrix
    $runtimes = @(
        'win-x64',
        'win-x86',
        'win-arm64',
        'linux-x64',
        'linux-arm',
        'linux-arm64',
        'osx-x64',
        'osx-arm64'
    )
}

$projectPath = Join-Path $rootDirectory 'AotCompatibility.TestApp/AotCompatibility.TestApp.csproj'
$actualWarningCount = 0
$failedRuns = 0

foreach ($framework in $targetFrameworks) {
    foreach ($rid in $runtimes) {
        Write-Host "--- Publishing for framework: $framework, runtime: $rid ---"

        # publish and capture output
        $publishArgs = @(
            $projectPath,
            '-nodeReuse:false',
            '/p:UseSharedCompilation=false',
            '/p:ExposeExperimentalFeatures=true',
            '-c', 'Release',
            '-f', $framework,
            '-r', $rid,
            '--self-contained', 'true'
        )

        # Invoke dotnet publish and capture both stdout and stderr
        $publishOutput = & dotnet publish @publishArgs 2>&1 | Out-String
        Write-Host $publishOutput

        foreach ($line in ($publishOutput -split "`r`n")) {
            if ($line -like "*analysis warning IL*") {
                Write-Host $line
                $actualWarningCount += 1
            }
        }

        # determine publish folder and executable name
        $publishFolder = Join-Path $rootDirectory "AotCompatibility.TestApp/bin/Release/$framework/$rid/publish"
        if (-not (Test-Path $publishFolder)) {
            Write-Host "Publish folder not found: $publishFolder"
            $failedRuns += 1
            continue
        }

        pushd $publishFolder | Out-Null
        try {
            Write-Host "Executing published test App for framework $framework, runtime $rid..."
            if ($rid -like 'win*') {
                # Windows executables
                .\AotCompatibility.TestApp.exe
            }
            else {
                # Unix-like
                ./AotCompatibility.TestApp
            }

            if ($LastExitCode -ne 0) {
                Write-Host "There was an error while executing AotCompatibility Test App for framework $framework, runtime $rid. LastExitCode is: $LastExitCode"
                $failedRuns += 1
            }
            else {
                Write-Host "Execution finished successfully for framework $framework, runtime $rid"
            }
        }
        catch {
            Write-Host "Exception while running published app for framework ${framework}, runtime ${rid}: $_"
            $failedRuns += 1
        }
        finally {
            popd | Out-Null
        }
    }
}

Write-Host "Actual analysis warning count is: $actualWarningCount"
Write-Host "Failed run count is: $failedRuns"

$expectedWarningCount = 0

if ($actualWarningCount -ne $expectedWarningCount -or $failedRuns -ne 0) {
    Write-Host "AOT compatibility test FAILED. Warnings or failed runs detected."
    Exit 1
}

Write-Host "AOT compatibility test PASSED. No analysis warnings and all runs succeeded."
Exit 0