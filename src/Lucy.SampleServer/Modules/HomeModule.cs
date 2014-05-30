using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lucy.SampleServer.Modules
{
    public class HomeModule:LucyModule
    {
        public HomeModule()
        {
            Get["/"] = GetHome;
        }

        private dynamic GetHome(dynamic o)
        {
            return View["Index"];
        }
    }
}