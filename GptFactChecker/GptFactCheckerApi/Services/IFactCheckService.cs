using GptFactCheckerApi.Model;
using Shared.Models;

namespace GptFactCheckerApi.Services;

public interface IFactCheckService
{
    Task<List<FactCheckResponse>> CheckFacts(List<ClaimDto> facts);
}
