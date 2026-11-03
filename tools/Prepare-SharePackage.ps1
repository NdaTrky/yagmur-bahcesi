param()
$ErrorActionPreference = 'Stop'
$workspace = Split-Path -Parent $PSScriptRoot
$windowsBuild = Join-Path $workspace 'Builds\Windows'
$androidApk = Join-Path $workspace 'Builds\Android\YagmurBahcesi-test.apk'
$required = @('YagmurBahcesi.exe','UnityPlayer.dll','YagmurBahcesi_Data','MonoBleedingEdge')
foreach ($name in $required) {
    if (-not (Test-Path -LiteralPath (Join-Path $windowsBuild $name))) {
        throw ('Missing Windows build file: '+$name+'. Run BuildWindows first.')
    }
}
if (-not (Test-Path -LiteralPath $androidApk)) { throw 'BuildAndroid must finish first.' }
# Package the existing binaries; this command does not compile or publish.
$runName = (Get-Date -Format 'yyyyMMdd-HHmmss')+'-'+[Guid]::NewGuid().ToString('N').Substring(0,6)
$out = Join-Path $workspace ('Builds\Paylasim\'+$runName)
New-Item -ItemType Directory -Path $out | Out-Null
$zipPath = Join-Path $out 'YagmurBahcesi-Windows-test.zip'
Add-Type -AssemblyName System.IO.Compression
Add-Type -AssemblyName System.IO.Compression.FileSystem
$sourceFiles = @(Get-ChildItem -LiteralPath $windowsBuild -File -Recurse)
$zip = [IO.Compression.ZipFile]::Open($zipPath,[IO.Compression.ZipArchiveMode]::Create)
try {
    foreach ($file in $sourceFiles) {
        $relative = $file.FullName.Substring($windowsBuild.Length+1).Replace('\','/')
        [IO.Compression.ZipFileExtensions]::CreateEntryFromFile($zip,$file.FullName,$relative,[IO.Compression.CompressionLevel]::Optimal) | Out-Null
    }
    $launcher = "@echo off"+[Environment]::NewLine+'start "" "%~dp0YagmurBahcesi.exe" -force-d3d11 -screen-fullscreen 0 -screen-width 1600 -screen-height 900'+[Environment]::NewLine
    $instructions = @'
Yağmur Bahçesi — Windows test sürümü

1. ZIP dosyasını bir klasöre tamamen çıkarın.
2. Çıkardığınız klasörde OYUNU_AC.cmd dosyasına çift tıklayın.
3. Unity/Unity Hub kurulumu gerekmez. EXE ve yanındaki klasörleri birlikte tutun.

Bu bir test sürümüdür. Bu bilgisayardaki önceki testlerde oyun kapanırken
UnityPlayer.dll hatası görülmüştür; bütün Windows cihazlarında doğrulanmamıştır.
İlerleme her oyuncunun kendi bilgisayarında saklanır. Çok oyunculu oyun yoktur.

Deneme: Bahçeye gir > bir çiçek büyüt > üç mini oyunu dene > pul kazan >
Bahçem'den çan çiçeğini aç/dik > yeni anı kaydet > kapatıp yeniden aç.
Sorun bildirirken cihaz, işlem sırası ve ekran görüntüsünü belirtin.
'@
    foreach ($item in @(@('OYUNU_AC.cmd',$launcher),@('ONCE_OKU.txt',$instructions))) {
        $entry = $zip.CreateEntry($item[0])
        $writer = New-Object IO.StreamWriter($entry.Open(),(New-Object Text.UTF8Encoding($false)))
        try { $writer.Write($item[1]) } finally { $writer.Dispose() }
    }
} finally { $zip.Dispose() }
$copiedApk = Join-Path $out 'YagmurBahcesi-Android-test.apk'
Copy-Item -LiteralPath $androidApk -Destination $copiedApk
$zip = [IO.Compression.ZipFile]::OpenRead($zipPath)
try {
    foreach ($file in $sourceFiles) {
        $relative = $file.FullName.Substring($windowsBuild.Length+1).Replace('\','/')
        $entry = $zip.GetEntry($relative)
        if ($null -eq $entry -or $entry.Length -ne $file.Length) { throw ('ZIP entry mismatch: '+$relative) }
    }
    if ($null -eq $zip.GetEntry('OYUNU_AC.cmd') -or $null -eq $zip.GetEntry('ONCE_OKU.txt')) { throw 'ZIP launch instructions missing.' }
} finally { $zip.Dispose() }
if ((Get-FileHash -LiteralPath $androidApk).Hash -ne (Get-FileHash -LiteralPath $copiedApk).Hash) { throw 'APK copy hash mismatch.' }
$manifest = [pscustomobject]@{
    created = (Get-Date -Format o)
    purpose = 'Existing test binaries packaged locally; no build, upload or publication performed.'
    windowsSource = $windowsBuild
    windowsSourceFiles = $sourceFiles.Count
    files = @($zipPath,$copiedApk | ForEach-Object {
        [pscustomobject]@{name=[IO.Path]::GetFileName($_);bytes=(Get-Item -LiteralPath $_).Length;sha256=(Get-FileHash -LiteralPath $_).Hash}
    })
}
$manifest | ConvertTo-Json -Depth 4 | Set-Content -LiteralPath (Join-Path $out 'paket-bilgisi.json') -Encoding UTF8
Write-Output ('Paylasim klasoru: '+$out)
$manifest.files | Format-Table name,bytes,sha256 -AutoSize
