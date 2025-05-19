using BerAuto.DataContext.Context;
using BerAuto.DataContext.Dtos;
using BerAuto.DataContext.Entities;
using BerAuto.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Scoped services
builder.Services.AddScoped<IAddressService, AddressService>();
builder.Services.AddScoped<ICarCategoryService, CarCategoryService>();
builder.Services.AddScoped<ICarService, CarService>();
builder.Services.AddScoped<IRentalService, RentalService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IUserService, UserService>();

// Add JWT authentication
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
            ClockSkew = TimeSpan.Zero
        };
    });

// Authorization policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
    options.AddPolicy("CustomerPolicy", policy => policy.RequireRole("Customer"));
    options.AddPolicy("EmployeePolicy", policy => policy.RequireRole("Employee"));
});

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

// AutoMapper config
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

// Swagger configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "BerAuto API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please insert JWT token",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            new string[] { }
        }
    });
});

// DbContext configuration
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

// Seed data
using (var scope = app.Services.CreateScope())
{
    var ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var userService = scope.ServiceProvider.GetRequiredService<IUserService>();

    ctx.Database.Migrate();

    // Seed Roles
    if (!ctx.Roles.Any())
    {
        ctx.Roles.AddRange(
            new Role { Name = "Admin" },
            new Role { Name = "Customer" },
            new Role { Name = "Employee" }
        );
        ctx.SaveChanges();
    }

    // Get Role IDs
    var adminRole = ctx.Roles.First(r => r.Name == "Admin").Id;
    var customerRole = ctx.Roles.First(r => r.Name == "Customer").Id;
    var employeeRole = ctx.Roles.First(r => r.Name == "Employee").Id;

    // Seed Users
    if (!ctx.Users.Any())
    {
        var users = new[]
        {
            new UserRegisterDto { Name = "admin", Email = "admin@berauto.com", Password = "adminadmin", PhoneNumber = "1234567890", RoleIds = new List<int> { adminRole } },
            new UserRegisterDto { Name = "customer1", Email = "cust1@berauto.com", Password = "customer1", PhoneNumber = "1111111111", RoleIds = new List<int> { customerRole } },
            new UserRegisterDto { Name = "employee1", Email = "emp1@berauto.com", Password = "employee1", PhoneNumber = "2222222222", RoleIds = new List<int> { employeeRole } },
            new UserRegisterDto { Name = "employee2", Email = "emp2@berauto.com", Password = "employee2", PhoneNumber = "3333333333", RoleIds = new List<int> { employeeRole } },
            new UserRegisterDto { Name = "customer2", Email = "cust2@berauto.com", Password = "customer2", PhoneNumber = "4444444444", RoleIds = new List<int> { customerRole } }
        };

        foreach (var user in users)
        {
            userService.RegisterAsync(user).GetAwaiter().GetResult();
        }
    }

    // Seed Car Categories
    if (!ctx.CarCategories.Any())
    {
        ctx.CarCategories.AddRange(
            new CarCategory { Name = "Sedan", Description = "N�gyajt�s, k�nyelmes szem�lyaut�" },
            new CarCategory { Name = "SUV", Description = "Terepj�r� jelleg� nagyobb m�ret� aut�" },
            new CarCategory { Name = "Hatchback", Description = "Kompakt m�ret� v�rosi aut�" },
            new CarCategory { Name = "Coupe", Description = "Sportos, k�tajt�s j�rm�" },
            new CarCategory { Name = "Van", Description = "Nagy t�rrel rendelkez� j�rm�, csal�di vagy �zleti c�lokra" }
        );
        ctx.SaveChanges();
    }

    // Seed Cars
    if (!ctx.Cars.Any())
    {
        ctx.Cars.AddRange(
            new Car { Brand = "Toyota", Model = "Corolla", Odometer = 45000, IsAvailable = true, CategoryId = 1, DailyRate = 5000.00m },
            new Car { Brand = "BMW", Model = "X5", Odometer = 30000, IsAvailable = true, CategoryId = 2, DailyRate = 4500.00m },
            new Car { Brand = "Ford", Model = "Focus", Odometer = 60000, IsAvailable = true, CategoryId = 1, DailyRate = 6000.00m },
            new Car { Brand = "Audi", Model = "A4", Odometer = 25000, IsAvailable = true, CategoryId = 2, DailyRate = 10000.00m },
            new Car { Brand = "Honda", Model = "Civic", Odometer = 15000, IsAvailable = true, CategoryId = 1, DailyRate = 8000.00m }
        );
        ctx.SaveChanges();
    }

    // Seed Addresses
    if (!ctx.Addresses.Any())
    {
        var firstUserId = ctx.Users.First().Id;

        ctx.Addresses.AddRange(
            new Address { Street = "F� utca 1", City = "Budapest", State = "Pest", ZipCode = "1000", UserId = firstUserId },
            new Address { Street = "Kossuth t�r 5", City = "Debrecen", State = "Hajd�-Bihar", ZipCode = "4024", UserId = firstUserId },
            new Address { Street = "Pet�fi S�ndor utca 10", City = "Szeged", State = "Csongr�d", ZipCode = "6720", UserId = firstUserId },
            new Address { Street = "R�k�czi �t 3", City = "Miskolc", State = "Borsod", ZipCode = "3525", UserId = firstUserId },
            new Address { Street = "Ady Endre utca 7", City = "P�cs", State = "Baranya", ZipCode = "7621", UserId = firstUserId }
        );
        ctx.SaveChanges();
    }
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BerAuto API v1"));
}

//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors("AllowAll");

app.MapControllers();

app.Run();
