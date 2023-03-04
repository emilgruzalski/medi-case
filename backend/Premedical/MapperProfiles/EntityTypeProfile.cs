using AutoMapper;
using Premedical.DataTransferObjects;
using Premedical.Models;

namespace Premedical.MapperProfiles;

public class EntityTypeProfile : Profile
{
    public EntityTypeProfile()
    {
        CreateMap<EntityType, EntityTypeDTO>();
    }
}
