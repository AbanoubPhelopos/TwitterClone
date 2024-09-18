using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Twitter.Application.Authantication;
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
        
        services.AddIdentity<User, IdentityRole<Guid>>()
            .AddEntityFrameworkStores<ApplicationDbContext>();
        
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                o.SaveToken = true;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes("RqqpETYPyJeeByw786ufuj333OdlcG0I")),
                    ValidIssuer = "TwitterApp",
                    ValidAudience = "TwitterApp users"
                };
            });
        
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