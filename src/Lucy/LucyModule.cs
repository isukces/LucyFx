using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lucy
{
    public class LucyModule : NancyModule
    {
        public LucyModule()
        {
          //  this.WardrobeConfig = config;
            Before += BeforeAction;
        }


        Response BeforeAction(NancyContext context)
        {
           // this.ViewBag["__IWardrobeConfig__"] = WardrobeConfig;
            return null;
        }
    }
}
