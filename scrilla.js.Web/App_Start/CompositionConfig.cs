using Castle.Windsor;
using Castle.Windsor.Installer;
using scrilla.Data.Dapper;
using scrilla.js.Web.Composition;
using scrilla.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Mvc;

namespace scrilla.js.Web
{
	public class CompositionConfig
	{
		public static void RegisterInstallers(IWindsorContainer container)
		{
			container.Install(FromAssembly.This());
			container.Install(FromAssembly.Containing<DapperInstaller>());
			container.Install(FromAssembly.Containing<ServicesInstaller>());
		}
	}
}