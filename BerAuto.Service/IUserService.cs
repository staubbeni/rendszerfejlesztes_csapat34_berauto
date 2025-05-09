using BerAuto.DataContext.Context;
using BerAuto.DataContext.Dtos;
using BerAuto.DataContext.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;

public interface IUserService
{
    Task<UserDto> RegisterAsync(UserRegisterDto userDto);
    Task<string> LoginAsync(UserLoginDto userDto);
    Task<UserDto> UpdateProfileAsync(int userId, UserUpdateDto userDto);
    Task<UserDto> UpdateAddressAsync(int userId, AddressDto addressDto);
    Task<IList<RoleDto>> GetRolesAsync();
    Task<List<User>> GetAllUsersAsync();
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
        _context = context;
        _mapper = mapper;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<UserDto> RegisterAsync(UserRegisterDto userDto)
    {
        try
        {
            var user = _mapper.Map<User>(userDto);
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
            user.Roles = new List<Role>();

            if (userDto.RoleIds != null)
            {
                foreach (var roleId in userDto.RoleIds)
                {
                    var existingRole = await _context.Roles.FirstOrDefaultAsync(r => r.Id == roleId);
                    if (existingRole != null)
                    {
                        user.Roles.Add(existingRole);
                    }
                }
            }

            if (!user.Roles.Any())
            {
                user.Roles.Add(await GetDefaultCustomerRoleAsync());
            }

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return _mapper.Map<UserDto>(user);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error during registration: {ex.Message}");
            throw new Exception("An error occurred during registration.");
        }
    }

    private async Task<Role> GetDefaultCustomerRoleAsync()
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

    public async Task<string> LoginAsync(UserLoginDto userDto)
    {
        try
        {
            var user = await _context.Users.Include(u => u.Roles).FirstOrDefaultAsync(x => x.Email == userDto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(userDto.Password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Invalid credentials.");
            }

            return await GenerateToken(user);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error during login: {ex.Message}");
            throw new UnauthorizedAccessException("Invalid credentials.");
        }
    }

    private async Task<string> GenerateToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["Jwt:ExpireDays"]));

        var id = await GetClaimsIdentity(user);
        var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], id.Claims, expires: expires, signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private async Task<ClaimsIdentity> GetClaimsIdentity(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Sid, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.AuthTime, DateTime.Now.ToString(CultureInfo.InvariantCulture))
        };

        if (user.Roles != null && user.Roles.Any())
        {
            claims.AddRange(user.Roles.Select(role => new Claim("roleIds", Convert.ToString(role.Id))));
            claims.AddRange(user.Roles.Select(role => new Claim(ClaimTypes.Role, role.Name)));
        }

        return new ClaimsIdentity(claims, "Token");
    }

    public async Task<UserDto> UpdateProfileAsync(int userId, UserUpdateDto userDto)
    {
        var user = await _context.Users.Include(u => u.Roles).FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null)
        {
            throw new KeyNotFoundException("User not found.");
        }

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
            }
        }

        _context.Users.Update(user);
        await _context.SaveChangesAsync();

        return _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto> UpdateAddressAsync(int userId, AddressDto addressDto)
    {
        var user = await _context.Users.Include(u => u.Address).FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null)
        {
            throw new KeyNotFoundException("User not found.");
        }

        var address = _mapper.Map<Address>(addressDto);

        if (user.Address == null)
        {
            user.Address = new List<Address>();
        }

        user.Address.Add(address);

        await _context.SaveChangesAsync();

        return _mapper.Map<UserDto>(user);
    }

    public async Task<IList<RoleDto>> GetRolesAsync()
    {
        var roles = await _context.Roles.ToListAsync();
        return _mapper.Map<IList<RoleDto>>(roles);
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        return await _context.Users.ToListAsync();
    }
}

