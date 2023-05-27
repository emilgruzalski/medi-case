using AutoMapper;
using MediCaseAPI.DataTransferObjects;
using MediCaseAPI.Models;

namespace MediCaseAPI.MapperProfiles;

public class EntityDictionaryProfile : Profile
{
    public EntityDictionaryProfile()
    {
        CreateMap<EntityDictionary, EntityDictionaryMainDTO>();
        CreateMap<EntityDictionaryPostDTO, EntityDictionary>();
        CreateMap<EntityDictionaryUpdateDTO, EntityDictionary>();
    }
}
