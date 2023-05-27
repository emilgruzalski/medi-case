using AutoMapper;
using MediCaseAPI.DataTransferObjects;
using MediCaseAPI.Models;

namespace MediCaseAPI.MapperProfiles;

public class EntitiesGraphDataProfile : Profile
{
    public EntitiesGraphDataProfile()
    {
        CreateMap<EntitiesGraphDatum, EntityGraphDataDTO>();
    }
}
