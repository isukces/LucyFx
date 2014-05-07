using Nancy.ViewEngines.Razor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lucy
{
    public abstract class LucyRazorViewBase<TModel>
        : NancyRazorViewBase<TModel>
    {
        public override void Initialize(RazorViewEngine engine, Nancy.ViewEngines.IRenderContext renderContext, object model)
        {
            base.Initialize(engine, renderContext, model);

            LucyEngine.Set1(this);
            //LucyToys toys = LucyToys.TryGetLucyToys(renderContext.Context.ViewBag);
            //if (toys == null)
            //    toys = new LucyToys();
            //toys.WriteLiteral = WriteLiteral;
            //renderContext.Context.ViewBag[LucyToys.KEY] = toys;
        }
    }
}
