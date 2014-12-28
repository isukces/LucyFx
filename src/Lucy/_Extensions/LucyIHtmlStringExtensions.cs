using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy.ViewEngines.Razor;

namespace Lucy 
{
    public static  class LucyIHtmlStringExtensions
    {
        public static string RenderString(this IHtmlString x)
        {
            return x == null ? string.Empty : (x.ToHtmlString() ?? string.Empty);
        }
    }
}
