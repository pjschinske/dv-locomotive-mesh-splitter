param (
    [switch]$NoArchive,
    [switch]$IncludeBuildNum,
    [string]$OutputDirectory = $PSScriptRoot
)

Set-Location "$PSScriptRoot"
$FilesToInclude = "Info.json","build/*","LICENSE"

$modInfo = Get-Content -Raw -Path "Info.json" | ConvertFrom-Json
$modId = $modInfo.Id
$modVersion = $modInfo.Version
if ($IncludeBuildNum) {
    echo "Including build number"
    $indexOfPlus = $modVersion.IndexOf("+")
    if ($indexOfPlus -eq -1) {
        #if no build number exists, we start from 1
        $modVersion = $modVersion + "+1"
    } else {
        #if there is an existing build number, we increment it
        $modBuildNum = $modVersion.SubString($indexOfPlus + 1)
        $modVersion = $modVersion.SubString(0, $indexOfPlus + 1) + ([int]$modBuildNum + 1)
    }
} else {
    #if we're making a release build, remove the build number
    $indexOfPlus = $modVersion.IndexOf("+")
    if ($indexOfPlus -ne -1) {
        $modVersion = $modVersion.SubString(0, $indexOfPlus)
    }
}
echo "version: " + $modVersion
$modInfo.Version = $modVersion
$modInfo | ConvertTo-Json | Out-File "Info.json"

$DistDir = "$OutputDirectory/dist"
if ($NoArchive) {
    $ZipWorkDir = "$OutputDirectory"
} else {
    $ZipWorkDir = "$DistDir/tmp"
}
$ZipOutDir = "$ZipWorkDir/$modId"

New-Item "$ZipOutDir" -ItemType Directory -Force
Copy-Item -Force -Recurse -Path $FilesToInclude -Destination "$ZipOutDir"

if (!$NoArchive)
{
    $FILE_NAME = "$DistDir/${modId}_v$modVersion.zip"
    Compress-Archive -Update -CompressionLevel Fastest -Path "$ZipOutDir/*" -DestinationPath "$FILE_NAME"
}
