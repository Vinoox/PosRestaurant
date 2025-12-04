using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Features.StaffManagement.Dtos.Queries
{
    public class StaffAssignmentDto : IMap
    {
        public required string UserId { get; set; }
        public required string UserName { get; set; }
        public required string RoleName { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<StaffAssignment, StaffAssignmentDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.FirstName + " " + src.User.LastName))
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.Name));
        }
    }
}
