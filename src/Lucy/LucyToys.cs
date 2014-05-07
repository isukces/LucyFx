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
        }

		#endregion Constructors 

		#region Static Methods 

		// Internal Methods 

        internal static LucyToys TryGetLucyToys(dynamic ViewBag)
        {
            var a = ViewBag[LucyToys.KEY];
            if (a.HasValue)
            {
                var t = a.Value as LucyToys;
                return t;
            }
            return null;
        }

		#endregion Static Methods 

		#region Fields 

        public static string KEY = "_____LucyToys______";

		#endregion Fields 

		#region Properties 

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
