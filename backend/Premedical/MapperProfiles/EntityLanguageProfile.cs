using AutoMapper;
using Premedical.DataTransferObjects;
using Premedical.Models;

namespace Premedical.MapperProfiles;

public class EntityLanguageProfile : Profile
{
    public EntityLanguageProfile()
    {
        CreateMap<EntityLanguage, EntityLanguageGetDTO>();
        CreateMap<EntityLanguagePostDTO, EntityLanguage>()
            .ForMember(d => d.LangValue, o => o.MapFrom(s => s.LangValue.Trim().ToLowerInvariant()));
        CreateMap<EntityLanguageUpdateDTO, EntityLanguage>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.LangValue, o => o.MapFrom(s => s.LangValue.Trim().ToLowerInvariant()));
    }
}
