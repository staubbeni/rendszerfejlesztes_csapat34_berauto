@echo off
echo Fájlok másolása az első részbe...

copy Models\ErrorViewModel.cs BerAuto_Part1\Models\
copy Models\LoginViewModel.cs BerAuto_Part1\Models\
copy Models\RegisterViewModel.cs BerAuto_Part1\Models\

copy Controllers\AccountController.cs BerAuto_Part1\Controllers\
copy Controllers\HomeController.cs BerAuto_Part1\Controllers\

copy Data\BerAutoContext.cs BerAuto_Part1\Data\
copy Data\DbInitializer.cs BerAuto_Part1\Data\
copy Data\UserService.cs BerAuto_Part1\Data\
copy Data\CarService.cs BerAuto_Part1\Data\

mkdir BerAuto_Part1\Views\Home
mkdir BerAuto_Part1\Views\Account
mkdir BerAuto_Part1\Views\Shared

copy Views\Home\*.* BerAuto_Part1\Views\Home\
copy Views\Account\*.* BerAuto_Part1\Views\Account\
copy Views\Shared\*.* BerAuto_Part1\Views\Shared\
copy Views\_ViewImports.cshtml BerAuto_Part1\Views\
copy Views\_ViewStart.cshtml BerAuto_Part1\Views\

echo Fájlok másolása a második részbe...

copy BerAuto_Part1\*.* BerAuto_Part2\
copy BerAuto_Part1\Models\*.* BerAuto_Part2\Models\
copy BerAuto_Part1\Controllers\*.* BerAuto_Part2\Controllers\
copy BerAuto_Part1\Data\*.* BerAuto_Part2\Data\
copy BerAuto_Part1\Views\Home\*.* BerAuto_Part2\Views\Home\
copy BerAuto_Part1\Views\Account\*.* BerAuto_Part2\Views\Account\
copy BerAuto_Part1\Views\Shared\*.* BerAuto_Part2\Views\Shared\
copy BerAuto_Part1\Views\*.* BerAuto_Part2\Views\

copy Models\Rental.cs BerAuto_Part2\Models\
copy Models\RentalRequest.cs BerAuto_Part2\Models\
copy Models\Invoice.cs BerAuto_Part2\Models\

copy Controllers\UserController.cs BerAuto_Part2\Controllers\

copy Data\RentalService.cs BerAuto_Part2\Data\

mkdir BerAuto_Part2\Views\User
copy Views\User\*.* BerAuto_Part2\Views\User\

echo Kész! 