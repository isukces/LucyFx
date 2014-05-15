using Nancy;
using Nancy.Bootstrapper;

namespace Lucy.Bundle
{
    public class BundleApplicationStartup : IApplicationStartup
    {
        #region Constructors

        public BundleApplicationStartup(IRootPathProvider rootPathProvider)
        {
            BundleSettings.RootPathProvider = rootPathProvider;
        }

        #endregion Constructors

        #region Methods

        // Public Methods 

        public void Initialize(IPipelines pipelines)
        {
        }

        #endregion Methods
    }
}
