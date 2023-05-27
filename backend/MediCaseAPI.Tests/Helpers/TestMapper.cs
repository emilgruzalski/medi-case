using AutoMapper;
using MediCaseAPI.MapperProfiles;

namespace MediCaseAPI.Tests.Helpers;

public static class TestMapper
{
    public static IMapper Create() =>
        new MapperConfiguration(cfg => cfg.AddMaps(typeof(EntityProfile).Assembly)).CreateMapper();
}
