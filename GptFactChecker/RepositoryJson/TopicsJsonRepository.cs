using Microsoft.Extensions.Options;
using Shared.Configuration;
using Shared.Models;
using Shared.Repository;

namespace RepositoryJson;

public class TopicsRepositoryJson : RepositoryBaseJson<Topic>, ITopicRepository
{
    private const string FileName = "Topics.json";

    public TopicsRepositoryJson(IOptions<RepositoryJsonOptions> options)
        : base(options, FileName) { }
}
