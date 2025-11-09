using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Features.Restaurants.Dtos.Queries
{
    public class RestaurantSummaryDto : IMap
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public void Mapping(AutoMapper.Profile profile)
        {
            profile.CreateMap<Restaurant, RestaurantSummaryDto>();
        }
    }
}
