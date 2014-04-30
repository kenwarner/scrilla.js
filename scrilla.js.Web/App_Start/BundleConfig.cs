using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using Castle.Core.Internal;

namespace scrilla.js.Web
{
	public class BundleConfig
	{
		// For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.UseCdn = true;

			RegisterStyleBundles(bundles);
			RegisterScriptBundles(bundles);
		}

		private static void RegisterStyleBundles(BundleCollection bundles)
		{
			bundles.Add(new StyleBundle(Links.Bundles.Styles.app)
				.IncludeDirectory(VirtualPathUtility.ToAppRelative(Links.app.styles.Url()), "*.css", false));

			bundles.Add(new StyleBundle(Links.Bundles.Styles.lib) { Builder = new CssRewriteUrlBundleBuilder() }
				.IncludeDirectory("~/lib/", "*.css", true));
				//.Include(VirtualPathUtility.ToAppRelative(Links.lib.jquery_ui_1_10_3.css.smoothness.jquery_ui_1_10_3_custom_css)));
		}

		private static void RegisterScriptBundles(BundleCollection bundles)
		{
			bundles.Add(new ScriptBundle(Links.Bundles.Scripts.app)
				.IncludeDirectory(VirtualPathUtility.ToAppRelative(Links.app.Url()), "*.js", true));

			bundles.Add(new ScriptBundle(Links.Bundles.Scripts.lib) {Orderer = new JQueryUIBundleOrderer()}
				.IncludeDirectory("~/lib/", "*.js", true));

		}
	}

	class CssRewriteUrlBundleBuilder : IBundleBuilder
	{
		public string BuildBundleContent(Bundle bundle, BundleContext context, IEnumerable<BundleFile> files)
		{
			var rewriteTransform = new CssRewriteUrlTransform();
			var bundleFiles = files as IList<BundleFile> ?? files.ToList();
			bundleFiles.ForEach(x => x.Transforms.Add(rewriteTransform));
			return new DefaultBundleBuilder().BuildBundleContent(bundle, context, bundleFiles);
		}
	}

	class JQueryUIBundleOrderer : DefaultBundleOrderer
	{
		public override IEnumerable<BundleFile> OrderFiles(BundleContext context, IEnumerable<BundleFile> files)
		{
			List<BundleFile> orderedFiles = new List<BundleFile>(base.OrderFiles(context, files));

			// core comes first, then widget, then mouse
			var widgetBundle = orderedFiles.Find(x => x.IncludedVirtualPath.Contains("jquery.ui.widget"));
			var mouseBundle = orderedFiles.Find(x => x.IncludedVirtualPath.Contains("jquery.ui.mouse"));
			orderedFiles.Remove(widgetBundle);
			orderedFiles.Remove(mouseBundle);

			var coreIndex = orderedFiles.FindIndex(x => x.IncludedVirtualPath.Contains("jquery.ui.core"));
			orderedFiles.Insert(coreIndex + 1, widgetBundle);
			orderedFiles.Insert(coreIndex + 2, mouseBundle);

			return orderedFiles;
		}
	}
}

namespace Links
{
	public static partial class Bundles
	{
		public static partial class Styles
		{
			public static readonly string app = "~/bundles/styles/app";
			public static readonly string lib = "~/bundles/styles/lib";
		}

		public static partial class Scripts
		{
			public static readonly string app = "~/bundles/scripts/app";
			public static readonly string lib = "~/bundles/scripts/lib";
		}
	}
}

