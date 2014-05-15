using Nancy;

namespace Lucy
{
    public abstract class LucyModule : NancyModule
    {
        #region Constructors

        protected LucyModule()
        {
            Before += BeforeAction;
        }

        protected LucyModule(string modulePath)
            : base(modulePath)
        {
            Before += BeforeAction;
        }

        #endregion Constructors

        #region Static Methods

        // Private Methods 

        static Response BeforeAction(NancyContext context)
        {
            // for future use
            return null;
        }

        #endregion Static Methods
    }
}
