using BerAuto.DataContext.Context;
using BerAuto.DataContext.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BerAuto.Services
{
    public interface IRoleService
    {
        List<Role> GetAllRole();
    }
    public class RoleServices : IRoleService
    {
        private readonly AppDbContext _context;
        public RoleServices(AppDbContext context)
        {
            _context = context;
        }
        public List<Role> List()
        {
            return _context.Roles.ToList();
        }
    }
}
