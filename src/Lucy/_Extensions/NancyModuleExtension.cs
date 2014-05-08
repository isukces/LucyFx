using Nancy;
using Nancy.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lucy
{
    public static class NancyModuleExtension
    {
        #region Static Methods

        // Public Methods 

        public static void AttachException(this NancyModule module, Exception exception)
        {
            var lt = LucyToys.Get(module.ViewBag) as LucyToys;
            lt.AttachedExceptions.Add(exception);
        }

        public static IEnumerable<Exception> GetExceptions(this NancyModule module)
        {
            var lt = LucyToys.Get(module.ViewBag) as LucyToys;
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
