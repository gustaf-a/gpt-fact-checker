using Shared.Models;

namespace FactCheckingService.Strategies.GeneralStrategy.FactCheckPrompt;

public interface IGeneralFactCheckPrompt
{
    public Prompt GetPrompt(Fact fact);
}
