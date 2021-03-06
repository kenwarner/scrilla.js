﻿using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Optimization;
using scrilla.js.Web.Composition;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.Windsor;

namespace scrilla.js.Web
{
	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801

	public class WebApiApplication : System.Web.HttpApplication
	{
		private readonly IWindsorContainer _container;

		public WebApiApplication()
		{
			_container = new WindsorContainer();
		}

		protected void Application_Start()
		{
			CompositionConfig.RegisterInstallers(_container);
			SetControllerComposition();

			AreaRegistration.RegisterAllAreas();
			GlobalConfiguration.Configure(WebApiConfig.Register);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);

			BundleTable.EnableOptimizations = true;
		}

		private void SetControllerComposition()
		{
			var controllerFactory = new WindsorControllerFactory(_container.Kernel);
			ControllerBuilder.Current.SetControllerFactory(controllerFactory);
			GlobalConfiguration.Configuration.Services.Replace(typeof(IHttpControllerActivator), new WindsorCompositionRoot(_container));
		}

		protected void Application_End()
		{
			_container.Dispose();
		}

		public override void Dispose()
		{
			_container.Dispose();
			base.Dispose();
		}
	}
}
