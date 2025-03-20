using BerAuto.DataContext.Context;
using BerAuto.DataContext.Entities;

public interface IRoleService
{
    List<Role> GetAllRoles();
}

public class RoleService : IRoleService 
{
    private readonly AppDbContext _context;

    public RoleService(AppDbContext context)
    {
        _context = context;
    }

    public List<Role> GetAllRoles()
    {
        return _context.Roles.ToList();
    }
}