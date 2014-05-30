using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lucy.Bundle;

namespace Lucy.SampleServer.Code
{
    public class BundlesRegistration : LucyBundleRegistration
    {
        public BundlesRegistration()
        {

            AddStyleBundle("mystyle")
                .Include("~/assets/css/header.css")
                .Include("~/assets/css/other.css");
        }
    }
}