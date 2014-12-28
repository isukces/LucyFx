using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Lucy.Bundle;
using Lucy.TextProviders;

namespace Lucy
{
    public class LucyToys
    {
        #region Constructors

        private LucyToys()
        {
            JavascriptBundle = new Bundle.Bundle("lucyDynamicBundle", BundleTypes.Script, null)
            {
                IsDynamic = true
            };
            RegisteredBundles.AddBundle(JavascriptBundle);
            AttachedExceptions = new List<Exception>();
            RenderedFiles = new List<Filename>();
        }

        #endregion Constructors

        #region Static Methods

        // Public Methods 
        [NotNull]
        public static LucyToys Get(dynamic viewBag)
        {
            var item = viewBag[Key];
            // ReSharper disable once InvertIf
            if (item.HasValue)
            {
                var lucyToys = item.Value as LucyToys;
                if (lucyToys != null)
                    return lucyToys;
            }
            throw new Exception(
                "Lucy's data has not been initialized properly. Derive your view from LucyRazorViewBase.");
        }

        public static LucyToys GetOrCreate(dynamic viewBag)
        {
            LucyToys lucyToys = null;
            var item = viewBag[Key];
            if (item.HasValue)
                lucyToys = item.Value as LucyToys;
            if (lucyToys == null)
                viewBag[Key] = lucyToys = new LucyToys();
            return lucyToys;
        }

        #endregion Static Methods

        #region Methods

        // Public Methods 

        public void WriteLiteral(object x)
        {
            if (WriteLiteralAction == null)
                throw new Exception(
                    "Unable to write literal. Derive your view from LucyRazorViewBase or call LucyEngine.InitializeView manually");
            if (x != null)
                WriteLiteralAction(x);
        }
        // Private Methods 

        private TResolveType Resolve<TResolveType>() where TResolveType : class
        {
            if (Container != null)
                return Container.Resolve<TResolveType>();
            throw new Exception("IoC Container is empty. Call Lucy.LucyEngine.RequestStartup method in your Bootstrapper.RequestStartup.");
        }

        #endregion Methods

        #region Fields

        ILucyTextProvider _nameProvider;
        private const string Key = "_____LucyToys______";

        #endregion Fields

        #region Properties

        public List<Exception> AttachedExceptions { get; private set; }

        public Nancy.TinyIoc.TinyIoCContainer Container { get; set; }

        public Bundle.Bundle JavascriptBundle { get; private set; }

        public ILucyTextProvider NameProvider
        {
            get
            {
                return _nameProvider ?? (_nameProvider = Resolve<ILucyTextProvider>());
            }
        }

        public List<Filename> RenderedFiles { get; set; }

        public Action<object> WriteLiteralAction { get; set; }

        #endregion Properties
    }
}
