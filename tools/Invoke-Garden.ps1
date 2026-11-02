param(
    [ValidateSet('Check','BuildWindows','BuildAndroid','BuildEmulator','Smoke','Open')]
    [string]$Action = 'Check',
    [int]$Width = 1600,
    [int]$Height = 900
)
$ErrorActionPreference = 'Stop'
$workspace = Split-Path -Parent $PSScriptRoot
$project = Join-Path $workspace 'YagmurBahcesi'
$editor = 'C:\Program Files\Unity\Hub\Editor\6000.3.17f1\Editor\Unity.exe'
$logs = Join-Path $workspace 'Logs'
New-Item -ItemType Directory -Path $logs -Force | Out-Null
if ($Action -eq 'Smoke') {
    $player = Join-Path $workspace 'Builds\Windows\YagmurBahcesi.exe'
    if (-not (Test-Path -LiteralPath $player)) { throw 'BuildWindows must finish first.' }
    $output = Join-Path $workspace ('Artifacts\Smoke-'+$Width+'x'+$Height)
    New-Item -ItemType Directory -Path $output -Force | Out-Null
    $arguments = @('--garden-smoke','--garden-output',('"'+$output+'"'),'-force-d3d11','-screen-fullscreen','0','-screen-width',$Width,'-screen-height',$Height,'-logFile',('"'+(Join-Path $output 'player.log')+'"'))
    # An interactive render window is required for end-of-frame capture.
    $process = Start-Process -FilePath $player -ArgumentList $arguments -PassThru
    if (-not $process.WaitForExit(60000)) { $process.Kill(); throw 'Runtime preview timed out after 60 seconds.' }
    if ($process.ExitCode -ne 0) { throw ('Runtime tests failed: '+$process.ExitCode) }
    Get-Content -LiteralPath (Join-Path $output 'runtime-tests.json') -Encoding UTF8
    exit 0
}
if ($Action -eq 'Open') {
    Start-Process -FilePath $editor -ArgumentList @('-projectPath',('"'+$project+'"'))
    exit 0
}
$method = switch ($Action) {
    'Check' { 'GardenChecks.Run' }
    'BuildWindows' { 'GardenBuilder.BuildWindows' }
    'BuildAndroid' { 'GardenBuilder.BuildAndroid' }
    'BuildEmulator' { 'GardenBuilder.BuildEmulator' }
}
if ($Action -in @('BuildAndroid','BuildEmulator')) {
    # Android IL2CPP rejects non-ASCII paths. Preserve the original project;
    # synchronize a build-only copy under the user's ASCII staging directory.
    $stage = Join-Path $env:USERPROFILE 'UnityBuilds\RainGardenAndroid'
    foreach ($folder in @('Assets','Packages','ProjectSettings')) {
        & robocopy (Join-Path $project $folder) (Join-Path $stage $folder) /E /NFL /NDL /NJH /NJS /NP | Out-Null
        if ($LASTEXITCODE -ge 8) { throw ('Staging copy failed: '+$folder) }
    }
    $project = $stage
}
$arguments = @('-batchmode','-nographics','-quit','-projectPath',('"'+$project+'"'),'-executeMethod',$method,'-logFile',('"'+(Join-Path $logs ($Action+'.log'))+'"'))
$process = Start-Process -FilePath $editor -ArgumentList $arguments -WindowStyle Hidden -PassThru
# Wait for Unity itself; Android's persistent adb daemon must not keep this
# script waiting after a successful build.
if (-not $process.WaitForExit(1200000)) { $process.Kill(); throw 'Unity build exceeded 20 minutes.' }
if ($process.ExitCode -ne 0) { throw ('Unity failed; inspect Logs/'+$Action+'.log') }
if ($Action -in @('BuildAndroid','BuildEmulator')) {
    $name = if ($Action -eq 'BuildAndroid') { 'YagmurBahcesi-test.apk' } else { 'YagmurBahcesi-emulator.apk' }
    $built = Join-Path (Split-Path -Parent $stage) ('Builds\Android\'+$name)
    $destination = Join-Path $workspace 'Builds\Android'
    New-Item -ItemType Directory -Path $destination -Force | Out-Null
    Copy-Item -LiteralPath $built -Destination $destination -Force
}
Write-Output ($Action+' finished successfully.')
