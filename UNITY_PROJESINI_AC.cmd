@echo off
setlocal
set "GARDEN_EDITOR=%ProgramFiles%\Unity\Hub\Editor\6000.3.17f1\Editor\Unity.exe"
set "GARDEN_PROJECT=%~dp0YagmurBahcesi"
if not exist "%GARDEN_EDITOR%" (
    echo Unity 6000.3.17f1 bulunamadi. Unity Hub uzerinden bu surumu kurun.
    pause
    exit /b 1
)
if not exist "%GARDEN_PROJECT%\ProjectSettings\ProjectVersion.txt" (
    echo Unity proje klasoru bulunamadi: "%GARDEN_PROJECT%"
    pause
    exit /b 1
)
if not exist "%GARDEN_PROJECT%\Assets\RainGarden\Scenes\Garden.unity" (
    echo Garden sahnesi bulunamadi.
    pause
    exit /b 1
)
start "" "%GARDEN_EDITOR%" -openfile "%GARDEN_PROJECT%\Assets\RainGarden\Scenes\Garden.unity"
endlocal
