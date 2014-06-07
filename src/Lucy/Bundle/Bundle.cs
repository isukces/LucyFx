using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Lucy.Bundle
{

    public class Bundle
    {
        #region Constructors

        internal Bundle(string name, BundleTypes bundleType, IEnumerable<Filename> files)
        {
            Name = name;
            _files = new ConcurrentContainer<Filename>();
            if (files != null)
                foreach (var i in files)
                    Include(i);
            BundleType = bundleType;
        }

        #endregion Constructors

        #region Static Methods

        // Internal Methods 

        internal static string DynamicBundleNameFromFiles(IEnumerable<Filename> filesWithDependencies)
        {
            var expanded = from fileName in filesWithDependencies
                           select RegisteredAliases.GetOrCreateAlias(fileName);
            var autoName = string.Join(BundleSettings.NameSeparator.ToString(CultureInfo.InvariantCulture), expanded);
            return autoName;
        }

        #endregion Static Methods

        #region Fields

        private readonly ConcurrentContainer<Filename> _files;

        #endregion Fields

        #region Properties

        public BundleTypes BundleType { get; private set; }

        public List<Filename> FilesWithDependencies
        {
            get
            {
                return RegisteredFileDependencies.ResolveDependencies(_files).ToList();
            }
        }

        public bool IsDynamic { get; internal set; }

        public string Name { get; private set; }

        /// <summary>
        /// Tilda prefixed path
        /// </summary>
        public string VirtualPath
        {
            get
            {
                if (!IsDynamic && Name.StartsWith("~/"))
                    return Name;
                var autoName = IsDynamic ? DynamicBundleNameFromFiles(FilesWithDependencies) : Name;
                switch (BundleType)
                {
                    case BundleTypes.Script:
                        return BundleSettings.Js.MakePath(autoName);
                    case BundleTypes.StyleSheet:
                        return BundleSettings.Css.MakePath(autoName);
                }
                throw new NotImplementedException();
            }
        }

        #endregion Properties

        public Bundle Include(Filename fileName, Alias alias = default(Alias))
        {
            if (alias.IsEmpty)
                alias = RegisteredAliases.GetOrCreateAlias(fileName);
            fileName.Check();
            _files.AddIfNotExists(fileName);
            RegisteredAliases.RegisterAlias(fileName, alias);
            return this;
        }
    }
}
