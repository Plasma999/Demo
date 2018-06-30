using System.Web;
using System.Web.Optimization;

namespace APIDemo
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jQuery-3.3.1/jquery-3.3.1.min.js",
                        "~/Scripts/jquery-extend.js",
                        "~/Scripts/DataTables-1.10.18/js/jquery.dataTables.min.js",
                        "~/Scripts/DataTables-1.10.18/js/dataTables.bootstrap.min.js",
                        "~/Scripts/DataTables-1.10.18/js/dataTables.bootstrap4.min.js",
                        "~/Scripts/DataTables-1.10.18/js/dataTables.foundation.min.js",
                        "~/Scripts/DataTables-1.10.18/js/dataTables.jqueryui.min.js",
                        "~/Scripts/DataTables-1.10.18/js/dataTables.semanticui.min.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/animate.min.css",
                      "~/Content/bootstrap.css",
                      "~/Content/fonts.css",
                      "~/Content/patentcloud-icon.css",
                      "~/Content/plugins.css",
                      "~/Content/site.css",
                      "~/Content/patentcloud.css",
                      "~/Content/jquery.smartmenus.bootstrap.css",
                      "~/Content/style.css",
                      "~/Scripts/DataTables-1.10.18/css/jquery.dataTables.min.css",
                      "~/Scripts/DataTables-1.10.18/css/dataTables.bootstrap.min.css",
                      "~/Scripts/DataTables-1.10.18/css/dataTables.bootstrap4.min.css",
                      "~/Scripts/DataTables-1.10.18/css/dataTables.foundation.min.css",
                      "~/Scripts/DataTables-1.10.18/css/dataTables.jqueryui.min.css",
                      "~/Scripts/DataTables-1.10.18/css/dataTables.semanticui.min.css"));
        }
    }
}
