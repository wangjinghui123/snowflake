echo off

set num=0

:x
set /a num+=1

echo 第%num%次 
set d=%date:~,10%
set t=%time:~,8%
set t=%t: =0%
echo 开始时间：%d% %t%
JM_2.exe token=WkE8fSqkq3Zm1Mfruj61uK0zj0OaScOF0znl id0=4000 id1=6000 time=75 startat=%d%/%t%.000 timeparam=60
set d=%date:~,10%
set t=%time:~,8%
echo 结束时间：%d% %t%

choice /t 150 /d y /n >nul 

rem timeout /t 150 /NOBREAK
rem if %num% leq 1 (goto:x) 
goto:x

pause