using System.Web.Optimization;

[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(scrilla.js.Web.App_Start.BootstrapBundleConfig), "RegisterBundles")]

namespace scrilla.js.Web.App_Start
{
	public class BootstrapBundleConfig
	{
		public static void RegisterBundles()
		{
			// Add @Styles.Render("~/Content/bootstrap/base") in the <head/> of your _Layout.cshtml view
			// For Bootstrap theme add @Styles.Render("~/Content/bootstrap/theme") in the <head/> of your _Layout.cshtml view
			// Add @Scripts.Render("~/bundles/bootstrap") after jQuery in your _Layout.cshtml view
			// When <compilation debug="true" />, MVC4 will render the full readable version. When set to <compilation debug="false" />, the minified version will be rendered automatically
			BundleTable.Bundles.Add(new ScriptBundle(Links.Bundles.Scripts.bootstrap).Include(Links.scripts.bootstrap_js));
			BundleTable.Bundles.Add(new StyleBundle(Links.Bundles.Styles.bootstrap).Include(Links.Content.bootstrap.bootstrap_css));
			BundleTable.Bundles.Add(new StyleBundle(Links.Bundles.Styles.bootstrapTheme).Include(Links.Content.bootstrap.bootstrap_theme_css));
		}
	}
}
