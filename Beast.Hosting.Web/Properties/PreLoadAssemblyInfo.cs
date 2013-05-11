using Beast.Hosting.Web;
using System.Web;

[assembly: PreApplicationStartMethod(typeof(WebLoader), "Load")]