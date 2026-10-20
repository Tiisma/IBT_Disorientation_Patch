[CmdletBinding()]
param(
    [string]$GamePath = $env:VINTAGE_STORY,
    [ValidateSet("Debug", "Release")]
    [string]$Configuration = "Release"
)

$ErrorActionPreference = "Stop"

if ([string]::IsNullOrWhiteSpace($GamePath)) {
    Write-Error "Provide the Vintage Story installation path with -GamePath or set the VINTAGE_STORY environment variable."
    exit 1
}

if (-not (Test-Path -LiteralPath $GamePath -PathType Container)) {
    Write-Error "Vintage Story installation directory not found: $GamePath"
    exit 1
}

$projectPath = Join-Path $PSScriptRoot "IBT_Disorientation_Patch.csproj"
$distributionPath = Join-Path $PSScriptRoot "dist"
$buildArguments = @(
    "build",
    $projectPath,
    "--configuration",
    $Configuration,
    "-p:GamePath=$GamePath"
)

& dotnet @buildArguments
if ($LASTEXITCODE -ne 0) {
    exit $LASTEXITCODE
}

$archive = Get-ChildItem -LiteralPath $distributionPath -Filter "ibm_disorientation_patch_*.zip" -File |
    Sort-Object LastWriteTime -Descending |
    Select-Object -First 1

if ($null -eq $archive) {
    Write-Error "The build succeeded, but no mod archive was found in $distributionPath."
    exit 1
}

Write-Host "Build and packaging complete: $($archive.FullName)"
