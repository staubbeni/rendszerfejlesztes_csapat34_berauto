using BerAuto.DataContext.Context;
using BerAuto.DataContext.Dtos;
using BerAuto.DataContext.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

public interface IUserService
{
    Task<UserDto> RegisterAsync(UserRegisterDto userDto);
    Task<(string Token, UserDto User)> LoginAsync(UserLoginDto loginDto); // userDto helyett loginDto
    Task<UserDto> UpdateProfileAsync(int userId, UserUpdateDto userDto);
    Task<UserDto> UpdateAddressAsync(int userId, AddressDto addressDto);
    Task<List<RoleDto>> GetRolesAsync();
    Task<List<UserDto>> GetAllUsersAsync();
    Task<UserDto> GetUserByIdAsync(int userId);
}

public class UserService : IUserService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;
    private readonly ILogger<UserService> _logger;

    public UserService(
        AppDbContext context,
        IMapper mapper,
        IConfiguration configuration,
        ILogger<UserService> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<UserDto> RegisterAsync(UserRegisterDto userDto)
    {
        if (userDto == null) throw new ArgumentNullException(nameof(userDto));

        try
        {
            // Ellenőrizzük, hogy az email már létezik-e
            if (await _context.Users.AnyAsync(u => u.Email == userDto.Email))
            {
                _logger.LogWarning("Email already exists: {Email}", userDto.Email);
                throw new InvalidOperationException("Email already exists.");
            }

            var user = _mapper.Map<User>(userDto);
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
            user.Roles = new List<Role>();
            user.Address = new List<Address>();

            if (userDto.Address != null)
            {
                var address = _mapper.Map<Address>(userDto.Address);
                // UserId-t nem állítjuk be, mert az EF Core kezeli a kapcsolatot mentés után
                user.Address.Add(address);
            }

            if (userDto.RoleIds != null && userDto.RoleIds.Any())
            {
                foreach (var roleId in userDto.RoleIds)
                {
                    var existingRole = await _context.Roles.FirstOrDefaultAsync(r => r.Id == roleId);
                    if (existingRole != null)
                    {
                        user.Roles.Add(existingRole);
                    }
                    else
                    {
                        _logger.LogWarning($"Role with ID {roleId} not found.");
                    }
                }
            }

            if (!user.Roles.Any())
            {
                user.Roles.Add(await GetDefaultCustomerRoleAsync());
            }

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            // Betöltjük a mentett felhasználót a címekkel együtt a válaszhoz
            var savedUser = await _context.Users
                .Include(u => u.Roles)
                .Include(u => u.Address)
                .FirstOrDefaultAsync(u => u.Id == user.Id);

            return _mapper.Map<UserDto>(savedUser);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error during registration for email {Email}", userDto.Email);
            throw new Exception("Database error during registration.", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during registration for email {Email}", userDto.Email);
            throw new Exception("An error occurred during registration.", ex);
        }
    }

    private async Task<Role> GetDefaultCustomerRoleAsync()
    {
        try
        {
            var customerRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Customer");
            if (customerRole == null)
            {
                customerRole = new Role { Name = "Customer" };
                await _context.Roles.AddAsync(customerRole);
                await _context.SaveChangesAsync();
            }
            return customerRole;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating default Customer role.");
            throw new Exception("Failed to create default Customer role.", ex);
        }
    }

    public async Task<(string Token, UserDto User)> LoginAsync(UserLoginDto loginDto)
    {
        if (loginDto == null) throw new ArgumentNullException(nameof(loginDto));

        try
        {
            var user = await _context.Users
                .Include(u => u.Roles)
                .Include(u => u.Address) // Hozzáadva a címek betöltéséhez
                .FirstOrDefaultAsync(x => x.Email == loginDto.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            {
                _logger.LogWarning("Invalid login attempt for email {Email}", loginDto.Email);
                throw new UnauthorizedAccessException("Invalid credentials.");
            }

            var token = await GenerateToken(user);
            var mappedUser = _mapper.Map<UserDto>(user);
            return (token, mappedUser);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login for email {Email}", loginDto.Email);
            throw new UnauthorizedAccessException("Invalid credentials.", ex);
        }
    }

    private async Task<string> GenerateToken(User user)
    {
        if (user == null) throw new ArgumentNullException(nameof(user));

        try
        {
            var keyString = _configuration["Jwt:Key"];
            if (string.IsNullOrEmpty(keyString))
            {
                _logger.LogError("JWT key is not configured.");
                throw new InvalidOperationException("JWT key is not configured.");
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddDays(Convert.ToDouble(_configuration["Jwt:ExpireDays"] ?? "7"));

            var id = await GetClaimsIdentity(user);
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: id.Claims,
                expires: expires,
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating token for user ID {UserId}", user.Id);
            throw new Exception("Failed to generate token.", ex);
        }
    }

    private async Task<ClaimsIdentity> GetClaimsIdentity(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Sid, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.AuthTime, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture))
        };

        if (user.Roles != null && user.Roles.Any())
        {
            claims.AddRange(user.Roles.Select(role => new Claim("roleIds", role.Id.ToString())));
            claims.AddRange(user.Roles.Select(role => new Claim(ClaimTypes.Role, role.Name)));
        }

        return new ClaimsIdentity(claims, "Token");
    }

    public async Task<UserDto> UpdateProfileAsync(int userId, UserUpdateDto userDto)
    {
        if (userDto == null) throw new ArgumentNullException(nameof(userDto));

        var user = await _context.Users
            .Include(u => u.Roles)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
            _logger.LogWarning("User not found with ID {UserId}", userId);
            throw new KeyNotFoundException("User not found.");
        }

        try
        {
            _mapper.Map(userDto, user);

            if (userDto.RoleIds != null && userDto.RoleIds.Any())
            {
                user.Roles.Clear();
                foreach (var roleId in userDto.RoleIds)
                {
                    var existingRole = await _context.Roles.FirstOrDefaultAsync(r => r.Id == roleId);
                    if (existingRole != null)
                    {
                        user.Roles.Add(existingRole);
                    }
                    else
                    {
                        _logger.LogWarning($"Role with ID {roleId} not found for user ID {userId}.");
                    }
                }
            }

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return _mapper.Map<UserDto>(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating profile for user ID {UserId}", userId);
            throw new Exception("Failed to update user profile.", ex);
        }
    }

    public async Task<UserDto> UpdateAddressAsync(int userId, AddressDto addressDto)
    {
        if (addressDto == null) throw new ArgumentNullException(nameof(addressDto));

        var user = await _context.Users
            .Include(u => u.Address)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
            _logger.LogWarning("User not found with ID {UserId}", userId);
            throw new KeyNotFoundException("User not found.");
        }

        try
        {
            var address = _mapper.Map<Address>(addressDto);
            address.UserId = userId;
            user.Address.Clear();
            user.Address.Add(address);

            await _context.SaveChangesAsync();

            return _mapper.Map<UserDto>(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating address for user ID {UserId}", userId);
            throw new Exception("Failed to update user address.", ex);
        }
    }

    public async Task<List<RoleDto>> GetRolesAsync()
    {
        try
        {
            var roles = await _context.Roles.ToListAsync();
            return _mapper.Map<List<RoleDto>>(roles);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving roles.");
            throw new Exception("Failed to retrieve roles.", ex);
        }
    }

    public async Task<List<UserDto>> GetAllUsersAsync()
    {
        try
        {
            var users = await _context.Users
                .Include(u => u.Roles)
                .Include(u => u.Address)
                .ToListAsync();
            return _mapper.Map<List<UserDto>>(users);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all users.");
            throw new Exception("Failed to retrieve users.", ex);
        }
    }

    public async Task<UserDto> GetUserByIdAsync(int userId)
    {
        try
        {
            var user = await _context.Users
                .Include(u => u.Roles)
                .Include(u => u.Address)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                _logger.LogWarning("User not found with ID {UserId}", userId);
                throw new KeyNotFoundException("User not found.");
            }

            return _mapper.Map<UserDto>(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user with ID {UserId}", userId);
            throw new Exception("Failed to retrieve user.", ex);
        }
    }
}