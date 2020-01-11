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
                "~/Scripts/share.js",
                "~/Scripts/DataTables-1.10.18/js/jquery.dataTables.min.js",
                "~/Scripts/flot-0.8.3/jquery.flot.min.js",
                "~/Scripts/flot-0.8.3/jquery.flot.pie.min.js"));

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
                "~/Content/style.css"));
            bundles.Add(new StyleBundle("~/Scripts/DataTables-1.10.18/css/DataTables").Include(  //為了抓到sort_asc.png,sort_both.png,sort_desc.png圖檔，虛擬路徑只好跟實體路徑一樣
                "~/Scripts/DataTables-1.10.18/css/jquery.dataTables.min.css"));
        }
    }
}
