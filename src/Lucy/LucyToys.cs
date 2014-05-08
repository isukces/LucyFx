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

        // Internal Methods 

        #endregion Static Methods

        #region Fields

        static string KEY = "_____LucyToys______";

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

        #endregion Fields

        #region Properties

        public List<Exception> AttachedExceptions { get; private set; }

        public Nancy.TinyIoc.TinyIoCContainer Container { get; set; }

        public List<string> Javascripts { get; private set; }

        public Action<object> WriteLiteral { get; set; }

        ILucyTextProvider nameProvider;
        public ILucyTextProvider NameProvider
        {
            get
            {
                if (nameProvider == null)
                    nameProvider = Container.Resolve<ILucyTextProvider>();
                return nameProvider;
            }
        }

        #endregion Properties
    }
}
