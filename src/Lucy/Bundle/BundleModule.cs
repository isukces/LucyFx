using Nancy;
using Nancy.Responses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Lucy.Bundle
{
    public class BundleModule : NancyModule
    {
		#region Constructors 

        public BundleModule()
        {
            // static bundles
            {
                var names = from bundle in RegisteredBundles.GetExplicitBundles()
                            where !bundle.IsDynamic
                            select bundle.VirtualPath.TrimStart('~');
                foreach (var name in names)
                {
                    System.Diagnostics.Debug.WriteLine("BundleModule: register static " + name);
                    Get[name] = BundleByVirtualPathResponse;
                }
            }
            // dynamic css
            {
                var name = BundleSettings.Css.MakePath("{names}").TrimStart('~');
                System.Diagnostics.Debug.WriteLine("BundleModule: register dynamic css " + name);
                Get[name] = CombinedStyleResponse;
            }
            // dynamic js
            {
                var name = BundleSettings.Js.MakePath("{names}").TrimStart('~');
                System.Diagnostics.Debug.WriteLine("BundleModule: register dynamic js" + name);
                Get[name] = CombinedScriptResponse;
            }
        }

		#endregion Constructors 

		#region Static Methods 

		// Private Methods 

        private static object CombinedScriptResponse(dynamic parameters)
        {
            Dictionary<Alias, Filename> dynamicAliases = null;
            string names = parameters.names;
            var files = names.Split(BundleSettings.NameSeparator)
                .Distinct()
                .Select(alias => ResolveAlias(ref dynamicAliases, alias, BundleTypes.Script))
                .ToList();
            var plain = RegisteredFileDependencies.ResolveDependencies(files);
            return RenderFiles(BundleTypes.Script, plain);
        }

        private static object CombinedStyleResponse(dynamic parameters)
        {
            Dictionary<Alias, Filename> dynamicAliases = null;
            string names = parameters.names;
            var files = names.Split(BundleSettings.NameSeparator)
                .Distinct()
                .Select(alias => ResolveAlias(ref dynamicAliases, alias, BundleTypes.StyleSheet))
                .ToList();
            var plain = RegisteredFileDependencies.ResolveDependencies(files);
            return RenderFiles(BundleTypes.Script, plain);
        }

        private static string CombineFiles(IEnumerable<Filename> files)
        {
            var builder = new StringBuilder();

            foreach (var file in files)
            {
                file.Check();
                var fullPath = Path.Combine(BundleSettings.rootPathProvider.GetRootPath(), file.Name.Substring(2));
                if (!File.Exists(fullPath))
                    throw new InvalidOperationException(
                        string.Format("Could not load bundled file {0}", fullPath));
                var content = File.ReadAllText(fullPath);
                builder.Append(content);
            }

            var combined = builder.ToString();
            return combined;
        }

        private static object RenderFiles(BundleTypes bundleType, IEnumerable<Filename> files)
        {
            string combined = CombineFiles(files);
            if (BundleSettings.processing == BundleProcessing.CombinedMinified)
            {
                var minifier = new Microsoft.Ajax.Utilities.Minifier();
                switch (bundleType)
                {
                    case BundleTypes.Script:
                        combined = minifier.MinifyJavaScript(combined);
                        break;
                    case BundleTypes.StyleSheet:
                        combined = minifier.MinifyStyleSheet(combined);
                        break;
                    default:
                        throw new NotImplementedException(string.Format(
                          "Unable to minify data for {0}.{1}",
                          bundleType.GetType(), bundleType));
                }
            }
            string contentType = BundleSettings.GetMimeType(bundleType);
            return new TextResponse(combined, contentType);
        }

        private static Filename ResolveAlias(ref Dictionary<Alias, Filename> dynamicAliases, Alias alias, BundleTypes bundleType)
        {
            var alias1 = new AliasBundleType(alias, bundleType);
            var fileByAlias = RegisteredAliases.GetFileByAlias(alias1 );
            if (fileByAlias != null)
                return fileByAlias.Value;
            Filename fileName;
            if (dynamicAliases == null)
            {
                dynamicAliases = new Dictionary<Alias, Filename>();
                foreach (var bundle in RegisteredBundles.GetExplicitBundles())
                    foreach (var file in bundle.FilesWithDependencies)
                    {
                        var type = GetFileTypeByExtension(file);
                        if (type == bundleType)
                            dynamicAliases[RegisteredAliases.GetOrCreateAlias(file)] = file;
                    }
            }
            if (!dynamicAliases.TryGetValue(alias, out fileName))
                throw new Exception(string.Format("Unregistered alias {0}", alias));
            return fileName;
        }
		// Internal Methods 

        internal static BundleTypes? GetFileTypeByExtension(Filename file)
        {
            var ext = file.Extension.ToLowerInvariant();
            if (ext == ".js")
                return BundleTypes.Script;
            if (ext == ".css")
                return BundleTypes.StyleSheet;
            return null;
        }

		#endregion Static Methods 

		#region Methods 

		// Private Methods 

        object BundleByVirtualPathResponse(dynamic parameters)
        {
            var bundle = RegisteredBundles.GetBundleByPath(Request.Path.MakeTildaPrefixedPath());
            return bundle == null
                ? HttpStatusCode.NotFound
                : RenderFiles(bundle.BundleType, bundle.FilesWithDependencies);
        }

		#endregion Methods 
    }
}
