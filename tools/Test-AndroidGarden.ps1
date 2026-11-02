param([string]$Serial='emulator-5554')
$ErrorActionPreference='Stop'
$workspace=Split-Path -Parent $PSScriptRoot
$adb=Join-Path $env:USERPROFILE 'UnityBuilds\AndroidSdk\platform-tools\adb.exe'
$package='com.raingarden.yagmurbahcesi.dev'
$activity=$package+'/com.unity3d.player.UnityPlayerGameActivity'
$out=Join-Path $workspace 'Artifacts\Android-API36'
New-Item -ItemType Directory -Path $out -Force | Out-Null
$avd=(& $adb -s $Serial emu avd name | Select-Object -First 1).Trim()
if ($avd -ne 'RainGarden_API36') { throw 'This test may reset data only in the dedicated RainGarden_API36 emulator.' }
$checks=New-Object System.Collections.Generic.List[object]
function Check($name,$passed) { $checks.Add([pscustomobject]@{name=$name;passed=[bool]$passed}); if (-not $passed) { Write-Warning $name } }
function Tap($x,$y) { & $adb -s $Serial shell input tap $x $y | Out-Null }
function Drag($x,$y,$toX,$toY) { & $adb -s $Serial shell input swipe $x $y $toX $toY 650 | Out-Null }
function State {
    $raw=(& $adb -s $Serial shell 'if [ -f /sdcard/Android/data/com.raingarden.yagmurbahcesi.dev/files/garden-v1.json ]; then cat /sdcard/Android/data/com.raingarden.yagmurbahcesi.dev/files/garden-v1.json; fi') -join "`n"
    if ($raw) { return ($raw | ConvertFrom-Json) }
    return $null
}
function Capture($name) {
    & $adb -s $Serial shell screencap -p ('/sdcard/rain-'+$name+'.png') | Out-Null
    & $adb -s $Serial pull ('/sdcard/rain-'+$name+'.png') (Join-Path $out ($name+'.png')) | Out-Null
}
# This is an agent-created, isolated AVD. Save previous test progress before reset.
$previous=State
if ($null -ne $previous) { $previous | ConvertTo-Json -Depth 5 | Set-Content -LiteralPath (Join-Path $out 'previous-test-state.json') -Encoding UTF8 }
& $adb -s $Serial shell pm clear $package | Out-Null
& $adb -s $Serial logcat -c
& $adb -s $Serial shell svc wifi disable
& $adb -s $Serial shell svc data disable
& $adb -s $Serial shell am start -W -n $activity | Set-Content -LiteralPath (Join-Path $out 'launch.txt') -Encoding UTF8
Start-Sleep -Seconds 6
Capture 'home-en'
Tap 1590 65
Start-Sleep -Milliseconds 500
Capture 'home'
Tap 525 710
Start-Sleep -Seconds 1
$state=State
Check 'Cold launch and play button while Wi-Fi/mobile data disabled' ($null -ne $state -and $state.visited)
Check 'Home language button changes English device default to Turkish' ($state.language -eq 'tr')
Tap 380 460
Tap 380 460
Check 'Two Android taps grow the first flower' ((State).plants[0] -eq 2)
Drag 1370 640 1430 500
Check 'Leaf boat drag selects boat rather than squirrel' ((State).boatLaunched)
Drag 1200 820 930 810
Check 'Apple drag selects fruit in front of friend' ((State).picnic[0] -eq 1)
Drag 800 590 940 480
Check 'Decoration drag saves its new position' ([Math]::Abs((State).decorations[0].x - (-1.6)) -gt .5)
Capture 'garden'
$before=State
$before | ConvertTo-Json -Depth 5 | Set-Content -LiteralPath (Join-Path $out 'state-before-restart.json') -Encoding UTF8
& $adb -s $Serial shell input keyevent 3
Start-Sleep -Milliseconds 500
& $adb -s $Serial shell am start -W -n $activity | Out-Null
Start-Sleep -Seconds 1
Check 'Background and resume preserve progress' ((State).boatLaunched -and (State).plants[0] -eq 2)
& $adb -s $Serial shell am force-stop $package
& $adb -s $Serial shell am start -W -n $activity | Out-Null
Start-Sleep -Seconds 4
Tap 525 710
Start-Sleep -Seconds 1
$after=State
Check 'Process restart restores flowers, boat, picnic and language' ($after.plants[0] -eq 2 -and $after.boatLaunched -and $after.picnic[0] -eq 1 -and $after.language -eq 'tr')
Capture 'restored'
Tap 1460 65
Start-Sleep -Milliseconds 500
Capture 'parent-gate'
$beforeModal=(State | ConvertTo-Json -Depth 5 -Compress)
Tap 230 550
Check 'Parent modal blocks world input' ((State | ConvertTo-Json -Depth 5 -Compress) -eq $beforeModal)
& $adb -s $Serial shell input keyevent 4
Start-Sleep -Milliseconds 500
Tap 1820 65
Start-Sleep -Milliseconds 500
Tap 1725 65
Check 'Sound toggle persists' (-not (State).sound)
$log=& $adb -s $Serial logcat -d -s Unity AndroidRuntime
$log | Set-Content -LiteralPath (Join-Path $out 'logcat.txt') -Encoding UTF8
Check 'No Unity exception or Android fatal crash in test log' (-not ($log | Select-String 'FATAL EXCEPTION|NullReferenceException|MissingReferenceException|Unhandled Exception'))
$mem=& $adb -s $Serial shell dumpsys meminfo $package
$mem | Set-Content -LiteralPath (Join-Path $out 'memory.txt') -Encoding UTF8
$result=[pscustomobject]@{date=(Get-Date -Format o);device=$avd;api=36;abi='x86_64';pageSize=4096;status=if ($checks.passed -contains $false) {'failed'} else {'passed'};checks=$checks}
$result | ConvertTo-Json -Depth 5 | Set-Content -LiteralPath (Join-Path $out 'android-tests.json') -Encoding UTF8
$result | ConvertTo-Json -Depth 5
if ($result.status -ne 'passed') { throw 'Android checks failed; inspect Artifacts/Android-API36.' }
