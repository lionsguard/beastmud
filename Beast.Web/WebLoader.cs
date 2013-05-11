using Microsoft.Web.Infrastructure.DynamicModuleHelper;

namespace Beast.Web
{
    public class WebLoader
    {
        public static void Load()
        {
            DynamicModuleUtility.RegisterModule(typeof(WebHost));
        }
    }
}
