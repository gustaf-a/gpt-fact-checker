using Shared.Models;

namespace FactCheckingService.Strategies.ClimateStrategy.FactCheckWithData;

public interface IClimateFactCheckWithDataPrompt
{
    public Prompt GetPrompt(Fact fact, List<ArgumentData> relevantArguments);
}
