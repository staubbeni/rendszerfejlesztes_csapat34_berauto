@echo off
echo Program.cs fájlok módosítása...

echo using BerAuto.Data; > BerAuto_Part1\Program.cs.new
echo using Microsoft.EntityFrameworkCore; >> BerAuto_Part1\Program.cs.new
echo. >> BerAuto_Part1\Program.cs.new
echo // Explicit port beállítása a WebApplication létrehozása előtt >> BerAuto_Part1\Program.cs.new
echo var urls = new string[] { "http://localhost:8081" }; >> BerAuto_Part1\Program.cs.new
echo. >> BerAuto_Part1\Program.cs.new
echo var builder = WebApplication.CreateBuilder(new WebApplicationOptions >> BerAuto_Part1\Program.cs.new
echo { >> BerAuto_Part1\Program.cs.new
echo     Args = args, >> BerAuto_Part1\Program.cs.new
echo     WebRootPath = "wwwroot", >> BerAuto_Part1\Program.cs.new
echo     ContentRootPath = Directory.GetCurrentDirectory() >> BerAuto_Part1\Program.cs.new
echo }); >> BerAuto_Part1\Program.cs.new
echo. >> BerAuto_Part1\Program.cs.new
echo // Explicit port beállítása >> BerAuto_Part1\Program.cs.new
echo builder.WebHost.UseUrls(urls); >> BerAuto_Part1\Program.cs.new
echo. >> BerAuto_Part1\Program.cs.new
echo // Add services to the container. >> BerAuto_Part1\Program.cs.new
echo builder.Services.AddControllersWithViews(); >> BerAuto_Part1\Program.cs.new
echo. >> BerAuto_Part1\Program.cs.new
echo // Adatbázis kontextus beállítása >> BerAuto_Part1\Program.cs.new
echo builder.Services.AddDbContext^<BerAutoContext^>(options =^> >> BerAuto_Part1\Program.cs.new
echo     options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))); >> BerAuto_Part1\Program.cs.new
echo. >> BerAuto_Part1\Program.cs.new
echo // Szolgáltatások regisztrálása >> BerAuto_Part1\Program.cs.new
echo builder.Services.AddScoped^<UserService^>(); >> BerAuto_Part1\Program.cs.new
echo builder.Services.AddScoped^<CarService^>(); >> BerAuto_Part1\Program.cs.new
echo. >> BerAuto_Part1\Program.cs.new
echo var app = builder.Build(); >> BerAuto_Part1\Program.cs.new
echo. >> BerAuto_Part1\Program.cs.new
echo // Adatbázis elérhetőségének ellenőrzése >> BerAuto_Part1\Program.cs.new
echo using (var scope = app.Services.CreateScope()) >> BerAuto_Part1\Program.cs.new
echo { >> BerAuto_Part1\Program.cs.new
echo     var services = scope.ServiceProvider; >> BerAuto_Part1\Program.cs.new
echo     try >> BerAuto_Part1\Program.cs.new
echo     { >> BerAuto_Part1\Program.cs.new
echo         var context = services.GetRequiredService^<BerAutoContext^>(); >> BerAuto_Part1\Program.cs.new
echo         if (context.Database.CanConnect()) >> BerAuto_Part1\Program.cs.new
echo         { >> BerAuto_Part1\Program.cs.new
echo             Console.WriteLine("Adatbázis kapcsolat sikeres!"); >> BerAuto_Part1\Program.cs.new
echo         } >> BerAuto_Part1\Program.cs.new
echo         else >> BerAuto_Part1\Program.cs.new
echo         { >> BerAuto_Part1\Program.cs.new
echo             Console.WriteLine("Nem sikerült kapcsolódni az adatbázishoz!"); >> BerAuto_Part1\Program.cs.new
echo         } >> BerAuto_Part1\Program.cs.new
echo     } >> BerAuto_Part1\Program.cs.new
echo     catch (Exception ex) >> BerAuto_Part1\Program.cs.new
echo     { >> BerAuto_Part1\Program.cs.new
echo         Console.WriteLine($"Hiba történt az adatbázis elérése során: {ex.Message}"); >> BerAuto_Part1\Program.cs.new
echo     } >> BerAuto_Part1\Program.cs.new
echo } >> BerAuto_Part1\Program.cs.new
echo. >> BerAuto_Part1\Program.cs.new
echo // Configure the HTTP request pipeline. >> BerAuto_Part1\Program.cs.new
echo if (app.Environment.IsDevelopment()) >> BerAuto_Part1\Program.cs.new
echo { >> BerAuto_Part1\Program.cs.new
echo     app.UseDeveloperExceptionPage(); >> BerAuto_Part1\Program.cs.new
echo } >> BerAuto_Part1\Program.cs.new
echo else >> BerAuto_Part1\Program.cs.new
echo { >> BerAuto_Part1\Program.cs.new
echo     app.UseExceptionHandler("/Home/Error"); >> BerAuto_Part1\Program.cs.new
echo } >> BerAuto_Part1\Program.cs.new
echo. >> BerAuto_Part1\Program.cs.new
echo app.UseStaticFiles(); >> BerAuto_Part1\Program.cs.new
echo app.UseRouting(); >> BerAuto_Part1\Program.cs.new
echo app.UseAuthorization(); >> BerAuto_Part1\Program.cs.new
echo. >> BerAuto_Part1\Program.cs.new
echo app.MapControllerRoute( >> BerAuto_Part1\Program.cs.new
echo     name: "default", >> BerAuto_Part1\Program.cs.new
echo     pattern: "{controller=Home}/{action=Index}/{id?}"); >> BerAuto_Part1\Program.cs.new
echo. >> BerAuto_Part1\Program.cs.new
echo Console.WriteLine("Az alkalmazás fut a http://localhost:8081 címen. Nyisd meg a böngészőben ezt a címet."); >> BerAuto_Part1\Program.cs.new
echo. >> BerAuto_Part1\Program.cs.new
echo app.Run(); >> BerAuto_Part1\Program.cs.new

