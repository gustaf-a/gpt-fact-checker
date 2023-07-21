using Shared.Models;

namespace FactCheckingService.Strategies.ClimateStrategy.FactCheckWithData;

public interface IClimateFactCheckWithDataPrompt
{
    public Task<Prompt> GetPrompt(Fact fact, List<ArgumentData> relevantArguments);
}
