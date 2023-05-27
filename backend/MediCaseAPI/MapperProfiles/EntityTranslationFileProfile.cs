using AutoMapper;
using MediCaseAPI.DataTransferObjects;
using MediCaseAPI.Models;

namespace MediCaseAPI.MapperProfiles;

public class EntityTranslationFileProfile : Profile
{
    public EntityTranslationFileProfile()
    {
        CreateMap<EntityTranslationFile, EntityTranslationFileDTO>();
    }
}
