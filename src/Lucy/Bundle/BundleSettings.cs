using System;
using Nancy;
using Nancy.ViewEngines.Razor;

namespace Lucy.Bundle
{
    public static class BundleSettings
    {
        #region Static Methods

        // Internal Methods 

        internal static string GetMimeType(BundleTypes bundleType)
        {
            switch (bundleType)
            {
                case BundleTypes.Script:
                    return Js.MimeType;
                case BundleTypes.StyleSheet:
                    return Css.MimeType;
                default:
                    throw new NotImplementedException(String.Format(
                       "Unable to find mime type for  {0}.{1}",
                       bundleType.GetType(), bundleType));
            }
        }

        internal static string GetRenderTemplate(BundleTypes bundleType)
        {
            switch (bundleType)
            {
                case BundleTypes.Script:
                    return Js.HtmlRenderTemplate;
                case BundleTypes.StyleSheet:
                    return Css.HtmlRenderTemplate;
                default:
                    throw new NotImplementedException(String.Format(
                        "Unable to find html render template for  {0}.{1}",
                        bundleType.GetType(), bundleType));
            }
        }

        internal static string MakePath(string path, string name, string extension)
        {
            path = path.MakeTildaPrefixedPath().TrimEnd('/');
            name = name.BackslashToSlash().TrimStart('/');
            if (!extension.StartsWith("."))
                extension = "." + extension;
            return path + "/" + name + extension;

        }

        #endregion Static Methods

        #region Static Fields

        public static char NameSeparator = ',';

        #endregion Static Fields

        #region Nested Classes


        public static class Css
        {
            #region Static Methods

            // Internal Methods 

            /// <summary>
            /// Tilda prefixed path for style
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            internal static string MakePath(string name)
            {
                return BundleSettings.MakePath(BundlePath, name, FileExtension);
            }

            #endregion Static Methods

            #region Static Fields
            
            public static string BundlePath = "~/bundles/styles/";
            public static string FileExtension = ".css";
            public static string HtmlRenderTemplate = "    <link rel=\"stylesheet\" href=\"{0}\" />\r\n";
            public static string MimeType = "text/css";

            #endregion Static Fields
        }
        public static class Js
        {
            #region Static Methods

            // Internal Methods 

            /// <summary>
            /// Tilda prefixed path for script
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            internal static string MakePath(string name)
            {
                return BundleSettings.MakePath(BundlePath, name, FileExtension);
            }

            #endregion Static Methods

            #region Static Fields

            public static string BundlePath = "~/bundles/scripts/";
            public static string FileExtension = ".js";
            public static string HtmlRenderTemplate = "    <script src=\"{0}\"></script>\r\n";
            public static string MimeType = "text/javascript";

            #endregion Static Fields
        }
        #endregion Nested Classes

        public static readonly StringComparer IgnoreCase = StringComparer.OrdinalIgnoreCase;

        public static void Enable(BundleProcessing processing)
        {
            Processing = processing;
        }

        internal static BundleProcessing Processing;
        internal static IRootPathProvider RootPathProvider;
    }
}
