using FactCheckingService;
using FactCheckingService.Services;
using FactCheckingService.Strategies;
using FactCheckingService.Strategies.ClimateStrategy;
using FactCheckingService.Strategies.ClimateStrategy.FactCheckWithData;
using FactCheckingService.Strategies.ClimateStrategy.TopicIdentification;
using FactCheckingService.Strategies.GeneralStrategy;
using FactCheckingService.Strategies.GeneralStrategy.FactCheckPrompt;
using FactCheckingService.Strategies.TopicStrategy;
using FactCheckingService.Strategies.TopicStrategy.ReferenceMatching;
using FactCheckingService.Strategies.TopicStrategy.RefererenceFactChecking;
using FactCheckingService.Utils;
using FactExtractionService;
using FactExtractionService.FactExtractors;
using FactExtractionService.FactExtractors.FunctionCallingStrategy;
using FactExtractionService.FactExtractors.FunctionCallingStrategy.FactExtractionPrompt;
using FactExtractionService.Utils;
using GptFactCheckerApi.Services;
using RepositoryJson;
using Shared.Configuration;
using Shared.GptClient;
using Shared.Prompts;
using Shared.Repository;
using System.ComponentModel.Design;

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
        services.Configure<RepositoryJsonOptions>(_configurationManager.GetSection(RepositoryJsonOptions.RepositoryJson));
        
        services.AddSingleton<ISourceRepository, SourceRepositoryJson>();
        services.AddSingleton<IClaimRepository, ClaimRepositoryJson>();
        services.AddSingleton<IClaimCheckRepository, ClaimCheckRepositoryJson>();
        services.AddSingleton<IClaimCheckReactionRepository, ClaimCheckReactionRepositoryJson>();
        services.AddSingleton<ITopicRepository, TopicsRepositoryJson>();

        services.AddSingleton<ISourcesClaimsRepository, SourcesClaimsRepositoryJson>();
        services.AddSingleton<IClaimsClaimChecksRepository, ClaimsClaimCheckRepositoryJson>();
        services.AddSingleton<IClaimChecksClaimCheckReactionsRepository, ClaimChecksClaimCheckReactionsRepositoryJson>();

        services.AddSingleton<ISourceService, SourceService>();
        services.AddSingleton<IClaimService, ClaimService>();
        services.AddSingleton<IClaimCheckService, ClaimCheckService>();
        services.AddSingleton<IClaimCheckReactionService, ClaimCheckReactionService>();
        services.AddSingleton<ITopicService, TopicService>();


        services.AddSingleton<IPromptBuilder, PromptBuilder>();

        services.AddHttpClient();

        services.AddSingleton<IGptClient, GptClient>();
        services.AddSingleton<IGptResponseParser, GptResponseParser>();

        ConfigureFactCheckingServices(services);

        ConfigureFactExtractionServices(services);
    }

    private void ConfigureFactCheckingServices(IServiceCollection services)
    {
        services.Configure<FactCheckerOptions>(_configurationManager.GetSection(FactCheckerOptions.FactChecker));

        services.Configure<ReferenceMatchingOptions>(_configurationManager.GetSection(ReferenceMatchingOptions.ReferenceMatching));
        services.Configure<ReferenceFactCheckingOptions>(_configurationManager.GetSection(ReferenceFactCheckingOptions.ReferenceFactChecking));

        services.AddSingleton<IFactCheckService, FactCheckService>();

        services.AddSingleton<IFactChecker, FactChecker>();
        services.AddSingleton<ITagMatcher, TagMatcher>();

        services.AddSingleton<IFactCheckerStrategy, TopicFactCheckerStrategy>();
        services.AddSingleton<IReferenceMatcher, ReferenceMatcher>();
        services.AddSingleton<IReferenceMatcherPromptDirector, ReferenceMatcherPromptDirector>();
        services.AddSingleton<IReferenceFactChecker, ReferenceFactChecker>();
        services.AddSingleton<IReferenceFactCheckerPromptDirector, ReferenceFactCheckerPromptDirector>();

        services.AddSingleton<IFactCheckerStrategy, ClimateFactCheckerWithReferencesStrategy>();
        services.AddSingleton<ITopicIdentifier, TopicIdentifier>();
        services.AddSingleton<ITopicIdentificationPrompt, TopicIdentificationPrompt>();
        services.AddSingleton<IClimateFactCheckerWithData, ClimateFactCheckerWithData>();
        services.AddSingleton<IClimateFactCheckWithDataPrompt, ClimateFactCheckWithDataPrompt>();

        services.AddSingleton<IFactCheckerStrategy, FactCheckerStrategyGeneral>();
        services.AddSingleton<IGeneralFactCheckPrompt, GeneralFactCheckPrompt>();
    }

    private void ConfigureFactExtractionServices(IServiceCollection services)
    {
        services.Configure<FactExtractionOptions>(_configurationManager.GetSection(FactExtractionOptions.FactExtraction));

        services.AddSingleton<IFactExtractor, FactExtractor>();
        services.AddSingleton<IFactExtractorService, FactExtractorService>();
        services.AddSingleton<IFactExtractionStrategy, FactExtractionWithFunctionCallingStrategy>();
        services.AddSingleton<IFactExtractionFunctionCallingPrompt, FactExtractionFunctionCallingPrompt>();

        services.AddSingleton<ISourceSplitter, SourceSplitter>();
    }
}
