param([string]$Apk)
$ErrorActionPreference='Stop'
$workspace=Split-Path -Parent $PSScriptRoot
if (-not $Apk) { $Apk=Join-Path $workspace 'Builds\Android\YagmurBahcesi-test.apk' }
$android='C:\Program Files\Unity\Hub\Editor\6000.3.17f1\Editor\Data\PlaybackEngines\AndroidPlayer'
$buildTools=Join-Path $android 'SDK\build-tools\36.0.0'
$readElf=Join-Path $android 'NDK\toolchains\llvm\prebuilt\windows-x86_64\bin\llvm-readelf.exe'
$out=Join-Path $workspace 'Artifacts\Android-Package'
$native=Join-Path $workspace 'Logs\NativeAudit'
New-Item -ItemType Directory -Path $out,$native -Force | Out-Null
$badging=& (Join-Path $buildTools 'aapt2.exe') dump badging $Apk
if ($LASTEXITCODE -ne 0) { throw 'APK manifest inspection failed.' }
$badging | Set-Content -LiteralPath (Join-Path $out 'manifest.txt') -Encoding UTF8
if (-not ($badging -match "minSdkVersion:'26'") -or -not ($badging -match "targetSdkVersion:'36'") -or -not ($badging -match "native-code: 'arm64-v8a'")) { throw 'Unexpected API level or ABI in the APK.' }
if ($badging -match 'application-debuggable') { throw 'Phone preview APK must have development mode disabled.' }
$permissions=@($badging | Select-String '^uses-permission:' | ForEach-Object {$_.Line})
$forbidden=$permissions | Select-String 'android.permission.(INTERNET|CAMERA|RECORD_AUDIO|READ_EXTERNAL_STORAGE|WRITE_EXTERNAL_STORAGE|ACCESS_FINE_LOCATION|AD_ID)'
if ($forbidden) { throw ('Unexpected permission: '+$forbidden) }
& (Join-Path $buildTools 'zipalign.exe') -c -P 16 -v 4 $Apk | Set-Content -LiteralPath (Join-Path $out 'zipalign.txt') -Encoding UTF8
if ($LASTEXITCODE -ne 0) { throw '16 KB APK zip alignment check failed.' }
Add-Type -AssemblyName System.IO.Compression.FileSystem
$zip=[IO.Compression.ZipFile]::OpenRead($Apk)
$elfs=New-Object System.Collections.Generic.List[object]
try {
    foreach ($entry in $zip.Entries) {
        if ($entry.FullName -notmatch '^lib/arm64-v8a/[^/]+\.so$') { continue }
        $destination=Join-Path $native ([IO.Path]::GetFileName($entry.FullName))
        [IO.Compression.ZipFileExtensions]::ExtractToFile($entry,$destination,$true)
        $headers=& $readElf -lW $destination
        $headers | Set-Content -LiteralPath (Join-Path $out ($entry.Name+'.txt')) -Encoding UTF8
        $alignments=@($headers | Where-Object {$_ -match '^\s*LOAD\s'} | ForEach-Object {[Convert]::ToInt64(($_.Trim() -split '\s+')[-1],16)})
        $pass=$alignments.Count -gt 0 -and -not ($alignments | Where-Object {$_ -lt 16384})
        $elfs.Add([pscustomobject]@{library=$entry.FullName;loadAlignments=$alignments;passed=$pass})
    }
} finally { $zip.Dispose() }
if ($elfs.Count -eq 0 -or $elfs.passed -contains $false) { throw 'Native ELF 16 KB segment alignment failed.' }
$report=[pscustomobject]@{date=(Get-Date -Format o);apk=[IO.Path]::GetFileName($Apk);sha256=(Get-FileHash -LiteralPath $Apk -Algorithm SHA256).Hash;bytes=(Get-Item -LiteralPath $Apk).Length;minApi=26;targetApi=36;abi='arm64-v8a';developmentBuild=$false;signing='Local Android debug key; not a Play upload key';permissions=$permissions;zipAlignment16KB='passed';elfAlignment16KB=$elfs;runtime16KB='not tested';status='passed'}
$report | ConvertTo-Json -Depth 5 | Set-Content -LiteralPath (Join-Path $out 'package-tests.json') -Encoding UTF8
$report | ConvertTo-Json -Depth 5
