using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Mappings;
using Domain.Entities;
using Domain.Enums;

namespace Application.Features.Ingredients.Dtos
{
    public class CreateIngredientDto : IMap
    {
        public string Name { get; set; }
        public Unit Unit { get; set; }

        public void Mapping(AutoMapper.Profile profile)
        {
            profile.CreateMap<CreateIngredientDto, Ingredient>();
        }
    }
}
