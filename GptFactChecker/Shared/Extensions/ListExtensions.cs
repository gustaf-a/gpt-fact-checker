﻿namespace Shared.Extensions;

public static class ListExtensions
{
    public static bool IsNullOrEmpty<T>(this List<T> list)
        => list is null || !list.Any();
}
