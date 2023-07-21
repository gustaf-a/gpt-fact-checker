using Microsoft.Extensions.Options;
using Shared.Configuration;
using Shared.Models;
using Shared.Repository;

namespace RepositoryJson;

public class SourceRepositoryJson : RepositoryBaseJson<Source>, ISourceRepository
{
    private const string FileName = "Sources.json";

    public SourceRepositoryJson(IOptions<RepositoryJsonOptions> options)
            : base(options, FileName) { }
}
