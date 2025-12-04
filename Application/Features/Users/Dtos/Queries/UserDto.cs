using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Mappings;
using Application.Features.Restaurants.Dtos.Queries;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Users.Dtos.Queries
{
    public class UserDto : IMap
    {
        public required string Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required IdentityRole globalRole { get; set; }
        public required ICollection<RestaurantSummaryDto> Restaurants { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<User, UserDto>();
        }
    }
}
