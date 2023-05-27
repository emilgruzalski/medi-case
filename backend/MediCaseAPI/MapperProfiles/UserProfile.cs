using AutoMapper;
using MediCaseAPI.DataTransferObjects;
using MediCaseAPI.Models;

namespace MediCaseAPI.MapperProfiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDTO>()
            .ForMember(d => d.Groups, o => o.MapFrom(s => s.Groups.OrderBy(g => g.Name)));
        CreateMap<UserRegisterDTO, User>()
            .ForMember(d => d.Email, o => o.MapFrom(s => s.Email.Trim().ToLowerInvariant()))
            .ForMember(d => d.PasswordHash, o => o.Ignore());
    }
}
