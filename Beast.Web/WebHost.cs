using Beast.Web;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace Beast.Web
{
    public class WebHost : IHost, IHttpModule
    {
        private static volatile bool _initialized;
        private static readonly object _initLock = new object();

        private Application _app;

        public void Dispose()
        {
            if (_app != null)
                _app.Dispose();
        }

        public void Init(HttpApplication context)
        {
            if (!_initialized)
            {
                lock (_initLock)
                {
                    if (!_initialized)
                    {
                        _initialized = true;
                        Start(context);
                    }
                }
            }
            OnInit(context);
        }

        private void Start(HttpApplication context)
        {
            var settings = ApplicationSettings.FromConfig();
            if (settings == null)
            {
                settings = ApplicationSettings.Default;
                settings.RootPath = context.Context.Server.MapPath("/");
                settings.ComponentDirectories.Add(context.Context.Server.MapPath("/bin"));
            }

            // Start the application
            _app = new Application(this, settings);
            _app.Run();

            InitSignalR(context, settings);

            OnStarted(context);
        }

        private void InitSignalR(HttpApplication context, ApplicationSettings settings)
        {
            var name = settings.GetValue(WebHostSettingKeys.SignalRRouteName, WebHostConnection.DefaultSignalRRouteName);
            var url = settings.GetValue(WebHostSettingKeys.SignalRUrl, WebHostConnection.DefaultSignalRUrl);

            GlobalHost.DependencyResolver.Register(typeof(WebHostConnection), () => new WebHostConnection(_app));

            var route = RouteTable.Routes.MapConnection<WebHostConnection>(name, url);

            RouteTable.Routes.Remove(route);
            for (int i = 0; i < RouteTable.Routes.Count; i++)
            {
                var rt = RouteTable.Routes[i];
                if (rt.GetType().Name.Contains("Ignore"))
                {
                    RouteTable.Routes.Insert(i + 1, route);
                    break;
                }
            }
        }

        protected virtual void OnStarted(HttpApplication context)
        {
        }

        protected virtual void OnInit(HttpApplication context)
        {
        }
    }
}
