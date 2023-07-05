using Shared.Models;

namespace FactCheckingService.FactCheckers.ClimateStrategy.FactCheckWithData;

public interface IClimateFactCheckWithDataPrompt
{
    public Task<Prompt> GetPrompt(Fact fact, List<ArgumentData> relevantArguments);
}