move /y BerAuto_Part1\Program.cs.new BerAuto_Part1\Program.cs

echo using BerAuto.Data; > BerAuto_Part2\Program.cs.new
echo using Microsoft.EntityFrameworkCore; >> BerAuto_Part2\Program.cs.new
echo. >> BerAuto_Part2\Program.cs.new
echo // Explicit port beállítása a WebApplication létrehozása előtt >> BerAuto_Part2\Program.cs.new
echo var urls = new string[] { "http://localhost:8082" }; >> BerAuto_Part2\Program.cs.new
echo. >> BerAuto_Part2\Program.cs.new
echo var builder = WebApplication.CreateBuilder(new WebApplicationOptions >> BerAuto_Part2\Program.cs.new
echo { >> BerAuto_Part2\Program.cs.new
echo     Args = args, >> BerAuto_Part2\Program.cs.new
echo     WebRootPath = "wwwroot", >> BerAuto_Part2\Program.cs.new
echo     ContentRootPath = Directory.GetCurrentDirectory() >> BerAuto_Part2\Program.cs.new
echo }); >> BerAuto_Part2\Program.cs.new
echo. >> BerAuto_Part2\Program.cs.new
echo // Explicit port beállítása >> BerAuto_Part2\Program.cs.new
echo builder.WebHost.UseUrls(urls); >> BerAuto_Part2\Program.cs.new
echo. >> BerAuto_Part2\Program.cs.new
echo // Add services to the container. >> BerAuto_Part2\Program.cs.new
echo builder.Services.AddControllersWithViews(); >> BerAuto_Part2\Program.cs.new
echo. >> BerAuto_Part2\Program.cs.new
echo // Adatbázis kontextus beállítása >> BerAuto_Part2\Program.cs.new
echo builder.Services.AddDbContext^<BerAutoContext^>(options =^> >> BerAuto_Part2\Program.cs.new
echo     options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))); >> BerAuto_Part2\Program.cs.new
echo. >> BerAuto_Part2\Program.cs.new
echo // Szolgáltatások regisztrálása >> BerAuto_Part2\Program.cs.new
echo builder.Services.AddScoped^<UserService^>(); >> BerAuto_Part2\Program.cs.new
echo builder.Services.AddScoped^<CarService^>(); >> BerAuto_Part2\Program.cs.new
echo builder.Services.AddScoped^<RentalService^>(); >> BerAuto_Part2\Program.cs.new
echo. >> BerAuto_Part2\Program.cs.new
echo var app = builder.Build(); >> BerAuto_Part2\Program.cs.new
echo. >> BerAuto_Part2\Program.cs.new
echo // Adatbázis elérhetőségének ellenőrzése >> BerAuto_Part2\Program.cs.new
echo using (var scope = app.Services.CreateScope()) >> BerAuto_Part2\Program.cs.new
echo { >> BerAuto_Part2\Program.cs.new
echo     var services = scope.ServiceProvider; >> BerAuto_Part2\Program.cs.new
echo     try >> BerAuto_Part2\Program.cs.new
echo     { >> BerAuto_Part2\Program.cs.new
echo         var context = services.GetRequiredService^<BerAutoContext^>(); >> BerAuto_Part2\Program.cs.new
echo         if (context.Database.CanConnect()) >> BerAuto_Part2\Program.cs.new
echo         { >> BerAuto_Part2\Program.cs.new
echo             Console.WriteLine("Adatbázis kapcsolat sikeres!"); >> BerAuto_Part2\Program.cs.new
echo         } >> BerAuto_Part2\Program.cs.new
echo         else >> BerAuto_Part2\Program.cs.new
echo         { >> BerAuto_Part2\Program.cs.new
echo             Console.WriteLine("Nem sikerült kapcsolódni az adatbázishoz!"); >> BerAuto_Part2\Program.cs.new
echo         } >> BerAuto_Part2\Program.cs.new
echo     } >> BerAuto_Part2\Program.cs.new
echo     catch (Exception ex) >> BerAuto_Part2\Program.cs.new
echo     { >> BerAuto_Part2\Program.cs.new
echo         Console.WriteLine($"Hiba történt az adatbázis elérése során: {ex.Message}"); >> BerAuto_Part2\Program.cs.new
echo     } >> BerAuto_Part2\Program.cs.new
echo } >> BerAuto_Part2\Program.cs.new
echo. >> BerAuto_Part2\Program.cs.new
echo // Configure the HTTP request pipeline. >> BerAuto_Part2\Program.cs.new
echo if (app.Environment.IsDevelopment()) >> BerAuto_Part2\Program.cs.new
echo { >> BerAuto_Part2\Program.cs.new
echo     app.UseDeveloperExceptionPage(); >> BerAuto_Part2\Program.cs.new
echo } >> BerAuto_Part2\Program.cs.new
echo else >> BerAuto_Part2\Program.cs.new
echo { >> BerAuto_Part2\Program.cs.new
echo     app.UseExceptionHandler("/Home/Error"); >> BerAuto_Part2\Program.cs.new
echo } >> BerAuto_Part2\Program.cs.new
echo. >> BerAuto_Part2\Program.cs.new
echo app.UseStaticFiles(); >> BerAuto_Part2\Program.cs.new
echo app.UseRouting(); >> BerAuto_Part2\Program.cs.new
echo app.UseAuthorization(); >> BerAuto_Part2\Program.cs.new
echo. >> BerAuto_Part2\Program.cs.new
echo app.MapControllerRoute( >> BerAuto_Part2\Program.cs.new
echo     name: "default", >> BerAuto_Part2\Program.cs.new
echo     pattern: "{controller=Home}/{action=Index}/{id?}"); >> BerAuto_Part2\Program.cs.new
echo. >> BerAuto_Part2\Program.cs.new
echo Console.WriteLine("Az alkalmazás fut a http://localhost:8082 címen. Nyisd meg a böngészőben ezt a címet."); >> BerAuto_Part2\Program.cs.new
echo. >> BerAuto_Part2\Program.cs.new
echo app.Run(); >> BerAuto_Part2\Program.cs.new

