namespace CallCenterCrm.Web
{
    using System.Web.Optimization;

    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            RegisterStyles(bundles);
            RegisterScripts(bundles);            

            bundles.IgnoreList.Clear();

            BundleTable.EnableOptimizations = false;
        }

        private static void RegisterStyles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/bootstrap").Include(
                "~/Content/bootstrap.flatly.css"));

            bundles.Add(new StyleBundle("~/Content/site").Include(
                "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/kendo").Include(
                "~/Content/Kendo/kendo.common-bootstrap.min.css",
                "~/Content/Kendo/kendo.default.min.css"));
        }

        private static void RegisterScripts(BundleCollection bundles)
        {

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryajax").Include(
                "~/Scripts/jquery.unobtrusive-ajax*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap.js",
                "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/kendo").Include(
                "~/Scripts/Kendo/kendo.all.min.js",
                "~/Scripts/Kendo/kendo.aspnetmvc.min.js"));
        }
    }
}
