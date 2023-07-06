using FactCheckingService;
using FactCheckingService.FactCheckers;
using FactCheckingService.FactCheckers.ClimateStrategy;
using FactCheckingService.FactCheckers.ClimateStrategy.FactCheckWithData;
using FactCheckingService.FactCheckers.ClimateStrategy.TopicIdentification;
using FactCheckingService.FactCheckers.GeneralStrategy;
using FactCheckingService.FactCheckers.GeneralStrategy.FactCheckPrompt;
using GptFactCheckerApi.Repository;
using GptFactCheckerApi.Repository.JsonRepo;
using GptFactCheckerApi.Services;
using Shared.Configuration;
using Shared.GptClient;

namespace GptFactCheckerApi;

public class Startup
{
    private readonly ConfigurationManager _configurationManager;

    public Startup(ConfigurationManager configurationManager)
    {
        _configurationManager = configurationManager;
    }

    public void ConfigureMyServices(IServiceCollection services)
    {
        services.Configure<OpenAiOptions>(_configurationManager.GetSection(OpenAiOptions.OpenAi));
        services.Configure<FactCheckerOptions>(_configurationManager.GetSection(FactCheckerOptions.FactChecker));

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

        services.AddHttpClient();

        services.AddSingleton<IGptClient, GptClient>();
        services.AddSingleton<IGptResponseParser, GptResponseParser>();

        services.AddSingleton<IFactCheckingService, Services.FactCheckingService>();

        services.AddSingleton<IFactChecker, FactChecker>();

        services.AddSingleton<IFactCheckerStrategy, ClimateFactCheckerWithReferencesStrategy>();
        services.AddSingleton<ITopicIdentifier, TopicIdentifier>();
        services.AddSingleton<ITopicIdentificationPrompt, TopicIdentificationPrompt>();
        services.AddSingleton<IClimateFactCheckerWithData, ClimateFactCheckerWithData>();
        services.AddSingleton<IClimateFactCheckWithDataPrompt, ClimateFactCheckWithDataPrompt>();

        services.AddSingleton<IFactCheckerStrategy, FactCheckerStrategyGeneral>();
        services.AddSingleton<IGeneralFactCheckPrompt, GeneralFactCheckPrompt>();

    }
}
