using BerAuto.DataContext.Context;
using BerAuto.DataContext.Entities;

public interface IUserService
{
    List<User> GetAllUsers();
}

public class UserService : IUserService
{
    private readonly AppDbContext _context;

    public UserService(AppDbContext context)
    {
        _context = context;
    }

    public List<User> GetAllUsers()
    {
        return _context.Users.ToList();
    }
}
