using FactCheckingService.Models;
using Shared.Extensions;

namespace FactCheckingService.Extensions;

internal static class ClaimsWithReferencesExtensions
{
    public static List<ClaimWithReferences> Merge(this List<ClaimWithReferences> list, List<ClaimWithReferences> listToMerge)
    {
        if (list is null)
            throw new ArgumentNullException(nameof(list));

        if (listToMerge is null)
            throw new ArgumentNullException(nameof(listToMerge));

        if (list.IsNullOrEmpty())
            return listToMerge;

        foreach (var item in listToMerge)
        {
            var matchingItem = list.FirstOrDefault(i => i.ClaimId.Equals(item.ClaimId));

            if (matchingItem is null)
            {
                list.Add(item);
                continue;
            }

            matchingItem.ReferenceIds.AddRange(item.ReferenceIds);
        }

        return list;
    }
}

