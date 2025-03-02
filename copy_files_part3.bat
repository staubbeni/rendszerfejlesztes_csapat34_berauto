@echo off
echo Fájlok másolása a harmadik részbe...

copy BerAuto.csproj BerAuto_Part3\BerAuto_Part3.csproj
copy appsettings.json BerAuto_Part3\
copy appsettings.Development.json BerAuto_Part3\
copy Program.cs BerAuto_Part3\

copy Models\*.* BerAuto_Part3\Models\
copy Controllers\*.* BerAuto_Part3\Controllers\
copy Data\*.* BerAuto_Part3\Data\

mkdir BerAuto_Part3\Views\Home
mkdir BerAuto_Part3\Views\Account
mkdir BerAuto_Part3\Views\Shared
mkdir BerAuto_Part3\Views\User
mkdir BerAuto_Part3\Views\Admin
mkdir BerAuto_Part3\Views\Assistant

copy Views\Home\*.* BerAuto_Part3\Views\Home\
copy Views\Account\*.* BerAuto_Part3\Views\Account\
copy Views\Shared\*.* BerAuto_Part3\Views\Shared\
copy Views\User\*.* BerAuto_Part3\Views\User\
copy Views\Admin\*.* BerAuto_Part3\Views\Admin\
copy Views\Assistant\*.* BerAuto_Part3\Views\Assistant\
copy Views\_ViewImports.cshtml BerAuto_Part3\Views\
copy Views\_ViewStart.cshtml BerAuto_Part3\Views\

echo Kész! 