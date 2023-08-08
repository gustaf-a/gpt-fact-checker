using Shared.Models;

namespace FactCheckingService.Strategies.TopicStrategy.ReferenceMatching;

public interface IReferenceMatcherPromptDirector
{
    public Prompt BuildPrompt(List<Fact> factsPart, List<Reference> referencesPart);
    string GetVersionInfo();
}
