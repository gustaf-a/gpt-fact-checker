using GptFactCheckerApi.Model;
using Shared.Models;

namespace GptFactCheckerApi.Services;

public interface IFactCheckService
{
    Task<BackendResponse<List<ClaimCheckResultsDto>>> CheckFacts(List<string> claimIds);
}
