using AutoMapper;
using BarberBoss.Application.AutoMapper;

namespace CommonTestsUtilities.AutoMapper;
public class AutoMappingBuilder
{
    public static IMapper Build()
    {
        var mapper = new MapperConfiguration(config =>
        {
            config.AddProfile(new AutoMapping());
        });

        return mapper.CreateMapper();
    }
}
