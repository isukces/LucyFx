using System;
using System.Collections.Generic;
using Lucy.TextProviders;

namespace Lucy
{
    public class LucyToys
    {
        #region Constructors

        private LucyToys()
        {
            Javascripts = new List<string>();
            AttachedExceptions = new List<Exception>();
            RenderedFiles = new List<Filename>();
        }

        #endregion Constructors

        #region Static Methods

        // Public Methods 

        public static LucyToys Get(dynamic viewBag)
        {
            var item = viewBag[Key];
            if (!item.HasValue)
                throw new Exception(
                    "Lucy's data has not been initialized properly. Derive your view from LucyRazorViewBase.");
            var lucyToys = item.Value as LucyToys;
            return lucyToys;
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

        // Private Methods 

        private TResolveType Resolve<TResolveType>() where TResolveType : class
        {
            if (Container != null)
                return Container.Resolve<TResolveType>();
            throw new Exception("IoC Container is empty. Call Lucy.LucyEngine.RequestStartup method in your Bootstrapper.RequestStartup.");
        }

        #endregion Methods

        #region Static Fields

        private const string Key = "_____LucyToys______";

        #endregion Static Fields

        #region Fields

        ILucyTextProvider _nameProvider;

        #endregion Fields

        #region Properties

        public List<Exception> AttachedExceptions { get; private set; }

        public Nancy.TinyIoc.TinyIoCContainer Container { get; set; }

        public List<string> Javascripts { get; private set; }

        public ILucyTextProvider NameProvider
        {
            get
            {
                return _nameProvider ?? (_nameProvider = Resolve<ILucyTextProvider>());
            }
        }

        public List<Filename> RenderedFiles { get; set; }

        public Action<object> WriteLiteral { get; set; }

        #endregion Properties
    }
}
