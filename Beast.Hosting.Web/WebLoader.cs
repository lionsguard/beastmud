using Beast.Hosting.Web.Security;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;

namespace Beast.Hosting.Web
{
    public class WebLoader
    {
        public static void Load()
        {
            DynamicModuleUtility.RegisterModule(typeof(WebHost));
            DynamicModuleUtility.RegisterModule(typeof(SecurityModule));
        }
    }
}
