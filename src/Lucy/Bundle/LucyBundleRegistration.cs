namespace Lucy.Bundle
{
    public abstract class LucyBundleRegistration
    {
        #region Methods

        // Protected Methods 

        protected Bundle AddScriptBundle(string name)
        {
            return AddBundle(name, BundleTypes.Script);
        }
        protected Bundle AddDynamicScriptBundle(string name)
        {
            var bundle = AddBundle(name, BundleTypes.Script);
            bundle.IsDynamic = true;
            return bundle;
        }

        protected void RegisterAlias(Filename fileName, Alias alias)
        {
            RegisteredAliases.RegisterAlias(fileName, alias);
        }

        protected void AddDependency(string baseFile, params string[] dependencies)
        {
            RegisteredFileDependencies.AddDependency(baseFile, dependencies);
        }

        protected Bundle AddStyleBundle(string name)
        {
            return AddBundle(name, BundleTypes.StyleSheet);
        }
        // Private Methods 

        private Bundle AddBundle(string name, BundleTypes type)
        {
            var bundle = new Bundle(name, type, null);
            return RegisteredBundles.AddBundle(bundle);
        }

        #endregion Methods
    }
}
