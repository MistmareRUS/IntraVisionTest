using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntraVisionTest.Extentions
{
    public class AdminAuthorizeAttribute : System.Web.Mvc.AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var id = (httpContext.Request.RequestContext.RouteData.Values["SecretKey"] as string)??(httpContext.Request["SecretKey"] as string);
            if (id == "magickey")
            {
                return true;
            }
            return false;
        }
    }
}