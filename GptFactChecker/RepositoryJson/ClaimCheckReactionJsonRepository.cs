using Microsoft.Extensions.Options;
using Shared.Configuration;
using Shared.Models;
using Shared.Repository;

namespace RepositoryJson;

public class ClaimCheckReactionRepositoryJson : RepositoryBaseJson<ClaimCheckReaction>, IClaimCheckReactionRepository
{
    private const string FileName = "ClaimCheckReactions.json";

    public ClaimCheckReactionRepositoryJson(IOptions<RepositoryJsonOptions> options)
            : base(options, FileName) { }
}
