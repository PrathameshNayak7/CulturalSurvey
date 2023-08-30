using Microsoft.Owin;
using Owin;
using System;
using System.Threading.Tasks;


[assembly: OwinStartup(typeof(CulturaSurvey.App_Start.Startup))]

namespace CulturaSurvey.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            
            app.MapSignalR();
        }
    }
}
