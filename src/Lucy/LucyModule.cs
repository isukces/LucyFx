using Nancy;
using Nancy.ModelBinding;
using Nancy.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        #region Methods

        // Protected Methods 

      
        // Private Methods 

        Response BeforeAction(NancyContext context)
        {
            // this.ViewBag["__IWardrobeConfig__"] = WardrobeConfig;
            return null;
        }

        #endregion Methods
       
    }
}
