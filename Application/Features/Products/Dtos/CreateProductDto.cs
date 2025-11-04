using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Features.Products.Dtos
{
    public class CreateProductDto : IMap
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public List<IngredientAmountDto> Ingredients { get; set; } = [];
    
        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateProductDto, Product>()
                .ForMember(dest => dest.ProductIngredients, opt => opt.Ignore()); //implementation ProductService will handle ProductIngredients mapping
        }
    }
}
