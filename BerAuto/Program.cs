using BerAuto.DataContext.Context;
using BerAuto.DataContext.Dtos;
using BerAuto.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

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

// **Seed default users**: admin, customer, employee
using (var scope = app.Services.CreateScope())
{
    var ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var userService = scope.ServiceProvider.GetRequiredService<IUserService>();

    // ensure roles exist
    var adminRole = ctx.Roles.First(r => r.Name == "Admin").Id;
    var customerRole = ctx.Roles.First(r => r.Name == "Customer").Id;
    var employeeRole = ctx.Roles.First(r => r.Name == "Employee").Id;

    // seed admin
    //key: eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjEwMDYiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiYWRtaW4iLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJhZG1pbkBkb21haW4uY29tIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvc2lkIjoiYjAzZjkwMmYtNDU1ZC00MTAyLWI0YWQtMGQyZjVlZTVhYTFjIiwiYXV0aF90aW1lIjoiMDUvMDkvMjAyNSAxMTozNDowNSIsInJvbGVJZHMiOiIxIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiQWRtaW4iLCJleHAiOjE3NDkzNzUyNDUsImlzcyI6Imh0dHBzOi8vbG9jYWxob3N0OjcwMjkiLCJhdWQiOiJodHRwczovL2xvY2FsaG9zdDo3MDI5In0.Le-jLTWYx8kGVJz4S-UpUTxx9rGgay3eSyKRHNZ3lP8
    if (!ctx.Users.Any(u => u.Name == "admin"))
    {
        userService.RegisterAsync(new UserRegisterDto
        {
            Name = "admin",
            Email = "admin@domain.com",
            Password = "adminadmin",
            PhoneNumber = "",
            RoleIds = new List<int> { adminRole }
        }).GetAwaiter().GetResult();
    }

    // seed customer
    //key: eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjEwMDciLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiY3VzdG9tZXIiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJjdXN0b21lckBkb21haW4uY29tIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvc2lkIjoiYzhmYzgwYWItYzM2Ny00MmY2LWIyYTctZGZmZDhhYTBiODlhIiwiYXV0aF90aW1lIjoiMDUvMDkvMjAyNSAxMToyMzoxMSIsInJvbGVJZHMiOiIyIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiQ3VzdG9tZXIiLCJleHAiOjE3NDkzNzQ1OTEsImlzcyI6Imh0dHBzOi8vbG9jYWxob3N0OjcwMjkiLCJhdWQiOiJodHRwczovL2xvY2FsaG9zdDo3MDI5In0.Zmh7ed9uMB_RYH9_2P3fFj8wmaJ5ea-TTJI0wOi6CbE
    if (!ctx.Users.Any(u => u.Name == "customer"))
    {
        userService.RegisterAsync(new UserRegisterDto
        {
            Name = "customer",
            Email = "customer@domain.com",
            Password = "customercustomer",
            PhoneNumber = "",
            RoleIds = new List<int> { customerRole }
        }).GetAwaiter().GetResult();
    }

    // seed employee
    //key: eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjEwMDgiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiZW1wbG95ZWUiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJlbXBsb3llZUBkb21haW4uY29tIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvc2lkIjoiMmVmNzVjZjQtOGMyZS00Zjc1LWE5NmQtZmFkMDE0ODBhMjBmIiwiYXV0aF90aW1lIjoiMDUvMDkvMjAyNSAxMTo0NTo1MyIsInJvbGVJZHMiOiIzIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiRW1wbG95ZWUiLCJleHAiOjE3NDkzNzU5NTMsImlzcyI6Imh0dHBzOi8vbG9jYWxob3N0OjcwMjkiLCJhdWQiOiJodHRwczovL2xvY2FsaG9zdDo3MDI5In0.5_a94D0A7RZKr3DEFR_6XGYe6-rlu9RlDrHQVSPZS6E
    if (!ctx.Users.Any(u => u.Name == "employee"))
    {
        userService.RegisterAsync(new UserRegisterDto
        {
            Name = "employee",
            Email = "employee@domain.com",
            Password = "employeeemployee",
            PhoneNumber = "",
            RoleIds = new List<int> { employeeRole }
        }).GetAwaiter().GetResult();
    }
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BerAuto API v1"));
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors("AllowAll");

app.MapControllers();

app.Run();
