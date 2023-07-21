using Microsoft.Extensions.Options;
using Shared.Configuration;
using Shared.Models;
using Shared.Repository;

namespace RepositoryJson;

public class ClaimRepositoryJson : RepositoryBaseJson<Fact>, IClaimRepository
{
    private const string FileName = "Claims.json";

    public ClaimRepositoryJson(IOptions<RepositoryJsonOptions> options)
        : base(options, FileName) { }
}
