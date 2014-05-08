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

        ///// <summary>
        ///// Stores error messages in ViewBag
        ///// </summary>
        ///// <param name="result"></param>
        //[Obsolete]
        //protected void SetValidationErrors(ModelValidationResult result)
        //{
        //    if (result.IsValid) return;
        //    foreach (var errorKeyValue in result.Errors)
        //    {
        //        ModelValidationError firstError = errorKeyValue.Value.First();
        //        ViewBag[ViewBagNamePrefix + errorKeyValue.Key] = firstError.ErrorMessage;
        //    }
        //}
        ///// <summary>
        ///// Prefix for key name storing error message in ViewBag
        ///// </summary>

        //public const string ViewBagNamePrefix = "_validation_error_for_";
    }
}
