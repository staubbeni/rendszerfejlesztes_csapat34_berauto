using BerAuto.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

builder.Services.AddControllersWithViews();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Adatbázis létrehozása és táblák inicializálása
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        
        // Adatbázis létrehozása
        using (var connection = new SqlConnection(connectionString.Replace("Database=CarRentalDB;", "Database=master;")))
        {
            connection.Open();
            
            // Adatbázis törlése, ha létezik
            using (var command = new SqlCommand("IF EXISTS (SELECT * FROM sys.databases WHERE name = 'CarRentalDB') DROP DATABASE CarRentalDB", connection))
            {
                command.ExecuteNonQuery();
            }
            
            // Adatbázis létrehozása
            using (var command = new SqlCommand("CREATE DATABASE CarRentalDB", connection))
            {
                command.ExecuteNonQuery();
            }
        }
        
        // Táblák létrehozása
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            
            // Cars tábla
            using (var command = new SqlCommand(@"
                CREATE TABLE Cars (
                    Id INT PRIMARY KEY IDENTITY(1,1),
                    Make NVARCHAR(MAX) NOT NULL,
                    Model NVARCHAR(MAX) NOT NULL,
                    Year INT NOT NULL,
                    LicensePlate NVARCHAR(MAX) NOT NULL,
                    DailyRate DECIMAL(18,2) NOT NULL,
                    IsAvailable BIT NOT NULL
                )", connection))
            {
                command.ExecuteNonQuery();
            }
            
            // Customers tábla
            using (var command = new SqlCommand(@"
                CREATE TABLE Customers (
                    Id INT PRIMARY KEY IDENTITY(1,1),
                    Name NVARCHAR(MAX) NOT NULL,
                    Email NVARCHAR(MAX) NOT NULL,
                    Phone NVARCHAR(MAX) NOT NULL,
                    Address NVARCHAR(MAX) NOT NULL,
                    DriverLicense NVARCHAR(MAX) NOT NULL
                )", connection))
            {
                command.ExecuteNonQuery();
            }
            
            // Rentals tábla
            using (var command = new SqlCommand(@"
                CREATE TABLE Rentals (
                    Id INT PRIMARY KEY IDENTITY(1,1),
                    CarId INT NOT NULL,
                    CustomerId INT NOT NULL,
                    RentalDate DATETIME2 NOT NULL,
                    ReturnDate DATETIME2 NOT NULL,
                    TotalCost DECIMAL(18,2) NOT NULL,
                    IsCompleted BIT NOT NULL,
                    FOREIGN KEY (CarId) REFERENCES Cars(Id),
                    FOREIGN KEY (CustomerId) REFERENCES Customers(Id)
                )", connection))
            {
                command.ExecuteNonQuery();
            }
            
            // Kezdeti adatok beszúrása a Cars táblába
            using (var command = new SqlCommand(@"
                INSERT INTO Cars (Make, Model, Year, LicensePlate, DailyRate, IsAvailable)
                VALUES 
                ('Toyota', 'Corolla', 2020, 'ABC-123', 15000, 1),
                ('Honda', 'Civic', 2021, 'DEF-456', 17000, 1),
                ('Ford', 'Focus', 2019, 'GHI-789', 13000, 1)", connection))
            {
                command.ExecuteNonQuery();
            }
            
            // Kezdeti adatok beszúrása a Customers táblába
            using (var command = new SqlCommand(@"
                INSERT INTO Customers (Name, Email, Phone, Address, DriverLicense)
                VALUES 
                ('Kovács János', 'kovacs.janos@example.com', '06301234567', 'Budapest, Példa utca 1.', 'AB123456'),
                ('Nagy Éva', 'nagy.eva@example.com', '06209876543', 'Debrecen, Minta utca 2.', 'CD789012')", connection))
            {
                command.ExecuteNonQuery();
            }
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Hiba történt az adatbázis létrehozása során: {Message}", ex.Message);
        
        // Részletesebb hibaüzenet a fejlesztés megkönnyítéséhez
        if (app.Environment.IsDevelopment())
        {
            Console.WriteLine($"Adatbázis hiba: {ex.Message}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Belső hiba: {ex.InnerException.Message}");
            }
        }
    }
}

app.Run();
