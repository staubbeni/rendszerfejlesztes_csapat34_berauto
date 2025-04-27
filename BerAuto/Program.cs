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
