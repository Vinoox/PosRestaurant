using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Mappings;
using Application.Features.Categories.Dtos.Queries;
using Application.Features.Ingredients.Dtos;
using Application.Features.Products.Dtos.Queries;
using Application.Features.Users.Dtos.Queries;
using AutoMapper;
using Domain.Entities;

namespace Application.Features.Restaurants.Dtos.Queries
{
    public class RestaurantDto : IMap
    {
        public int Id { get; set; }
        public string Name { get;  set; } = null!;
        public ICollection<CategoryDto> Categories { get;  set; } = new List<CategoryDto>();
        public ICollection<ProductDto> Products { get;  set; } = new List<ProductDto>();
        public ICollection<RestaurantUserDto> Staff { get;  set; } = new List<RestaurantUserDto>();
        public ICollection<IngredientDto> Ingredients { get;  set; } = new List<IngredientDto>();

        //public ICollection<Order> Orders { get; set; } = new List<Order>();

    public void Mapping(Profile profile)
        {
            profile.CreateMap<Restaurant, RestaurantDto>()
            .ForMember(dest => dest.Staff, opt => opt.MapFrom(src => src.StaffAssignments));
        }
    }
}
