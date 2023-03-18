using AutoMapper;
using Premedical.DataTransferObjects;
using Premedical.Models;

namespace Premedical.MapperProfiles;

public class EntitiesGraphDataProfile : Profile
{
    public EntitiesGraphDataProfile()
    {
        CreateMap<EntitiesGraphDatum, EntityGraphDataDTO>();
    }
}
