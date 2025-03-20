using BerAuto.DataContext.Context;
using BerAuto.DataContext.Entities;

public interface ICarCategoryService
{
    List<CarCategory> GetAllCarCategories();
}

public class CarCategoryService : ICarCategoryService
{
    private readonly AppDbContext _context;

    public CarCategoryService(AppDbContext context)
    {
        _context = context;
    }

    public List<CarCategory> GetAllCarCategories() 
    {
        return _context.CarCategories.ToList();
    }
}