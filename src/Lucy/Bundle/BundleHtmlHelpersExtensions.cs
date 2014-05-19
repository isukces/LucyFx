using System.Linq;
using System.Text;
using Nancy.Conventions;
using Nancy.ViewEngines.Razor;

namespace Lucy.Bundle
{
    public static class BundleHtmlHelpersExtensions
    {
        #region Static Methods

        // Public Methods 

        public static IHtmlString RenderScripts<TModel>(this HtmlHelpers<TModel> x)
        {
            var toys = x.RenderContext.GetLucyToys();
            
            var htmlRenderer = new HtmlRenderer(x.RenderContext, BundleTypes.Script);
            return htmlRenderer.Render(toys.JavascriptBundle);
            /*
            var sb = new StringBuilder();
            foreach (var javascript in toys.JavascriptBundle.Distinct())
                sb.AppendFormat("<script src=\"{0}\"></script>\r\n", javascript);
            return new NonEncodedHtmlString(sb.ToString());
                 */
        }

        #endregion Static Methods
    }
}
