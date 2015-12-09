using System.Collections.Generic;

namespace Borogove
{
    public static class ListExtensions
    {
        public static void AddOrCreateList<T>(this List<T> list, T toAdd, bool allowRepeats = false)
        {
            list = list ?? new List<T>();
            if (allowRepeats || !list.Contains(toAdd))
            {
                list.Add(toAdd);
            }
        }
    }
}
