using System.Collections.Generic;
using System.Threading;

namespace Lucy.Bundle
{
    public static class RegisteredBundles
    {
        #region Constructors

        static RegisteredBundles()
        {
            ExplicitBundlesByName = new Dictionary<string, Bundle>(BundleSettings.IgnoreCase);
            ExplicitBundlesByPath = new Dictionary<string, Bundle>(BundleSettings.IgnoreCase);
        }

        #endregion Constructors

        #region Static Methods

        // Internal Methods 

        internal static Bundle AddBundle(Bundle bundle)
        {
            Lock.EnterWriteLock();
            try
            {
                ExplicitBundlesByName[bundle.Name] = bundle;
                ExplicitBundlesByPath[bundle.VirtualPath] = bundle;
            }
            finally
            {
                Lock.ExitWriteLock();
            }
            return bundle;
        }

        internal static Bundle GetBundleByName(string path)
        {
            Lock.EnterReadLock();
            try
            {
                Bundle result;
                return ExplicitBundlesByName.TryGetValue(path, out result) ? result : null;
            }
            finally
            {
                Lock.ExitReadLock();
            }
        }

        internal static Bundle GetBundleByPath(string path)
        {
            Lock.EnterReadLock();
            try
            {
                Bundle result;
                return ExplicitBundlesByPath.TryGetValue(path, out result) ? result : null;
            }
            finally
            {
                Lock.ExitReadLock();
            }
        }

        internal static IEnumerable<Bundle> GetExplicitBundles()
        {
            Lock.EnterReadLock();
            try
            {
                return ExplicitBundlesByName.Values;
            }
            finally
            {
                Lock.ExitReadLock();
            }

        }

        #endregion Static Methods

        #region Static Fields

        static readonly Dictionary<string, Bundle> ExplicitBundlesByName;
        static readonly Dictionary<string, Bundle> ExplicitBundlesByPath;
        static readonly ReaderWriterLockSlim Lock = new ReaderWriterLockSlim();

        #endregion Static Fields
    }
}
