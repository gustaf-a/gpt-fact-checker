using GptFactCheckerApi.Model;
using Shared.Models;

namespace GptFactCheckerApi.Services;

public interface IFactCheckingService
{
    Task<List<FactCheckResponse>> CheckFacts(List<ClaimDto> facts);
}
