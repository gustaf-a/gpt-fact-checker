using Shared.Models;

namespace FactCheckingService.Strategies.TopicStrategy.RefererenceFactChecking;

public interface IReferenceFactCheckerPromptDirector
{
    Prompt BuildPrompt(Fact claim, List<Reference> relevantReferences);
}
