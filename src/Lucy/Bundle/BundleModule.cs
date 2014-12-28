using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Ajax.Utilities;
using Nancy;
using Nancy.Responses;

namespace Lucy.Bundle
{
    public class BundleModule : NancyModule
    {
        #region Constructors

        public BundleModule()
        {
            // static bundles
            {
                string[] names = (
                    from bundle in RegisteredBundles.GetExplicitBundles()
                    where !bundle.IsDynamic
                    select bundle.VirtualPath.TrimStart('~'))
                    .ToArray();
                foreach (string name in names)
                {
                    Debug.WriteLine("BundleModule: register static " + name);
                    Get[name] = BundleByVirtualPathResponse;
                }
            }
            // dynamic css
            {
                string name = BundleSettings.Css.MakePath("{names}").TrimStart('~');
                Debug.WriteLine("BundleModule: register dynamic css " + name);
                Get[name] = CombinedStyleResponse;
            }
            // dynamic js
            {
                string name = BundleSettings.Js.MakePath("{names}").TrimStart('~');
                Debug.WriteLine("BundleModule: register dynamic js" + name);
                Get[name] = CombinedScriptResponse;
            }
        }

        #endregion Constructors

        #region Static Methods

        // Private Methods 

        private static string CombineFiles(IEnumerable<FileInfo> files)
        {
            var builder = new StringBuilder();
            foreach (var fileInfo in files)
            {
                var content = File.ReadAllText(fileInfo.FullName);
                builder.AppendLine(content);
            }
            var combined = builder.ToString();
            return combined;
        }

        private static FileInfo FilenameToFileInfo(Filename file)
        {
            file.Check();
            var fileInfo =
                new FileInfo(Path.Combine(BundleSettings.rootPathProvider.GetRootPath(), file.Name.Substring(2)));
            if (fileInfo.Exists)
                return fileInfo;
            throw new InvalidOperationException(string.Format("Bundled file {0} not found", fileInfo.FullName));
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="dynamicAliases">I think we can use normal Dictionary here</param>
        /// <param name="alias"></param>
        /// <param name="bundleType"></param>
        /// <returns></returns>
        private static Filename ResolveAlias(ref Dictionary<Alias, Filename> dynamicAliases, Alias alias,
            BundleTypes bundleType)
        {
            var alias1 = new AliasBundleType(alias, bundleType);
            Filename? fileByAlias = RegisteredAliases.GetFileByAlias(alias1);
            if (fileByAlias != null)
                return fileByAlias.Value;
            Filename fileName;
            if (dynamicAliases == null)
            {
                // fill 
                dynamicAliases = new  Dictionary<Alias, Filename>();
                foreach (Bundle bundle in RegisteredBundles.GetExplicitBundles())
                    foreach (Filename file in bundle.FilesWithDependencies)
                    {
                        BundleTypes? type = GetFileTypeByExtension(file);
                        if (type == bundleType)
                            dynamicAliases[RegisteredAliases.GetOrCreateAlias(file)] = file;
                    }
            }
            if (!dynamicAliases.TryGetValue(alias, out fileName))
                throw new Exception(string.Format("Unregistered alias {0}", alias));
            return fileName;
        }

        private static DateTime TruncateToSeconds(DateTime dateTime)
        {
            return new DateTime(dateTime.Year,
                dateTime.Month,
                dateTime.Day,
                dateTime.Hour,
                dateTime.Minute,
                dateTime.Second,
                dateTime.Kind);
        }
        // Internal Methods 

        internal static BundleTypes? GetFileTypeByExtension(Filename file)
        {
            string ext = file.Extension.ToLowerInvariant();
            if (ext == ".js")
                return BundleTypes.Script;
            if (ext == ".css")
                return BundleTypes.StyleSheet;
            return null;
        }

        #endregion Static Methods

        #region Methods

        // Private Methods 

        private object BundleByVirtualPathResponse(dynamic parameters)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            Bundle bundle = RegisteredBundles.GetBundleByPath(Request.Path.MakeTildaPrefixedPath());
            stopwatch.Stop();
            Debug.WriteLine("GetBundleByPath for {0} = {1}ms", Request.Path, stopwatch.ElapsedMilliseconds);
        

            if ( bundle == null)
                return HttpStatusCode.NotFound;
            stopwatch.Start();
            var response = RenderFiles(bundle.BundleType, bundle.FilesWithDependencies);
            stopwatch.Stop();
            Debug.WriteLine("Render files for {0} = {1}ms",Request.Path, stopwatch.ElapsedMilliseconds);
            return response;
        }

        private bool CanSendNotModifiedResponse(string eTag, DateTime lastModified)
        {
            var haveIfNoneMatchCondition = false;
            var ifNoneMatch = Request.Headers.IfNoneMatch;
            if (ifNoneMatch != null && ifNoneMatch.Any())
            {
                haveIfNoneMatchCondition = true;
                if (Request.Headers.IfNoneMatch.All(x => x != eTag))
                    return false;
            }
            var ifModifiedSince = Request.Headers.IfModifiedSince;
            if (!ifModifiedSince.HasValue) return haveIfNoneMatchCondition; // false if no 'IfNoneMatch' and no 'IfModifiedSince' present
            lastModified = TruncateToSeconds(lastModified);
            return lastModified <= ifModifiedSince.Value;
        }

        private object CombinedScriptResponse(dynamic parameters)
        {
            Dictionary<Alias, Filename> dynamicAliases = null;
            string names = parameters.names;
            List<Filename> files = names.Split(BundleSettings.NameSeparator)
                .Distinct()
                .Select(alias => ResolveAlias(ref dynamicAliases, alias, BundleTypes.Script))
                .ToList();
            List<Filename> plain = RegisteredFileDependencies.ResolveDependencies(files).ToList();
            return RenderFiles(BundleTypes.Script, plain);
        }

        private object CombinedStyleResponse(dynamic parameters)
        {
            Dictionary<Alias, Filename> dynamicAliases = null;
            string names = parameters.names;
            List<Filename> files = names.Split(BundleSettings.NameSeparator)
                .Distinct()
                .Select(alias => ResolveAlias(ref dynamicAliases, alias, BundleTypes.StyleSheet))
                .ToList();
            List<Filename> plain = RegisteredFileDependencies.ResolveDependencies(files).ToList();
            return RenderFiles(BundleTypes.Script, plain);
        }

        private object RenderFiles(BundleTypes bundleType, IEnumerable<Filename> files)
        {
            FileInfo[] fileInfos = (
                from file in files
                select FilenameToFileInfo(file)
                ).ToArray();
            bool doCompress = BundleSettings.processing == BundleProcessing.CombinedMinified;
            var cacheItem = BundleCache.TryGet(bundleType, fileInfos, doCompress);
            var combined = cacheItem != null ? cacheItem.Content : null;
            if (combined == null)
            {
                combined = CombineFiles(fileInfos);
                if (doCompress)
                {
                    var minifier = new Minifier();
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
                cacheItem = BundleCache.Store(bundleType, fileInfos, doCompress, combined);
            }
            var contentType = BundleSettings.GetMimeType(bundleType);
            var response = new TextResponse(combined, contentType);
            var etag = cacheItem.ETag;
            if (CanSendNotModifiedResponse(etag, cacheItem.ContentTimestampUtc))
                return HttpStatusCode.NotModified;
            response.Headers["ETag"] = etag;
            response.Headers["Last-Modified"] = cacheItem.ContentTimestampUtc.ToString("R");
            return response;
        }

        #endregion Methods
    }
}