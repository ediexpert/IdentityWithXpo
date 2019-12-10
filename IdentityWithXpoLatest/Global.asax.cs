﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using DevExpress.Xpo;

namespace IdentityWithXpoLatest
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            XpoDefault.DataLayer = XpoHelper.GetDataLayer("DefaultConnection");
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
