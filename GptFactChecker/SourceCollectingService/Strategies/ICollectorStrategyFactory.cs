namespace SourceCollectingService.Strategies;

public interface ICollectorStrategyFactory
{
    public ICollectorStrategy CreateStrategy(string sourceType);
}
