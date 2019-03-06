@echo off
setlocal
setlocal enabledelayedexpansion

for /f "tokens=2 delims==" %%f in ('wmic service where "name='Amcom.SDC IntelliSpeech Service'" get PathName/value') do set "myVar=%%f"

Set filename= %myVar%

For %%A in (%filename%) do (
  Set Folder=%%~dpA
  Set Name=%%~nxA
)

if not exist "%Folder%"  Goto Error2
if not exist "%Folder%" set  Folder = "D:\Program Files\Amcom Software\Amcom.SDC IntelliSpeech Service\Report Files"
if not exist "%Folder%\Report Files" Goto ERROR3
if not exist "*.rdlc" Goto ERROR4

xcopy "*.rdlc" "%Folder%\Report Files" /Y
if ERRORLEVEL 0 Goto :Sucess 
if ERRORLEVEL 1 Goto :Failure

:Sucess
msg *  Reports are updated sucessfully at "%Folder%Report Files".
EXIT
:Failure
msg * Updation of reports failed.

:ERROR1
msg * Speech service is not installed.
EXIT
:ERROR2
msg *  "%Folder%Report Files" path does not exists.
EXIT
:ERROR3
msg * Current folder doesn't contain updated report files. 

