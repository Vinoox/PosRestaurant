using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;

namespace Application.Features.Products.Dtos.Queries
{
    public class ProductIngredientDto : IMap
    {
        public int IngredientId { get; set; }
        public string Name { get; set; } = null!;
        public decimal Amount { get; set; }
        public Unit Unit { get; set; }
    
        public void Mapping(Profile profile)
        {
            profile.CreateMap<ProductIngredient, ProductIngredientDto>()
                .ForMember(dest => dest.Name,
                            opt => opt.MapFrom(src => src.Ingredient.Name))
                .ForMember(dest => dest.Unit,
                            opt => opt.MapFrom(src => src.Unit.ToString()))
                .ForMember(dest => dest.Amount,
                            opt => opt.MapFrom(src => src.Amount));
        }
    }
}
