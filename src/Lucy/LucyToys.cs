using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lucy
{
    public class LucyToys
    {
        #region Constructors

        public LucyToys()
        {
            Javascripts = new List<string>();
            AttachedExceptions = new List<Exception>();
        }

        #endregion Constructors

        #region Static Methods

        // Public Methods 

        public static LucyToys Get(dynamic ViewBag)
        {
            var item = ViewBag[LucyToys.KEY];
            if (item.HasValue)
            {
                var lucyToys = item.Value as LucyToys;
                return lucyToys;
            }
            throw new Exception("Lucy's data has not been initialized properly. Derive your view from LucyRazorViewBase.");
        }

        public static LucyToys GetOrCreate(dynamic ViewBag)
        {
            LucyToys lucyToys = null;
            var item = ViewBag[LucyToys.KEY];
            if (item.HasValue)
                lucyToys = item.Value as LucyToys;
            if (lucyToys == null)
                ViewBag[LucyToys.KEY] = lucyToys = new LucyToys();
            return lucyToys;
        }

        #endregion Static Methods

        #region Methods

        // Private Methods 

        private ResolveType Resolve<ResolveType>() where ResolveType : class
        {
            if (Container != null)
                return Container.Resolve<ResolveType>();
            throw new Exception("IoC Container is empty. Call Lucy.LucyEngine.RequestStartup method in your Bootstrapper.RequestStartup.");
        }

        #endregion Methods

        #region Static Fields

        static string KEY = "_____LucyToys______";

        #endregion Static Fields

        #region Fields

        ILucyTextProvider nameProvider;

        #endregion Fields

        #region Properties

        public List<Exception> AttachedExceptions { get; private set; }

        public Nancy.TinyIoc.TinyIoCContainer Container { get; set; }

        public List<string> Javascripts { get; private set; }

        public ILucyTextProvider NameProvider
        {
            get
            {
                if (nameProvider == null)
                    nameProvider = this.Resolve<ILucyTextProvider>();
                return nameProvider;
            }
        }

        public Action<object> WriteLiteral { get; set; }

        #endregion Properties
    }
}
