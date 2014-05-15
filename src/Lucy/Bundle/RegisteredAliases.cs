using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lucy.Bundle
{
    internal static class RegisteredAliases
    {
        #region Static Methods

        // Public Methods 

        internal static void RegisterAlias(Filename fileName, Alias alias)
        {
            fileName.Check();
            alias.Check();

            {
                Alias existingAlias;
                if (byFilename.TryGetValue(fileName, out existingAlias))
                    if (!existingAlias.Equals(alias))
                        throw new Exception(
                            string.Format("Unable to register alias '{0}' for file '{1}'. It already has alias '{2}'.",
                            alias, fileName, existingAlias));
            }
            {
                Filename existingFileName;
                if (byAlias.TryGetValue(alias, out existingFileName))
                    if (!existingFileName.Equals(fileName))
                        throw new Exception(
                            string.Format("Unable to register alias '{0}' for file '{1}' because it has already been registered for file '{2}'.",
                            alias, fileName, existingFileName));
            }
            byAlias[alias] = fileName;
            byFilename[fileName] = alias;
        }
        // Internal Methods 

        internal static Alias GetOrCreateAlias(Filename fileName)
        {
            fileName.Check();
            Alias alias;
            if (byFilename.TryGetValue(fileName, out alias))
                return alias;
            var shortName = fileName.Name.Split('/').Last().ToLower();
            if (shortName.EndsWith(".js"))
                shortName = shortName.Substring(0, shortName.Length - 3);
            else if (shortName.EndsWith(".css"))
                shortName = shortName.Substring(0, shortName.Length - 4);
            shortName = shortName.Replace(".", "-");
            return shortName;
        }

        #endregion Static Methods

        #region Static Fields

        static readonly Dictionary<Alias, Filename> byAlias = new Dictionary<Alias, Filename>();
        static readonly Dictionary<Filename, Alias> byFilename = new Dictionary<Filename, Alias>();

        #endregion Static Fields

        internal static Filename? GetFileByAlias(Alias alias)
        {
            Filename fn;
            if (byAlias.TryGetValue(alias, out fn))
                return fn;
            return null;
        }
    }
}
