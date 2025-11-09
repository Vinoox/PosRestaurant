using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Mappings;
using Application.Features.Restaurants.Dtos.Queries;
using AutoMapper;
using Domain.Entities;

namespace Application.Features.Users.Dtos.Queries
{
    public class UserSummaryDto : IMap
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public IEnumerable<RestaurantSummaryDto> Restaurants { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<User, UserSummaryDto>();
        }
    }
}
