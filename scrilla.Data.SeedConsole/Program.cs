using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using scrilla.Services;
using System.Data.SqlClient;
using System.Configuration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using scrilla.Data.Dapper;

namespace scrilla.Data.SeedConsole
{
	class Program
	{
		static void Main(string[] args)
		{
			SeedDataInitializer seeder = new SeedDataInitializer();
			seeder.Initialize(args[0]);
		}
	}

	internal class SeedDataInitializer
	{
		public void Initialize(string filename)
		{
			using (var container = new WindsorContainer())
			{
				container.Install(FromAssembly.Containing<DapperInstaller>());
				container.Install(FromAssembly.Containing<ServicesInstaller>());
				var importService = container.Resolve<ITransactionImportService>();
				importService.Import(filename);
			}
		}
	}
}
