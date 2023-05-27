using AutoMapper;
using MediCaseAPI.DataTransferObjects;
using MediCaseAPI.Models;

namespace MediCaseAPI.MapperProfiles;

public class EntityTypeProfile : Profile
{
    public EntityTypeProfile()
    {
        CreateMap<EntityType, EntityTypeDTO>()
            .ForMember(d => d.CanBeRoot, o => o.MapFrom(s => EntityTypeRules.RootTypes.Contains(s.Id)))
            .ForMember(d => d.AllowedChildTypeIds, o => o.MapFrom(s => EntityTypeRules.GetAllowedChildren(s.Id)));
    }
}
