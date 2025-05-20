using AutoMapper;
using BerAuto.DataContext.Dtos;
using BerAuto.DataContext.Entities;

namespace BerAuto.Services
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // User Mappings
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.Rentals, opt => opt.MapFrom(src => src.Rentals))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.Roles.Select(r => r.Name).ToList()));
            CreateMap<UserDto, User>();
            CreateMap<UserRegisterDto, User>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address != null ? new List<Address> { new Address
                {
                    City = src.Address.City,
                    Street = src.Address.Street,
                    ZipCode = src.Address.ZipCode,
                    State = src.Address.State
                } } : new List<Address>()));
            CreateMap<UserUpdateDto, User>();
            CreateMap<Address, AddressDto>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<AddressDto, Address>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()); // Hozzáadva az Id figyelmen kívül hagyásához

            // Car Mappings
            CreateMap<Car, CarDto>()
                .ForMember(dest => dest.Make, opt => opt.MapFrom(src => src.Brand))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.DailyRate))
                .ForMember(dest => dest.CarCategoryId, opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(dest => dest.IsAvailable, opt => opt.MapFrom(src => src.IsAvailable))
                .ForMember(dest => dest.Odometer, opt => opt.MapFrom(src => src.Odometer));

            CreateMap<CarDto, Car>()
                .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Make))
                .ForMember(dest => dest.DailyRate, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CarCategoryId));

            CreateMap<CarCreateDto, Car>()
                .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Make))
                .ForMember(dest => dest.Model, opt => opt.MapFrom(src => src.Model))
                .ForMember(dest => dest.DailyRate, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CarCategoryId))
                .ForMember(dest => dest.IsAvailable, opt => opt.Ignore())
                .ForMember(dest => dest.Odometer, opt => opt.Ignore());

            CreateMap<CarUpdateDto, Car>()
                .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Make))
                .ForMember(dest => dest.Model, opt => opt.MapFrom(src => src.Model))
                .ForMember(dest => dest.DailyRate, opt => opt.MapFrom(src => src.Price.HasValue ? src.Price.Value : 0))
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CarCategoryId))
                .ForMember(dest => dest.IsAvailable, opt => opt.Ignore())
                .ForMember(dest => dest.Odometer, opt => opt.Ignore());

            CreateMap<CarCategory, CarCategoryDto>();

            // Rental Mappings
            CreateMap<Rental, RentalDto>()
                .ForMember(dest => dest.From, opt => opt.MapFrom(src => src.From))
                .ForMember(dest => dest.To, opt => opt.MapFrom(src => src.To));
            CreateMap<RentalDto, Rental>();
            CreateMap<RentalRequestDto, Rental>()
                .ForMember(dest => dest.From, opt => opt.MapFrom(src => src.From))
                .ForMember(dest => dest.To, opt => opt.MapFrom(src => src.To))
                .ForMember(dest => dest.TotalCost, opt => opt.Ignore());

            // Role Mappings
            CreateMap<Role, RoleDto>();
        }
    }
}