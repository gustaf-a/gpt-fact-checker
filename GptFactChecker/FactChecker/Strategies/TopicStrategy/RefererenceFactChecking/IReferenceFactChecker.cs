using FactCheckingService.Models;
using Shared.Models;

namespace FactCheckingService.Strategies.TopicStrategy.RefererenceFactChecking;

public interface IReferenceFactChecker
{
    public Task<FactCheckResponse> FactCheck(Fact claim, List<Reference> relevantReferences);
    string GetVersionInfo();
}
