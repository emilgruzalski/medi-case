using AutoMapper;
using Premedical.DataTransferObjects;
using Premedical.Models;

namespace Premedical.MapperProfiles;

public class EntityProfile : Profile
{
    public EntityProfile()
    {
        CreateMap<Entity, EntityDTO>()
            .ForMember(d => d.TypeName, o => o.MapFrom(s => s.Type.Name))
            .ForMember(d => d.ParentId, o => o.MapFrom(s => s.ParentLink == null ? null : s.ParentLink.ParentId))
            .ForMember(d => d.Translations, o => o.MapFrom(s => s.EntityTranslations));
    }
}
