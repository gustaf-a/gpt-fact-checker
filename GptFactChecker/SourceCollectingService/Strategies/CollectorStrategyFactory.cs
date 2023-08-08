using Shared.Models;
using SourceCollectingService.Strategies.MediaCollectingStrategy;

namespace SourceCollectingService.Strategies;

public class CollectorStrategyFactory : ICollectorStrategyFactory
{
    private readonly IServiceProvider _serviceProvider;

    public CollectorStrategyFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public ICollectorStrategy CreateStrategy(string sourceType)
    {
        if(string.IsNullOrWhiteSpace(sourceType))
            throw new ArgumentNullException(nameof(sourceType));

        var collectingStrategy = CreateCollectingStrategy(sourceType);

        if (collectingStrategy is null)
            throw new Exception($"Failed to create strategy for source type {sourceType}");

        return collectingStrategy;
    }

    private ICollectorStrategy? CreateCollectingStrategy(string sourceType)
    {
        return sourceType.ToLower() switch
        {
            SourceTypes.Video => (IMediaCollectorStrategy)_serviceProvider.GetService(typeof(IMediaCollectorStrategy)),
            SourceTypes.Podcast => (IMediaCollectorStrategy)_serviceProvider.GetService(typeof(IMediaCollectorStrategy)),
            //TODO add Text
            _ => throw new NotImplementedException($"Failed to find collecting strategy for source type {sourceType}.")
        };
    }
}
