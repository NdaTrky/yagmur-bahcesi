$ErrorActionPreference='Stop'
$env:ANDROID_HOME=Join-Path $env:USERPROFILE 'UnityBuilds\AndroidSdk'
$env:ANDROID_AVD_HOME=Join-Path $env:USERPROFILE 'UnityBuilds\Avd'
$adb=Join-Path $env:ANDROID_HOME 'platform-tools\adb.exe'
$active=& $adb devices
if ($active -match 'emulator-5554\s+device') { throw 'The test emulator is already running. Close it with adb -s emulator-5554 emu kill before opening a visible preview.' }
# The visible window is the interactive Android preview for the user.
Start-Process -FilePath (Join-Path $env:ANDROID_HOME 'emulator\emulator.exe') -ArgumentList @('-avd','RainGarden_API36','-no-snapshot','-gpu','swiftshader','-memory','3072','-cores','4','-camera-back','none','-camera-front','none')
