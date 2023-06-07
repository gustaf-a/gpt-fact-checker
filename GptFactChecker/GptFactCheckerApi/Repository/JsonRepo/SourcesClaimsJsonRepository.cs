﻿using Claim = GptFactCheckerApi.Model.Claim;

namespace GptFactCheckerApi.Repository.JsonRepo;

public class SourcesClaimsJsonRepository : ISourcesClaimsRepository
{
    private const string JsonFilePath = @"C:\PrivateRepos\gpt-fact-checker\GptFactChecker\GptFactCheckerApi\MockData\Sources_Claims.json";

    private readonly IParentChildrenHolderRepository _parentChildrenHolderJsonRepository;

    public SourcesClaimsJsonRepository()
    {
        _parentChildrenHolderJsonRepository = new ParentChildrenHolderJsonRepository(JsonFilePath);
    }

    public async Task<bool> AddClaimsForSource(string sourceId, List<Claim> claims)
    {
        var claimsIds = claims.Select(c => c.Id).ToList();

        return await _parentChildrenHolderJsonRepository.AddChildrenForParent(sourceId, claimsIds);
    }

    public async Task<List<string>> GetClaimsForSource(string sourceId)
    {
        return await _parentChildrenHolderJsonRepository.GetChildrenForParent(sourceId);
    }

    public async Task<bool> RemoveClaims(List<string> claimIds)
    {
        return await _parentChildrenHolderJsonRepository.RemoveChildren(claimIds);
    }

    public async Task<bool> RemoveSource(string sourceId)
    {
        return await _parentChildrenHolderJsonRepository.RemoveParent(sourceId);
    }
}
