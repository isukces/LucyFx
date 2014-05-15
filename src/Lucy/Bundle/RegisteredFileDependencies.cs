using System.Collections.Generic;

namespace Lucy.Bundle
{
    public static class RegisteredFileDependencies
    {
		#region Constructors 

        static RegisteredFileDependencies()
        {
            DependenciesDictionary = new Dictionary<Filename, List<Filename>>();
        }

		#endregion Constructors 

		#region Static Methods 

		// Public Methods 

        public static void AddDependency(string file, params string[] dependencies)
        {
            List<Filename> list;
            if (!DependenciesDictionary.TryGetValue(file, out list))
                DependenciesDictionary[file] = list = new List<Filename>();
            foreach (var dep in dependencies)
                list.AddIfNotExists(dep);
        }
		// Private Methods 

        static void FillDependencies(List<Filename> files, Filename file)
        {
            List<Filename> tmp;
            if (DependenciesDictionary.TryGetValue(file, out tmp) && tmp != null && tmp.Count > 0)
                foreach (var depFile in tmp)
                {
                    FillDependencies(files, depFile);
                    files.AddIfNotExists(depFile);
                }
            files.AddIfNotExists(file);
        }
		// Internal Methods 

        internal static List<Filename> ResolveDependencies(List<Filename> filenames)
        {
            var resolvedDependencies = new List<Filename>();
            if (filenames == null || filenames.Count == 0)
                return resolvedDependencies;
            foreach (var file in filenames)
                FillDependencies(resolvedDependencies, file);
            return resolvedDependencies;
        }

		#endregion Static Methods 

		#region Static Fields 

        static readonly Dictionary<Filename, List<Filename>> DependenciesDictionary;

		#endregion Static Fields 
    }
}
