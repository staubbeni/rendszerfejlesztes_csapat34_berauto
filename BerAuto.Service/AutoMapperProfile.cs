using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<UserRegisterDto, User>();
            CreateMap<UserUpdateDto, User>();
            CreateMap<Address, AddressDto>().ReverseMap();

            // Car Mappings
            CreateMap<Car, CarDto>().ReverseMap();
            CreateMap<CarCreateDto, Car>()
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CarCategoryId));
            CreateMap<CarUpdateDto, Car>()
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CarCategoryId));

            CreateMap<CarCategory, CarCategoryDto>();

            // Rental Mappings
            CreateMap<Rental, RentalDto>();
            CreateMap<Role, RoleDto>();
        }
    }
}

