using System.Collections.Generic;

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
            //foreach (var i in files)
            //    GetOrRegister(i);
            //var bf = new Bundle(name, type, files);
            ExplicitBundlesByName[bundle.Name] = bundle;
            ExplicitBundlesByPath[bundle.VirtualPath] = bundle;
            return bundle;
        }

        internal static Bundle GetBundleByName(string path)
        {
            Bundle result;
            return ExplicitBundlesByName.TryGetValue(path, out result) ? result : null;
        }

        internal static Bundle GetBundleByPath(string path)
        {
            Bundle result;
            return ExplicitBundlesByPath.TryGetValue(path, out result) ? result : null;
        }

        internal static IEnumerable<Bundle> GetExplicitBundles()
        {
            return ExplicitBundlesByName.Values;
        }

        #endregion Static Methods

        #region Static Fields

        static readonly Dictionary<string, Bundle> ExplicitBundlesByName;
        static readonly Dictionary<string, Bundle> ExplicitBundlesByPath;

        #endregion Static Fields
    }
}
