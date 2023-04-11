using AutoMapper;
using Premedical.DataTransferObjects;
using Premedical.Models;

namespace Premedical.MapperProfiles;

public class EntityTranslationFileProfile : Profile
{
    public EntityTranslationFileProfile()
    {
        CreateMap<EntityTranslationFile, EntityTranslationFileDTO>();
    }
}
