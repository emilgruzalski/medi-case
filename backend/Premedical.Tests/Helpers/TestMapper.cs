using AutoMapper;
using Premedical.MapperProfiles;

namespace Premedical.Tests.Helpers;

public static class TestMapper
{
    public static IMapper Create() =>
        new MapperConfiguration(cfg => cfg.AddMaps(typeof(EntityProfile).Assembly)).CreateMapper();
}
