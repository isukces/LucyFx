using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lucy
{
    public abstract class LucyModule : NancyModule
    {
        protected LucyModule()
        {
            Before += BeforeAction;
        }

        protected LucyModule(string modulePath)
            : base(modulePath)
        {
            Before += BeforeAction;
        }


        Response BeforeAction(NancyContext context)
        {
            // this.ViewBag["__IWardrobeConfig__"] = WardrobeConfig;
            return null;
        }
    }
}
