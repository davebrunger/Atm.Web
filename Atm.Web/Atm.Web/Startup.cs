namespace Atm.Web;

public static class Startup
{
    private const string corsPolicyName = "corsPolicyName";

    public static IServiceCollection AddAtmDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AtmDbContext>(options =>
        {
            options.UseSqlServer(configuration["ATM:ConnectionString"]);
        });
        services.AddScoped<IAtmDbContext, AtmDbContext>();
        return services;
    }

    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(config =>
        {
            config.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Description = "JWT Authorization header using the Bearer scheme.",
            });
            config.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme, Id = "bearerAuth"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });
        return services;
    }

    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(o =>
        {
            o.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidAudience = configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey
                (Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!)),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true
            };
        });
        return services;
    }

    public static IServiceCollection AddClientCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(corsPolicyName, policy =>
            {
                policy.WithOrigins("http://localhost:3000", "http://192.168.86.67:3000");
                policy.WithHeaders("Content-Type", "Authorization");
            });
        });
        return services;
    }

    public static WebApplication UseAtmSwaggerEndpoint(this WebApplication app)
    {
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Web ATM v1");
        });
        return app;
    }

    public static WebApplication UseCientCors(this WebApplication app)
    {
        app.UseCors(corsPolicyName);
        return app;
    }
}
