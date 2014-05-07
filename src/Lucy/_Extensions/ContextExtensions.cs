using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lucy
{
    public static class ContextExtensions
    {
        /// <param name="context">Nancy context</param>
        /// <param name="path">Path to expand</param>
        /// <returns>Expanded path</returns>
        public static string ToFullPath2(this NancyContext context, string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return path;
            }

            if (context.Request == null)
            {
                return path.TrimStart('~');
            }

            if (string.IsNullOrEmpty(context.Request.Url.BasePath))
            {
                return path.TrimStart('~');
            }

            if (!path.StartsWith("~/"))
            {
                return path;
            }

            return string.Format("{0}{1}", context.Request.Url.BasePath, path.TrimStart('~'));
        }
    }
}
