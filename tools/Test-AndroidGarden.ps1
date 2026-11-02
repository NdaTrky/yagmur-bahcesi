param([string]$Serial='emulator-5554')
$ErrorActionPreference='Stop'
$workspace=Split-Path -Parent $PSScriptRoot
$adb=Join-Path $env:USERPROFILE 'UnityBuilds\AndroidSdk\platform-tools\adb.exe'
$package='com.raingarden.yagmurbahcesi.dev'
$activity=$package+'/com.unity3d.player.UnityPlayerGameActivity'
$out=Join-Path $workspace 'Artifacts\Android-API36'
New-Item -ItemType Directory -Path $out -Force | Out-Null
& $adb start-server | Out-Null
$ready=$false
for($attempt=0;$attempt -lt 10;$attempt++) {
    if ((& $adb devices) -match ('^'+[Regex]::Escape($Serial)+'\s+device$')) { $ready=$true; break }
    Start-Sleep -Milliseconds 750
}
if(-not $ready) { throw 'The dedicated Android emulator is not ready.' }
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
# Expanded reward loop: Android touch input only, never edit the save to grant progress.
Tap 940 705
Start-Sleep -Milliseconds 400
Tap 810 675
Start-Sleep -Milliseconds 400
$partial=State
Check 'Picnic opens and accepts its first fruit' ($partial.activities[0].step -eq 1)
Capture 'picnic'
Tap 1205 895
& $adb -s $Serial shell am force-stop $package
& $adb -s $Serial shell am start -W -n $activity | Out-Null
Start-Sleep -Seconds 3
Tap 525 710
Start-Sleep -Milliseconds 600
Tap 940 705
Check 'Partial mini game survives process restart' ((State).activities[0].id -eq $partial.activities[0].id -and (State).activities[0].step -eq 1)
Tap 1110 675
Tap 810 675
Start-Sleep -Milliseconds 400
Check 'Picnic completion persists exactly one stamp' ((State).stamps -eq 1 -and (State).activities[0].claimed)
Tap 810 675
Check 'Extra reward screen tap does not duplicate stamp' ((State).stamps -eq 1)
Tap 720 855
Start-Sleep -Milliseconds 400
Capture 'collection'
Tap 1100 668
Start-Sleep -Milliseconds 400
Tap 660 710
Start-Sleep -Milliseconds 400
Check 'Earned stamp unlocks and plants bluebell' ((State).stamps -eq 0 -and (State).flowerIds[3] -eq 'bluebell' -and (State).unlocked -contains 'bluebell')
Tap 880 465
Check 'New flower grows in its garden plot' ((State).plants[3] -eq 2)
foreach($kind in @(1,2)) {
    if ($kind -eq 1) { Tap 220 280 } else { Tap 1640 350 }
    Start-Sleep -Milliseconds 350
    Capture ('activity-'+$kind)
    for($step=0;$step -lt 30;$step++) {
        $current=State
        if ($current.activities[$kind].claimed) { break }
        Tap 700 895
        Start-Sleep -Milliseconds 150
    }
    Check ('Android help completes activity '+$kind) ((State).activities[$kind].claimed)
    & $adb -s $Serial shell input keyevent 4
    Start-Sleep -Milliseconds 300
}
Check 'Three activity rewards and one purchase balance correctly' ((State).stamps -eq 2 -and (State).activityCompletions -eq 3)
Tap 270 980
Start-Sleep -Milliseconds 400
Capture 'discoveries'
& $adb -s $Serial shell input keyevent 4
Tap 1690 990
Start-Sleep -Milliseconds 400
Tap 960 875
Start-Sleep -Seconds 1
$album=& $adb -s $Serial shell 'ls /sdcard/Android/data/com.raingarden.yagmurbahcesi.dev/files/garden-album/*.png'
Check 'Local garden album writes without storage permission' ($album -match '\.png')
Capture 'expanded-garden'
& $adb -s $Serial shell am force-stop $package
& $adb -s $Serial shell am start -W -n $activity | Out-Null
Start-Sleep -Seconds 3
Tap 525 710
Start-Sleep -Milliseconds 600
$expanded=State
Check 'Expanded wallet, new flower and discoveries survive restart' ($expanded.stamps -eq 2 -and $expanded.flowerIds[3] -eq 'bluebell' -and $expanded.discoveries[11])
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
