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
    public class UpdateProductDto : IMap
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public int? CategoryId { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateProductDto, ProductUpdateParams>();
        }
    }
}
