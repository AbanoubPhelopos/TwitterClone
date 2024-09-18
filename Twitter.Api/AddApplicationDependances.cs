namespace Twitter.Api;

public static class AddApplicationDependances
{
    public static IServiceCollection AddApplicationDependanceies(this IServiceCollection services,IConfigurationManager configuration)
    {

        services.AddControllers();

        services.AddEndpointsApiExplorer()
            .AddSwaggerGen()
            .AddDbConfig(configuration)
            .AddAuthConfig();
        
        return services;
    }

    private static IServiceCollection AddDbConfig(this IServiceCollection services, IConfigurationManager configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("Default")));
        return services;
    }
    private static IServiceCollection AddAuthConfig(this IServiceCollection services)
    {
        services.AddIdentityApiEndpoints<User>().AddEntityFrameworkStores<ApplicationDbContext>();
        return services;
    }
    
}