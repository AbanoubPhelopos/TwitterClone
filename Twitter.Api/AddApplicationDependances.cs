using Microsoft.OpenApi.Models;

namespace Twitter.Api;
public static class AddApplicationDependances
{
    public static IServiceCollection AddApplicationDependencies(this IServiceCollection services, IConfigurationManager configuration)
    {

        services.AddControllers();

        services.AddEndpointsApiExplorer()
            .AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });

            })
            .AddDbConfig(configuration)
            .AddAuthConfig()
            .AddValidationconfig()
            .AddMappingConfig()
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
                o.MapInboundClaims = false;
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

    private static IServiceCollection AddMappingConfig(this IServiceCollection services)
    {
        var mappingConfig = TypeAdapterConfig.GlobalSettings;
        mappingConfig.Scan(Assembly.GetExecutingAssembly());
        services.AddSingleton<IMapper>(new Mapper(mappingConfig));
        return services;
    }

    private static IServiceCollection AddServicesDependancy(this IServiceCollection services)
    {
        
        return services;
    }


}