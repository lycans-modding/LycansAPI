using System;
using System.Collections.Generic;
using System.Linq;

namespace LycansAPI.Core.Extensions;

public static class EnumerableExtension
{
    public static void ForEach<T>(this IEnumerable<T>? list, Action<T> action)
    {
        list.ToList().ForEach(action);
    }

    public static void TryForEach<T>(this IEnumerable<T>? list, Action<T> action, IDictionary<T, Exception?>? exceptions = null)
    {
        list.ForEach(e =>
        {
            try
            {
                action(e);
            }
            catch (Exception ex)
            {
                exceptions?.Add(e, ex);
            }
        });
    }
}