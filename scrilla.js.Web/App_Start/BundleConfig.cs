using System.Web;
using System.Web.Optimization;

namespace scrilla.js.Web
{
	public class BundleConfig
	{
		// For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.Add(new ScriptBundle(Links.Bundles.Scripts.jquery).Include(Links.scripts.jquery_2_0_3_js));
			bundles.Add(new StyleBundle(Links.Bundles.Styles.site).Include(Links.Content.site_css));
		}
	}
}

namespace Links
{
	public static partial class Bundles
	{
		public static partial class Scripts
		{
			public static readonly string jquery = "~/bundles/scripts/jquery";
			public static readonly string bootstrap = "~/bundles/scripts/bootstrap";
		}
		public static partial class Styles
		{
			public static readonly string site = "~/bundles/styles/site";
			public static readonly string bootstrap = "~/bundles/styles/bootstrap";
			public static readonly string bootstrapTheme = "~/bundles/styles/bootstrap-theme";
		}
	}
}

