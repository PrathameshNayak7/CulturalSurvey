using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;
using System.Security.Principal;
using CulturaSurvey.Infrastructure;
using CulturaSurvey;
using System.Web.Optimization;
using CulturaSurvey.ViewModel;

namespace CulturaSurvey
{
    public class Global : HttpApplication
    {

       void Application_Start(object sender, EventArgs e)
        {

            

            RouteCollection routes=new RouteCollection();
            // Code that runs on application startup
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
          
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            RegisterBundles(BundleTable.Bundles);

      

            BundleTable.EnableOptimizations = true;
        
        }
       
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new StyleBundle("~/Content/Home").Include("~/Content/css/bootstrap.css")
            .Include("~/Content/css/customized.css")
            .Include("~/Content/css/font-awesome.css")
            .Include("~/Content/css/font-awesome-animation.css")
            .Include("~/Content/css/iziToast.min.css")
            .Include("~/Content/css/Style.css", new CssRewriteUrlTransform()));

            bundles.Add(new ScriptBundle("~/Content/culturalsurveybundle")
                .Include("~/Content/js/jquery-3.3.1.min.js")
                .Include("~/Content/js/bootstrap.js.js")
                .Include("~/Content/js/toastr-custom.js")
                .Include("~/Content/js/iziToast.min.js"));

            bundles.Add(new ScriptBundle("~/Content/culturalsurveywelcome").Include("~/Content/js/Welcome.js"));
            bundles.Add(new ScriptBundle("~/Content/culturalsurveyselection").Include("~/Content/js/Selection.js").Include("~/Content/js/_Load_Masters.js"));

            bundles.Add(new ScriptBundle("~/Content/culturalsurveyquestions").Include("~/Content/js/questions.js"));

        }
        protected void Application_EndRequest()
        {
            var context = new HttpContextWrapper(Context);
            if ((context.Response.StatusCode == 401) || (context.Response.StatusCode == 404))
            {
                context.Response.Redirect("/Event/Error");
            }
        }
        private void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();

            if (ex is HttpAntiForgeryException)
            {
                Response.Clear();
                Server.ClearError(); //make sure you log the exception first
                Response.Redirect("/Event/Error", true);
            }
            else if (ex is HttpException)
            {
                //Response.Redirect("/Survey/Error", true);
            }
        }

        public override void Init()
        {
            this.PostAuthenticateRequest += MvcApplication_PostAuthenticateRequest;
            base.Init();
        }

        void MvcApplication_PostAuthenticateRequest(object sender, EventArgs e)
        {
            var authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                string encTicket = authCookie.Value;
                if (!String.IsNullOrEmpty(encTicket))
                {
                    var ticket = FormsAuthentication.Decrypt(encTicket);
                    var id = new User_IDentity(ticket);
                    //var userRoles = Roles.GetRolesForUser(id.Name);
                    //var prin = new GenericPrincipal(id, userRoles);
                    //HttpContext.Current.User = prin;
                }
            }
            
        }
    }
}