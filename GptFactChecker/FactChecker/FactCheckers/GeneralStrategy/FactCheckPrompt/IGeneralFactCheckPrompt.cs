using Shared.Models;

namespace FactCheckingService.FactCheckers.GeneralStrategy.FactCheckPrompt;

public interface IGeneralFactCheckPrompt
{
    public Task<Prompt> GetPrompt(Fact fact);
}
