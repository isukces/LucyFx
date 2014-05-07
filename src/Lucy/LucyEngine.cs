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
            LucyToys toys = LucyToys.TryGetLucyToys(context.ViewBag);
            if (toys == null)
                toys = new LucyToys();
            toys.Container = container;
            context.ViewBag[LucyToys.KEY] = toys;

        }

        public static void Set1<TModel>(NancyRazorViewBase<TModel> view)
        {
            LucyToys toys = LucyToys.TryGetLucyToys(view.ViewBag);
            if (toys == null)
                toys = new LucyToys();
            toys.WriteLiteral = view.WriteLiteral;
            view.ViewBag[LucyToys.KEY] = toys;
        }


    }
}
