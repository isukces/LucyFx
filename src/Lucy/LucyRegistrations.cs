using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lucy
{
    public class LucyRegistrations : Nancy.Bootstrapper.ApplicationRegistrations
    {
        public LucyRegistrations()
        {
#if DEBUG
            this.RegisterWithDefault<ILucyTextProvider>(typeof(DebugLucyTextProvider));
#else
            this.RegisterWithDefault<ILucyTextProvider>(typeof(DefaultLucyTextProvider));  
#endif
        }
    }
}
