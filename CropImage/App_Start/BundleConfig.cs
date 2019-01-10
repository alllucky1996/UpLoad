using System.Web;
using System.Web.Optimization;

namespace CropImage
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
        }
        public static void RegisterMetronicBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Theme/assets/global/plugins/jquery.min.js",
                        "~/Theme/assets/global/plugins/bootstrap/js/bootstrap.min.js",
                        "~/Theme/assets/global/plugins/js.cookie.min.js",
                        "~/Theme/assets/global/plugins/jquery-slimscroll/jquery.slimscroll.min.js",
                        "~/Theme/assets/global/plugins/jquery.blockui.min.js",
                        "~/Theme/assets/global/plugins/bootstrap-switch/js/bootstrap-switch.min.js",
                        "~/Theme/assets/global/scripts/app.min.js",
                        "~/Theme/assets/layouts/layout4/scripts/layout.min.js",
                        "~/Theme/assets/layouts/layout4/scripts/demo.min.js",
                        "~/Theme/assets/layouts/global/scripts/quick-sidebar.min.js",
                        "~/Theme/assets/layouts/global/scripts/quick-nav.min.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Theme/assets/global/plugins/font-awesome/css/font-awesome.min.css",
                      "~/Theme/assets/global/plugins/simple-line-icons/simple-line-icons.min.css",
                      "~/Theme/assets/global/plugins/bootstrap/css/bootstrap.min.css",
                      "~/Theme/assets/global/plugins/bootstrap-switch/css/bootstrap-switch.min.css",
                      "~/Theme/assets/global/css/components.min.css",
                      "~/Theme/assets/global/css/plugins.min.css",
                      "~/Theme/assets/pages/css/image-crop.min.css",
                      "~/Theme/assets/layouts/layout4/css/layout.min.css",
                      "~/Theme/assets/layouts/layout4/css/themes/default.min.css",
                      "~/Theme/assets/layouts/layout4/css/custom.min.css"));
            // cuttom
           // crop image
            bundles.Add(new StyleBundle("~/Crop/css").Include("~/Theme/assets/global/plugins/jcrop/css/jquery.Jcrop.min.css"));
            bundles.Add(new ScriptBundle("~/Crop/jquery").Include(
                "~/Theme/assets/global/plugins/jcrop/js/jquery.color.js",
                "~/Theme/assets/global/plugins/jcrop/js/jquery.Jcrop.min.js",
                "~/Theme/assets/pages/scripts/form-image-crop.js"));
        }
    }
}
