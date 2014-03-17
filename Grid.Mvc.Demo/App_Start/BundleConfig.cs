using System.Web;
using System.Web.Optimization;

namespace Grid.Mvc.Demo
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {

            var siteJsBundle = new ScriptBundle("~/bundles/siteJs.js")
                .Include("~/Scripts/jquery-{version}.js", "~/Scripts/bootstrap.min.js",
                "~/Scripts/ladda-bootstrap/spin.min.js", "~/Scripts/ladda-bootstrap/ladda.min.js",
                "~/Scripts/URI.js",
                "~/Scripts/gridmvc.js", "~/Scripts/gridmvc-ext.js");

            bundles.Add(siteJsBundle);

            var gridJsDemo = new ScriptBundle("~/bundles/gridDemo.js")
               .Include("~/Scripts/gridmvcajax.demo.js");

            bundles.Add(gridJsDemo);
        }
    }
}