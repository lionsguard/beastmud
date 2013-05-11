using Beast.Web;
using System.Web;

[assembly: PreApplicationStartMethod(typeof(WebLoader), "Load")]