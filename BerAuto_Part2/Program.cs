using BerAuto.Data; 
using Microsoft.EntityFrameworkCore; 
 
// Explicit port beállítása a WebApplication létrehozása előtt 
var urls = new string[] { "http://localhost:8082" }; 
 
var builder = WebApplication.CreateBuilder(new WebApplicationOptions 
{ 
    Args = args, 
    WebRootPath = "wwwroot", 
    ContentRootPath = Directory.GetCurrentDirectory() 
}); 
 
// Explicit port beállítása 
builder.WebHost.UseUrls(urls); 
 
// Add services to the container. 
builder.Services.AddControllersWithViews(); 
 
// Adatbázis kontextus beállítása 
builder.Services.AddDbContext<BerAutoContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))); 
 
// Szolgáltatások regisztrálása 
builder.Services.AddScoped<UserService>(); 
builder.Services.AddScoped<CarService>(); 
builder.Services.AddScoped<RentalService>(); 
 
var app = builder.Build(); 
 
// Adatbázis elérhetőségének ellenőrzése 
using (var scope = app.Services.CreateScope()) 
{ 
    var services = scope.ServiceProvider; 
    try 
    { 
        var context = services.GetRequiredService<BerAutoContext>(); 
        if (context.Database.CanConnect()) 
        { 
            Console.WriteLine("Adatbázis kapcsolat sikeres!"); 
        } 
        else 
        { 
            Console.WriteLine("Nem sikerült kapcsolódni az adatbázishoz!"); 
        } 
    } 
    catch (Exception ex) 
    { 
        Console.WriteLine($"Hiba történt az adatbázis elérése során: {ex.Message}"); 
    } 
} 
 
// Configure the HTTP request pipeline. 
if (app.Environment.IsDevelopment()) 
{ 
    app.UseDeveloperExceptionPage(); 
} 
else 
{ 
    app.UseExceptionHandler("/Home/Error"); 
} 
 
app.UseStaticFiles(); 
app.UseRouting(); 
app.UseAuthorization(); 
 
app.MapControllerRoute( 
    name: "default", 
    pattern: "{controller=Home}/{action=Index}/{id?}"); 
 
Console.WriteLine("Az alkalmazás fut a http://localhost:8082 címen. Nyisd meg a böngészőben ezt a címet."); 
 
app.Run(); 
