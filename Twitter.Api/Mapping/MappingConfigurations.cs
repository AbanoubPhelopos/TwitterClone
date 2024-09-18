using Mapster;
using Twitter.Contract.Post;

namespace Twitter.Api.Mapping;

public class MappingConfigurations : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        //config.NewConfig<Post,PostResponse>().Map(d=>d, s=>s)
    }
}