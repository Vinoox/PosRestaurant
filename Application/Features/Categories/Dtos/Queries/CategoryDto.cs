using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Mappings;
using Application.Features.Products.Dtos.Queries;
using AutoMapper;
using Domain.Entities;

namespace Application.Features.Categories.Dtos.Queries
{
    public class CategoryDto : IMap
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<ProductDto> Products { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Category, CategoryDto>();
        }
    }
}
