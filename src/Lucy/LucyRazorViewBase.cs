using Lucy.Bundle;
using Nancy.ViewEngines.Razor;

namespace Lucy
{

    public abstract class LucyRazorViewBase<TModel, TViewModel>
        : LucyRazorViewBase<ModelViewModel<TModel, TViewModel>>
    {

    }

    public abstract class LucyRazorViewBase<TModel>
        : NancyRazorViewBase<TModel>
    {
        #region Methods

        // Public Methods 

        public override void Initialize(RazorViewEngine engine, Nancy.ViewEngines.IRenderContext renderContext, object model)
        {
            base.Initialize(engine, renderContext, model);
            LucyEngine.InitializeView(this);
            Scripts = new HtmlRenderer(renderContext, BundleTypes.Script);
            Styles = new HtmlRenderer(renderContext, BundleTypes.StyleSheet);
        }

        #endregion Methods

        #region Properties

        public HtmlRenderer Scripts { get; private set; }

        public HtmlRenderer Styles { get; private set; }

        #endregion Properties
    }
}
