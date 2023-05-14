using AutoMapper;
using Premedical.DataTransferObjects;
using Premedical.Models;

namespace Premedical.MapperProfiles;

public class GroupProfile : Profile
{
    public GroupProfile()
    {
        CreateMap<Group, GroupDTO>()
            .ForMember(d => d.UsersCount, o => o.MapFrom(s => s.Users.Count));
        CreateMap<Group, GroupSummaryDTO>();
        CreateMap<GroupPostDTO, Group>()
            .ForMember(d => d.Name, o => o.MapFrom(s => s.Name.Trim()));
    }
}
