using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Lucy
{
    static class StringExtension
    {
        #region Static Methods

        // Public Methods 

        public static void CheckAlias(this string alias)
        {
            if (aliasRegexp == null)
                aliasRegexp = new Regex("^[a-z][-a-z0-9]*$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            if (!aliasRegexp.IsMatch(alias))
                throw new Exception(string.Format("Invalid alias '{0}'. Valid alias starts with latin letter optionally followed by latin letters or digits.", alias));
            // for future use

        }

        public static void CheckTildaPrefixedPath(this string path)
        {
            path = (path ?? string.Empty).Trim().Replace("\\", "/");
            if (!path.StartsWith("~/"))
                throw new Exception(string.Format("Path '{0}' is invalid because doesn't start with '~/'", path));
        }

        public static string MakeTildaPrefixedPath(this string path)
        {
            path = (path ?? string.Empty).Trim().BackslashToSlash();
            if (path.StartsWith("~/")) return path;
            path = "~/" + path.TrimStart('~', '/');
            return path;
        }

        public static string BackslashToSlash(this string path)
        {
            return path.Replace("\\", "/");
        }

        #endregion Static Methods

        #region Static Fields

        static Regex aliasRegexp;

        #endregion Static Fields
    }
}