move /y BerAuto_Part2\Program.cs.new BerAuto_Part2\Program.cs

echo using BerAuto.Data; > BerAuto_Part3\Program.cs.new
echo using Microsoft.EntityFrameworkCore; >> BerAuto_Part3\Program.cs.new
echo. >> BerAuto_Part3\Program.cs.new
echo // Explicit port beállítása a WebApplication létrehozása előtt >> BerAuto_Part3\Program.cs.new
echo var urls = new string[] { "http://localhost:8083" }; >> BerAuto_Part3\Program.cs.new
echo. >> BerAuto_Part3\Program.cs.new
echo var builder = WebApplication.CreateBuilder(new WebApplicationOptions >> BerAuto_Part3\Program.cs.new
echo { >> BerAuto_Part3\Program.cs.new
echo     Args = args, >> BerAuto_Part3\Program.cs.new
echo     WebRootPath = "wwwroot", >> BerAuto_Part3\Program.cs.new
echo     ContentRootPath = Directory.GetCurrentDirectory() >> BerAuto_Part3\Program.cs.new
echo }); >> BerAuto_Part3\Program.cs.new
echo. >> BerAuto_Part3\Program.cs.new
echo // Explicit port beállítása >> BerAuto_Part3\Program.cs.new
echo builder.WebHost.UseUrls(urls); >> BerAuto_Part3\Program.cs.new
echo. >> BerAuto_Part3\Program.cs.new
echo // Add services to the container. >> BerAuto_Part3\Program.cs.new
echo builder.Services.AddControllersWithViews(); >> BerAuto_Part3\Program.cs.new
echo. >> BerAuto_Part3\Program.cs.new
echo // Adatbázis kontextus beállítása >> BerAuto_Part3\Program.cs.new
echo builder.Services.AddDbContext^<BerAutoContext^>(options =^> >> BerAuto_Part3\Program.cs.new
echo     options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))); >> BerAuto_Part3\Program.cs.new
echo. >> BerAuto_Part3\Program.cs.new
echo // Szolgáltatások regisztrálása >> BerAuto_Part3\Program.cs.new
echo builder.Services.AddScoped^<UserService^>(); >> BerAuto_Part3\Program.cs.new
echo builder.Services.AddScoped^<CarService^>(); >> BerAuto_Part3\Program.cs.new
echo builder.Services.AddScoped^<RentalService^>(); >> BerAuto_Part3\Program.cs.new
echo. >> BerAuto_Part3\Program.cs.new
echo var app = builder.Build(); >> BerAuto_Part3\Program.cs.new
echo. >> BerAuto_Part3\Program.cs.new
echo // Adatbázis elérhetőségének ellenőrzése >> BerAuto_Part3\Program.cs.new
echo using (var scope = app.Services.CreateScope()) >> BerAuto_Part3\Program.cs.new
echo { >> BerAuto_Part3\Program.cs.new
echo     var services = scope.ServiceProvider; >> BerAuto_Part3\Program.cs.new
echo     try >> BerAuto_Part3\Program.cs.new
echo     { >> BerAuto_Part3\Program.cs.new
echo         var context = services.GetRequiredService^<BerAutoContext^>(); >> BerAuto_Part3\Program.cs.new
echo         if (context.Database.CanConnect()) >> BerAuto_Part3\Program.cs.new
echo         { >> BerAuto_Part3\Program.cs.new
echo             Console.WriteLine("Adatbázis kapcsolat sikeres!"); >> BerAuto_Part3\Program.cs.new
echo         } >> BerAuto_Part3\Program.cs.new
echo         else >> BerAuto_Part3\Program.cs.new
echo         { >> BerAuto_Part3\Program.cs.new
echo             Console.WriteLine("Nem sikerült kapcsolódni az adatbázishoz!"); >> BerAuto_Part3\Program.cs.new
echo         } >> BerAuto_Part3\Program.cs.new
echo     } >> BerAuto_Part3\Program.cs.new
echo     catch (Exception ex) >> BerAuto_Part3\Program.cs.new
echo     { >> BerAuto_Part3\Program.cs.new
echo         Console.WriteLine($"Hiba történt az adatbázis elérése során: {ex.Message}"); >> BerAuto_Part3\Program.cs.new
echo     } >> BerAuto_Part3\Program.cs.new
echo } >> BerAuto_Part3\Program.cs.new
echo. >> BerAuto_Part3\Program.cs.new
echo // Configure the HTTP request pipeline. >> BerAuto_Part3\Program.cs.new
echo if (app.Environment.IsDevelopment()) >> BerAuto_Part3\Program.cs.new
echo { >> BerAuto_Part3\Program.cs.new
echo     app.UseDeveloperExceptionPage(); >> BerAuto_Part3\Program.cs.new
echo } >> BerAuto_Part3\Program.cs.new
echo else >> BerAuto_Part3\Program.cs.new
echo { >> BerAuto_Part3\Program.cs.new
echo     app.UseExceptionHandler("/Home/Error"); >> BerAuto_Part3\Program.cs.new
echo } >> BerAuto_Part3\Program.cs.new
echo. >> BerAuto_Part3\Program.cs.new
echo app.UseStaticFiles(); >> BerAuto_Part3\Program.cs.new
echo app.UseRouting(); >> BerAuto_Part3\Program.cs.new
echo app.UseAuthorization(); >> BerAuto_Part3\Program.cs.new
echo. >> BerAuto_Part3\Program.cs.new
echo app.MapControllerRoute( >> BerAuto_Part3\Program.cs.new
echo     name: "default", >> BerAuto_Part3\Program.cs.new
echo     pattern: "{controller=Home}/{action=Index}/{id?}"); >> BerAuto_Part3\Program.cs.new
echo. >> BerAuto_Part3\Program.cs.new
echo Console.WriteLine("Az alkalmazás fut a http://localhost:8083 címen. Nyisd meg a böngészőben ezt a címet."); >> BerAuto_Part3\Program.cs.new
echo. >> BerAuto_Part3\Program.cs.new
echo app.Run(); >> BerAuto_Part3\Program.cs.new

move /y BerAuto_Part3\Program.cs.new BerAuto_Part3\Program.cs

echo Kész!