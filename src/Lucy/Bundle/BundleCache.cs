using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Lucy.Bundle
{
    static class BundleCache
    {
        #region Static Methods

        // Private Methods 

        private static string ComputeKey(BundleTypes bundleType, IEnumerable<FileInfo> files, bool minified)
        {
            var code = 2 * (int)bundleType + (minified ? 1 : 0);
            var key = code + "*" + string.Join("*", files.Select(i => i.FullName.ToLowerInvariant()));
            return key;
        }
        // Internal Methods 

        internal static BundleCacheItem Store(BundleTypes bundleType, FileInfo[] files, bool minified, String content)
        {
            var key = ComputeKey(bundleType, files, minified);
            var result = Cache.GetOrAdd(key, _ => new BundleCacheItem(files));
            result.Content = content;
            return result;
        }

        internal static BundleCacheItem TryGet(BundleTypes bundleType, IEnumerable<FileInfo> files, bool minified)
        {
            var key = ComputeKey(bundleType, files, minified);
            BundleCacheItem result;
            if (!Cache.TryGetValue(key, out result)) return null;
            return result.NeedRebuild ? null : result;
        }

        #endregion Static Methods

        #region Static Fields

        static readonly ConcurrentDictionary<string, BundleCacheItem> Cache = new ConcurrentDictionary<string, BundleCacheItem>(StringComparer.OrdinalIgnoreCase);

        #endregion Static Fields
    }
}
