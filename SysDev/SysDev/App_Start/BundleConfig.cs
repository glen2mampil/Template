using System.Web;
using System.Web.Optimization;

namespace SysDev
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/datatables/jquery.datatables.js",
                      "~/Scripts/dataTables/datatables.bootstrap.js"
                      ));


            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap.css",
                "~/admin-lte/css/AdminLTE.css",
                "~/admin-lte/css/skins/skin-blue.css",
                "~/Content/font-awesome.min.css",
                "~/Content/sweetalert2/dist/sweetalert2.min.css",
                "~/Content/plugins/iCheck/square/blue.css",
                "~/Content/dataTables/css/datatables.bootstrap.css",
                "~/Content/Site.css"));

            bundles.Add(new ScriptBundle("~/admin-lte/js").Include(
                
                "~/Content/sweetalert2/dist/sweetalert2.min.js",
                "~/Content/plugins/iCheck/icheck.min.js",
                "~/Content/components/fastclick/lib/fastclick.js",
                "~/Content/components/jquery-slimscroll/jquery.slimscroll.min.js",
                "~/admin-lte/js/app.js"));
        }
    }
}
