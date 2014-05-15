using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lucy
{
    public static class ListExtensions
    {
        public static void AddIfNotExistsCaseInsensitive(this List<string> list, string item)
        {
            if (!list.ContainsCaseInsensitive(item))
                list.Add(item);
        }
        public static bool ContainsCaseInsensitive(this List<string> list, string item)
        {
            var idx = list.FindIndex(x => x.Equals(item, StringComparison.OrdinalIgnoreCase));
            return idx >= 0;
        }

        public static void AddIfNotExists<T>(this List<T> list, T item)
        {
            if (!list.Contains(item))
                list.Add(item);
        }
    }
}
