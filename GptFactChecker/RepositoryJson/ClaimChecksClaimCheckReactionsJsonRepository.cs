using Microsoft.Extensions.Options;
using Shared.Configuration;
using Shared.Models;
using Shared.Repository;

namespace RepositoryJson;

public class ClaimChecksClaimCheckReactionsRepositoryJson : IClaimChecksClaimCheckReactionsRepository
{
    private const string FileName = "ClaimChecks_ClaimCheckReactions.json";

    private readonly string JsonFilePath;

    private readonly IParentChildrenHolderRepository _parentChildrenHolderRepositoryJson;

    public ClaimChecksClaimCheckReactionsRepositoryJson(IOptions<RepositoryJsonOptions> options)
        {
            JsonFilePath = options.Value.DataFolder + FileName;

            _parentChildrenHolderRepositoryJson = new ParentChildrenHolderRepositoryJson(JsonFilePath);
        }

        public async Task<bool> AddClaimCheckReactionForClaimCheck(string claimCheckId, ClaimCheckReaction claimCheckReaction)
        {
            return await _parentChildrenHolderRepositoryJson.AddChildrenForParent(claimCheckId, new List<string> { claimCheckReaction.Id });
        }

        public async Task<List<string>> GetClaimCheckReactionsForClaimCheck(string claimCheckId)
        {
            return await _parentChildrenHolderRepositoryJson.GetChildrenForParent(claimCheckId);
        }

        public async Task<bool> RemoveClaimCheck(string claimCheckId)
        {
            return await _parentChildrenHolderRepositoryJson.RemoveParent(claimCheckId);
        }

        public async Task<bool> RemoveClaimCheckReactions(List<string> claimCheckReactionIds)
        {
            return await _parentChildrenHolderRepositoryJson.RemoveChildren(claimCheckReactionIds);
        }
    }
