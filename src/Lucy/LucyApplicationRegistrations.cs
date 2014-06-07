using Lucy.TextProviders;

namespace Lucy
{
    public class LucyApplicationRegistrations : Nancy.Bootstrapper.Registrations
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
