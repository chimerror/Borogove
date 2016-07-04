using System.Data.Entity;
using System.Web;
using System.Web.Http;

namespace Borogove.API
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            DbConfiguration.SetConfiguration(new ApiDbConfiguration());
        }
    }
}
