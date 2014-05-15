using Lucy.TextProviders;

namespace Lucy
{
    public class LucyApplicationRegistrations : Nancy.Bootstrapper.ApplicationRegistrations
    {
        public LucyApplicationRegistrations()
        {
#if DEBUG
            RegisterWithDefault<ILucyTextProvider>(typeof(DebugLucyTextProvider));
#else
            RegisterWithDefault<ILucyTextProvider>(typeof(DefaultLucyTextProvider));  
#endif
        }
    }
}
