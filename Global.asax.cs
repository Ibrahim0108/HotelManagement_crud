using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace HotelManagement
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
            HttpCookie authCookie = Context.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);

                if (authTicket != null && !authTicket.Expired)
                {
                    string username = authTicket.Name;
                    string role = authTicket.UserData;  // <-- this contains the role string

                    var identity = new System.Security.Principal.GenericIdentity(username);
                    string[] roles = role.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries); // if multiple roles separated by comma

                    var principal = new System.Security.Principal.GenericPrincipal(identity, roles);
                    HttpContext.Current.User = principal;
                }
            }
        }



    }
}
