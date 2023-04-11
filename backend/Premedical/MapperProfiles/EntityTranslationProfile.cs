using AutoMapper;
using Premedical.DataTransferObjects;
using Premedical.Models;

namespace Premedical.MapperProfiles;

public class EntityTranslationProfile : Profile
{
    public EntityTranslationProfile()
    {
        CreateMap<EntityTranslation, EntityTranslationDTO>()
            .ForMember(d => d.LangValue, o => o.MapFrom(s => s.Lang.LangValue))
            .ForMember(d => d.Files, o => o.MapFrom(s => s.EntityTranslationFiles));
        CreateMap<EntityTranslationPostDTO, EntityTranslation>();
        CreateMap<EntityTranslationUpdateDTO, EntityTranslation>();
        CreateMap<EntityTranslationContentDTO, EntityTranslation>();
    }
}
