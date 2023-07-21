using Microsoft.Extensions.Options;
using Shared.Configuration;
using Shared.Models;
using Shared.Repository;

namespace RepositoryJson;

public class ClaimCheckRepositoryJson : RepositoryBaseJson<ClaimCheck>, IClaimCheckRepository
{
    private const string FileName = "ClaimChecks.json";

    public ClaimCheckRepositoryJson(IOptions<RepositoryJsonOptions> options)
            : base(options, FileName) { }
}
