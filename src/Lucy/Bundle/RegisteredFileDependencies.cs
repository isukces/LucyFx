using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Lucy.Bundle
{
    public static class RegisteredFileDependencies
    {
		#region Constructors 

        static RegisteredFileDependencies()
        {
            DependenciesDictionary = new ConcurrentDictionary<Filename, ConcurrentContainer<Filename>>();
        }

		#endregion Constructors 

		#region Static Methods 

		// Public Methods 

        public static void AddDependency(string file, params string[] dependencies)
        {
            var list = DependenciesDictionary.GetOrAdd(file, key => new ConcurrentContainer<Filename>());
            foreach (var dep in dependencies)
                list.AddIfNotExists(dep);
        }
		// Private Methods 

        static void FillDependencies(ConcurrentContainer<Filename> files, Filename file)
        {
            ConcurrentContainer<Filename> myDependencies;
            if (DependenciesDictionary.TryGetValue(file, out myDependencies) && myDependencies != null)
                myDependencies.Iterate(depFile =>
                {
                    FillDependencies(files, depFile);
                    files.AddIfNotExists(depFile);
                });
            files.AddIfNotExists(file);
        }
		// Internal Methods 

        internal static ConcurrentContainer<Filename> ResolveDependencies(ConcurrentContainer<Filename> filenames)
        {
            var resolvedDependencies = new ConcurrentContainer<Filename>();
            if (filenames != null)
                filenames.Iterate(file => FillDependencies(resolvedDependencies, file));
            return resolvedDependencies;
        }

        internal static ConcurrentContainer<Filename> ResolveDependencies(List<Filename> filenames)
        {
            var resolvedDependencies = new ConcurrentContainer<Filename>();
            if (filenames == null || filenames.Count <= 0) 
                return resolvedDependencies;
            foreach (var filename in filenames)
                FillDependencies(resolvedDependencies, filename);
            return resolvedDependencies;
        }

		#endregion Static Methods 

		#region Static Fields 

        static readonly ConcurrentDictionary<Filename, ConcurrentContainer<Filename>> DependenciesDictionary;

		#endregion Static Fields 
    }
}
