using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;

namespace Application.Features.Users.Dtos
{
    public class UpdateUserDto : IMap
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Pin { get; set; }
        public UserRole? Duty { get; set; }

        public string? OldPassword { get; set; }
        public string? NewPassword { get; set; }
        public string? ConfirmNewPassword { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateUserDto, User>()
                .ForMember(dest => dest.FirstName, opt => opt.Condition(src => !string.IsNullOrEmpty(src.FirstName)))

                .ForMember(dest => dest.LastName, opt => opt.Condition(src => !string.IsNullOrEmpty(src.LastName)))

                .ForMember(dest => dest.Pin, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Pin)))

                .ForMember(dest => dest.Duty, opt => opt.Condition(src => src.Duty.HasValue))

                .ForMember(dest => dest.PasswordHash, opt =>
                {
                    opt.Condition(src => !string.IsNullOrEmpty(src.NewPassword));
                    opt.MapFrom(src => BCrypt.Net.BCrypt.HashPassword(src.NewPassword));
                });
        }
    }
}