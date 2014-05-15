using System;
using System.Collections.Generic;
using System.Linq;

namespace Lucy.Bundle
{

    public class Bundle
    {
        #region Constructors

        internal Bundle(string name, BundleTypes bundleType, IEnumerable<Filename> files)
        {
            Name = name;
            Files = new List<Filename>();
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
            var autoName = string.Join(BundleSettings.NameSeparator.ToString(), expanded);
            return autoName;
        }

        #endregion Static Methods

        #region Methods

        // Public Methods 

        public Bundle Include(Filename fileName)
        {
            fileName.Check();
            Files.AddIfNotExists(fileName);
            return this;
        }

        public Bundle Include(Filename fileName, Alias alias)
        {
            fileName.Check();
            Files.AddIfNotExists(fileName);
            RegisteredAliases.RegisterAlias(fileName, alias);
            return this;
        }

        #endregion Methods

        #region Properties

        public BundleTypes BundleType { get; private set; }

        public List<Filename> Files { get; private set; }

        public List<Filename> FilesWithDependencies
        {
            get
            {
                return RegisteredFileDependencies.ResolveDependencies(Files);
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
                string autoName = IsDynamic ? DynamicBundleNameFromFiles(FilesWithDependencies) : Name;
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
    }
}
