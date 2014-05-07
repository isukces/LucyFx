@echo off



if "%NUGETEXE%" == "" goto a
if "%NUGETREPO%" == "" goto b

echo NUGETEXE = %NUGETEXE%
echo NUGETREPO = %NUGETREPO%

%NUGETEXE% pack Lucy.csproj  -IncludeReferencedProjects -Prop Configuration=RELEASE

rem copy *.nupkg %NUGETREPO%
rem del *.nupkg
goto end



:a
echo ERROR brak zmiennej NUGETEXE
goto end

:b
echo ERROR brak zmiennej NUGETREPO 
goto end


:end
pause
 