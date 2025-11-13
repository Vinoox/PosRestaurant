using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Features.Products.Dtos.Queries
{
    public class ProductDto : IMap
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string CategoryName { get; set; } = null!;
        public List<ProductIngredientDto> Ingredients { get; set; } = [];

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(
                dest => dest.Ingredients,
                opt => opt.MapFrom(src => src.ProductIngredients
                .Select(pi => new ProductIngredientDto
                {
                    IngredientId = pi.IngredientId,
                    Name = pi.Ingredient.Name,
                    Quantity = pi.Quantity,
                    Unit = pi.Ingredient.Unit.ToString(),
                })
                ));
        }
    }
}
