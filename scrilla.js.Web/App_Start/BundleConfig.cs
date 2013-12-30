using System.Web;
using System.Web.Optimization;

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

			bundles.Add(new StyleBundle(Links.Bundles.Styles.bootstrap)
				.Include(VirtualPathUtility.ToAppRelative(Links.lib.bootstrap_3_0_3.bootstrap_css))
				.Include(VirtualPathUtility.ToAppRelative(Links.lib.bootstrap_3_0_3.bootstrap_theme_css)));

			bundles.Add(new StyleBundle(Links.Bundles.Styles.chosen)
				.Include(VirtualPathUtility.ToAppRelative(Links.lib.chosen_1_0_0.chosen_css)));

			bundles.Add(new StyleBundle(Links.Bundles.Styles.jqRangeSlider)
				.Include(VirtualPathUtility.ToAppRelative(Links.lib.jQRangeSlider_5_5_0.css.classic_css))
				.Include(VirtualPathUtility.ToAppRelative(Links.lib.jquery_ui_1_10_3.css.smoothness.jquery_ui_1_10_3_custom_css)));

			bundles.Add(new StyleBundle(Links.Bundles.Styles.ngGrid)
				.Include(VirtualPathUtility.ToAppRelative(Links.lib.ng_grid_2_0_7.ng_grid_css)));
		}

		private static void RegisterScriptBundles(BundleCollection bundles)
		{
			bundles.Add(new ScriptBundle(Links.Bundles.Scripts.app)
				.IncludeDirectory(VirtualPathUtility.ToAppRelative(Links.app.Url()), "*.js", true));

			bundles.Add(new ScriptBundle(Links.Bundles.Scripts.angularjs,
				"//ajax.googleapis.com/ajax/libs/angularjs/1.2.4/angular.min.js") { CdnFallbackExpression = "window.angular" }
				.Include(VirtualPathUtility.ToAppRelative(Links.lib.angularjs_1_2_4.angular_js)));

			bundles.Add(new ScriptBundle(Links.Bundles.Scripts.angularjsRoute,
				"//ajax.googleapis.com/ajax/libs/angularjs/1.2.4/angular-route.min.js") { CdnFallbackExpression = "function() { try { window.angular.module('ngRoute'); } catch(e) { return false; } return true; })(" }
				.Include(VirtualPathUtility.ToAppRelative(Links.lib.angularjs_1_2_4.angular_route_js)));

			bundles.Add(new ScriptBundle(Links.Bundles.Scripts.angularjsResource,
				"//ajax.googleapis.com/ajax/libs/angularjs/1.2.4/angular-resource.min.js") { CdnFallbackExpression = "function() { try { window.angular.module('ngResource'); } catch(e) { return false; } return true; })(" }
				.Include(VirtualPathUtility.ToAppRelative(Links.lib.angularjs_1_2_4.angular_resource_js)));

			bundles.Add(new ScriptBundle(Links.Bundles.Scripts.bootstrap,
				"//netdna.bootstrapcdn.com/bootstrap/3.0.3/js/bootstrap.min.js") { CdnFallbackExpression = "$.fn.button" }
				.Include(VirtualPathUtility.ToAppRelative(Links.lib.bootstrap_3_0_3.bootstrap_js)));

			bundles.Add(new ScriptBundle(Links.Bundles.Scripts.chosen,
				"//cdnjs.cloudflare.com/ajax/libs/chosen/1.0/chosen.jquery.min.js") { CdnFallbackExpression = "$.fn.chosen" }
				.Include(VirtualPathUtility.ToAppRelative(Links.lib.chosen_1_0_0.chosen_jquery_js)));

			bundles.Add(new ScriptBundle(Links.Bundles.Scripts.jquery,
				"//code.jquery.com/jquery-2.0.3.min.js") { CdnFallbackExpression = "window.jQuery" }
				.Include(VirtualPathUtility.ToAppRelative(Links.lib.jquery_2_0_3.jquery_2_0_3_js)));

			bundles.Add(new ScriptBundle(Links.Bundles.Scripts.jqRangeSlider)
				.Include(VirtualPathUtility.ToAppRelative(Links.lib.jQRangeSlider_5_5_0.jQDateRangeSlider_withRuler_min_js))
				.Include(VirtualPathUtility.ToAppRelative(Links.lib.jquery_ui_1_10_3.js.jquery_ui_1_10_3_custom_js)));

			bundles.Add(new ScriptBundle(Links.Bundles.Scripts.ngGrid)
				.Include(VirtualPathUtility.ToAppRelative(Links.lib.ng_grid_2_0_7.ng_grid_2_0_7_debug_js)));

			bundles.Add(new ScriptBundle(Links.Bundles.Scripts.underscore,
				"//cdnjs.cloudflare.com/ajax/libs/underscore.js/1.5.2/underscore-min.js") { CdnFallbackExpression = "window._" }
				.Include(VirtualPathUtility.ToAppRelative(Links.lib.underscore_1_5_2.underscore_js)));
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
			public static readonly string bootstrap = "~/bundles/styles/bootstrap";
			public static readonly string chosen = "~/bundles/styles/chosen";
			public static readonly string jqRangeSlider = "~/bundles/styles/jq-range-slider";
			public static readonly string ngGrid = "~/bundles/styles/ng-grid";
		}

		public static partial class Scripts
		{
			public static readonly string app = "~/bundles/scripts/app";
			public static readonly string angularjs = "~/bundles/scripts/angularjs";
			public static readonly string angularjsResource = "~/bundles/scripts/angularjs-resource";
			public static readonly string angularjsRoute = "~/bundles/scripts/angularjs-route";
			public static readonly string bootstrap = "~/bundles/scripts/bootstrap";
			public static readonly string chosen = "~/bundles/scripts/chosen";
			public static readonly string jquery = "~/bundles/scripts/jquery";
			public static readonly string jqRangeSlider = "~/bundles/scripts/jq-range-slider";
			public static readonly string ngGrid = "~/bundles/scripts/ng-grid";
			public static readonly string underscore = "~/bundles/scripts/underscore";

			public static readonly string[] scrilla = 
			{
				Scripts.jquery,
				Scripts.underscore,
				Scripts.chosen,
				Scripts.bootstrap,
				Scripts.angularjs,
				Scripts.angularjsRoute,
				Scripts.angularjsResource,
				Scripts.jqRangeSlider,
				Scripts.ngGrid,
				Scripts.app
			};
		}
	}
}

