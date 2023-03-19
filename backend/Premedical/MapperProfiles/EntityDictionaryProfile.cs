using AutoMapper;
using Premedical.DataTransferObjects;
using Premedical.Models;

namespace Premedical.MapperProfiles;

public class EntityDictionaryProfile : Profile
{
    public EntityDictionaryProfile()
    {
        CreateMap<EntityDictionary, EntityDictionaryMainDTO>();
        CreateMap<EntityDictionaryPostDTO, EntityDictionary>();
        CreateMap<EntityDictionaryUpdateDTO, EntityDictionary>();
    }
}
