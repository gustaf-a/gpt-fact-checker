using GptFactCheckerApi.Repository;
using GptFactCheckerApi.Services;

namespace GptFactCheckerApi;

public static class Startup
{
    public static void ConfigureMyServices(this IServiceCollection services)
    {
        services.AddSingleton<IClaimRepository, ClaimJsonRepository>();
        services.AddSingleton<ISourceRepository, SourceRepository>();
        services.AddSingleton<ISourcesClaimsRepository, SourcesClaimsRepository>();

        services.AddSingleton<IClaimService, ClaimService>();
        services.AddSingleton<ISourceService, SourceService>();
    }
}
