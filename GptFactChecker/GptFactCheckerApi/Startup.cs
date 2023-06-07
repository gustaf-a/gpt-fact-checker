using GptFactCheckerApi.Repository;
using GptFactCheckerApi.Repository.JsonRepo;
using GptFactCheckerApi.Services;

namespace GptFactCheckerApi;

public static class Startup
{
    public static void ConfigureMyServices(this IServiceCollection services)
    {
        services.AddSingleton<ISourceRepository, SourceJsonRepository>();
        services.AddSingleton<IClaimRepository, ClaimJsonRepository>();
        services.AddSingleton<IClaimCheckRepository, ClaimCheckJsonRepository>();
        services.AddSingleton<IClaimCheckReactionRepository, ClaimCheckReactionJsonRepository>();

        services.AddSingleton<ISourcesClaimsRepository, SourcesClaimsJsonRepository>();
        services.AddSingleton<IClaimsClaimChecksRepository, ClaimsClaimCheckJsonRepository>();
        services.AddSingleton<IClaimChecksClaimCheckReactionsRepository, ClaimChecksClaimCheckReactionsJsonRepository>();

        services.AddSingleton<ISourceService, SourceService>();
        services.AddSingleton<IClaimService, ClaimService>();
        services.AddSingleton<IClaimCheckService, ClaimCheckService>();
        services.AddSingleton<IClaimCheckReactionService, ClaimCheckReactionService>();
    }
}
