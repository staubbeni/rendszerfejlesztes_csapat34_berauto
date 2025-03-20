using BerAuto.DataContext.Context;
using BerAuto.DataContext.Entities;

public interface IRentalService
{
    List<Rental> GetAllRentals();
}

public class RentalService : IRentalService
{
    private readonly AppDbContext _context;

    public RentalService(AppDbContext context)
    {
        _context = context;
    }

    public List<Rental> GetAllRentals() 
    {
        return _context.Rentals.ToList();
    }
}