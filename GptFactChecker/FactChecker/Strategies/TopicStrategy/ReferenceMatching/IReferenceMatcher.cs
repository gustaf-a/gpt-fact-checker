﻿using FactCheckingService.Models;
using Shared.Models;

namespace FactCheckingService.Strategies.TopicStrategy.ReferenceMatching;

public interface IReferenceMatcher
{
    string GetVersionInfo();
    public Task<List<ClaimWithReferences>> MatchFactsWithReferences(List<Fact> facts, List<Reference> references);
}
