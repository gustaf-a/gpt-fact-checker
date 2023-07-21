using Shared.Models;

namespace FactCheckingService.Strategies.GeneralStrategy.FactCheckPrompt;

public interface IGeneralFactCheckPrompt
{
    public Task<Prompt> GetPrompt(Fact fact);
}
