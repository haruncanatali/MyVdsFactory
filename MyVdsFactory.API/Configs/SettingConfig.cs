using System.Globalization;
using MyVdsFactory.Application.Common.Managers;
using MyVdsFactory.Domain.Addition;

namespace MyVdsFactory.API.Configs;

public static class SettingsConfig
{
    public static IServiceCollection AddSettingsConfig(this IServiceCollection services, IConfiguration configuration)
    {
        System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("tr-TR");
        
        services.AddTransient<ClaimManager>();
        services.AddTransient<TokenManager>();
        services.Configure<TokenSettings>(configuration.GetSection("TokenSetting"));

        return services;
    }
}