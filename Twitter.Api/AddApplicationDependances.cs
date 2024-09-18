using Twitter.Api.Authantication;
using Twitter.Application.Services;

namespace Twitter.Api;
public static class AddApplicationDependances
{
    public static IServiceCollection AddApplicationDependanceies(this IServiceCollection services,IConfigurationManager configuration)
    {

        services.AddControllers();

        services.AddEndpointsApiExplorer()
            .AddSwaggerGen()
            .AddDbConfig(configuration)
            .AddAuthConfig()
            .AddValidationconfig()
            .AddServicesDependancy();
        
        return services;
    }

    private static IServiceCollection AddDbConfig(this IServiceCollection services, IConfigurationManager configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("Default")));
        return services;
    }
    private static IServiceCollection AddAuthConfig(this IServiceCollection services)
    {
        services.AddSingleton<IJwtProvider, JwtProvider>();
        services.AddScoped<IAuthServices, AuthServices>();
        
        services.AddIdentityApiEndpoints<User>().AddEntityFrameworkStores<ApplicationDbContext>();
        return services;
    }

    private static IServiceCollection AddValidationconfig(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation()
                        .AddValidatorsFromAssembly(typeof(LoginValidation).Assembly);
        return services;
    }

    private static IServiceCollection AddServicesDependancy(this IServiceCollection services)
    {
        return services;
    }
    
}