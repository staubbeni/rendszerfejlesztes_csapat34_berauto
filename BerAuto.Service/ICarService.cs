using BerAuto.DataContext.Context;
using BerAuto.DataContext.Entities;

public interface ICarService
{
    List<Car> GetAllCars();
}

public class CarService : ICarService
{
    private readonly AppDbContext _context;

    public CarService(AppDbContext context)
    {
        _context = context;
    }

    public List<Car> GetAllCars() 
    {
        return _context.Cars.ToList();
    }
}