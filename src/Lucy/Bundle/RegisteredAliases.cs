using System;
using System.Collections.Generic;

namespace Lucy.Bundle
{
    internal static class RegisteredAliases
    {
        #region Static Methods

        // Internal Methods 

        internal static Filename? GetFileByAlias(AliasBundleType alias)
        {
            Filename filename;
            return ByAlias.TryGetValue(alias, out filename)
                ? filename
                : null;
        }

        internal static Alias GetOrCreateAlias(Filename fileName)
        {
            fileName.Check();
            AliasBundleType aliasBundleType;
            if (ByFilename.TryGetValue(fileName, out aliasBundleType))
                return aliasBundleType.Alias;
            var shortName = fileName.ShortNameWithoutExtension;
            shortName = shortName.Replace(".", "-");
            return shortName;
        }

        internal static void RegisterAlias(Filename fileName, Alias alias)
        {
            fileName.Check();
            alias.Check();
            var type = BundleModule.GetFileTypeByExtension(fileName);
            if (!type.HasValue)
                throw new Exception("Unable to get file type from " + fileName);
            AliasBundleType aliasBundleType;
            if (ByFilename.TryGetValue(fileName, out aliasBundleType))
                if (!aliasBundleType.Alias.Equals(alias))
                    throw new Exception(
                        string.Format("Unable to register alias '{0}' for file '{1}'. It already has alias '{2}'.",
                            alias, fileName, aliasBundleType));
            aliasBundleType = new AliasBundleType(alias, type.Value);
            Filename existingFileName;
            if (ByAlias.TryGetValue(aliasBundleType, out existingFileName))
                if (!existingFileName.Equals(fileName))
                    throw new Exception(
                        string.Format(
                            "Unable to register alias '{0}' for file '{1}' because it has already been registered for file '{2}'.",
                            alias, fileName, existingFileName));
            ByAlias[aliasBundleType] = fileName;
            ByFilename[fileName] = aliasBundleType;
        }

        #endregion Static Methods

        #region Static Fields

        static readonly Dictionary<AliasBundleType, Filename> ByAlias = new Dictionary<AliasBundleType, Filename>();
        static readonly Dictionary<Filename, AliasBundleType> ByFilename = new Dictionary<Filename, AliasBundleType>();

        #endregion Static Fields
    }
}
