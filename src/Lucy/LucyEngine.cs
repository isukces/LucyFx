using Nancy.ViewEngines.Razor;

namespace Lucy
{
    public static class LucyEngine
    {
        #region Static Methods

        // Public Methods 

        /// <summary>
        /// If you use views derived from LucyRazorViewBase - forget about this method.
        /// Otherwise invoke it from YOURVIEW.Initialize.
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="view"></param>
        public static void InitializeView<TModel>(NancyRazorViewBase<TModel> view)
        {
            LucyToys toys = LucyToys.GetOrCreate(view.ViewBag);
            toys.WriteLiteralAction = view.WriteLiteral;
        }


        /// <summary>
        /// MUST DO: Call this method from YOURBOOTSTRAPER.RequestStartup.
        /// </summary>
        /// <param name="container"></param>
        /// <param name="context"></param>
        public static void RequestStartup(Nancy.TinyIoc.TinyIoCContainer container, Nancy.NancyContext context)
        {
            LucyToys toys = LucyToys.GetOrCreate(context.ViewBag);
            toys.Container = container;
        }

        #endregion Static Methods
    }
}