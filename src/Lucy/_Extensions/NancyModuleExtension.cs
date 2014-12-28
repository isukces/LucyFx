using System.Linq.Expressions;
using Nancy;
using Nancy.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lucy
{
    public static class NancyModuleExtension
    {
        #region Static Methods

        // Public Methods 

        public static void AttachException(this NancyModule module, Exception exception)
        {
            LucyToys lt = LucyToys.Get(module.ViewBag);
            lt.AttachedExceptions.Add(exception);
        }
        /*
        public static void AttachException<TModel>(this NancyModule module, Expression<Func<TModel, object>> selector, string message)
        {
            LucyToys lt = LucyToys.Get(module.ViewBag);
            lt.AttachedExceptions.Add(exception);
        }
         */

        private static IEnumerable<Exception> GetExceptions(this NancyModule module)
        {
            LucyToys lt = LucyToys.Get(module.ViewBag);
            return lt.AttachedExceptions;
        }

        public static ModelBindingException GetModelBindingException(this NancyModule module)
        {
            return module
                .GetExceptions()
                .OfType<ModelBindingException>()
                .FirstOrDefault();
        }

        #endregion Static Methods
    }
}
