using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Mvc;

namespace scrilla.js.Web.Composition
{
	public class ControllersInstaller : IWindsorInstaller
	{
		public void Install(IWindsorContainer container, IConfigurationStore store)
		{
			// Install MVC Controllers
			container.Register(Classes
				.FromThisAssembly()
				.BasedOn<IController>()
				.LifestyleTransient());

			// Install WebApi Controllers
			container.Register(Classes
				.FromThisAssembly()
				.BasedOn<IHttpController>()
				.LifestyleTransient());
		}
	}
}