using Nancy.ViewEngines.Razor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lucy
{
    public class LucyEngine
    {
        public static void RequestStartup(Nancy.TinyIoc.TinyIoCContainer container, Nancy.NancyContext context)
        {
            LucyToys.GetOrCreate(context.ViewBag);
        }

        public static void AttachView<TModel>(NancyRazorViewBase<TModel> view)
        {
            LucyToys toys = LucyToys.GetOrCreate(view.ViewBag);
            toys.WriteLiteral = view.WriteLiteral;
        }


    }
}
