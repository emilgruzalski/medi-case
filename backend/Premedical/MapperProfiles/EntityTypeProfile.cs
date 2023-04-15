using AutoMapper;
using Premedical.DataTransferObjects;
using Premedical.Models;

namespace Premedical.MapperProfiles;

public class EntityTypeProfile : Profile
{
    public EntityTypeProfile()
    {
        CreateMap<EntityType, EntityTypeDTO>()
            .ForMember(d => d.CanBeRoot, o => o.MapFrom(s => EntityTypeRules.RootTypes.Contains(s.Id)))
            .ForMember(d => d.AllowedChildTypeIds, o => o.MapFrom(s => EntityTypeRules.GetAllowedChildren(s.Id)));
    }
}
