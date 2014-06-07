using System;
using System.Collections.Generic;
using System.Threading;

namespace Lucy.Bundle
{
    internal static class RegisteredAliases
    {
        #region Static Methods

        // Internal Methods 

        internal static Filename? GetFileByAlias(AliasBundleType alias)
        {
            Lock.EnterReadLock();
            try
            {
                Filename filename;
                return ByAlias.TryGetValue(alias, out filename)
                    ? filename
                    : null;
            }
            finally
            {
                Lock.ExitReadLock();
            }
        }

        internal static Alias GetOrCreateAlias(Filename fileName)
        {
            fileName.Check();
            Lock.EnterReadLock();
            try
            {
                AliasBundleType aliasBundleType;
                if (ByFilename.TryGetValue(fileName, out aliasBundleType))
                    return aliasBundleType.Alias;
            }
            finally
            {
                Lock.ExitReadLock();
            }
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

            Lock.EnterUpgradeableReadLock();
            try
            {
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
                Lock.EnterWriteLock();
                try
                {
                    ByAlias[aliasBundleType] = fileName;
                    ByFilename[fileName] = aliasBundleType;
                }
                finally
                {
                    Lock.ExitWriteLock();
                }
            }
            finally
            {
                Lock.ExitUpgradeableReadLock();
            }
        }

        #endregion Static Methods

        #region Static Fields

        static readonly Dictionary<AliasBundleType, Filename> ByAlias = new Dictionary<AliasBundleType, Filename>();
        static readonly Dictionary<Filename, AliasBundleType> ByFilename = new Dictionary<Filename, AliasBundleType>();
        static readonly ReaderWriterLockSlim Lock = new ReaderWriterLockSlim();

        #endregion Static Fields
    }
}
