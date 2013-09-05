using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using DapperExtensions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scrilla.Data.Dapper
{
	public class DapperInstaller : IWindsorInstaller
	{
		public void Install(IWindsorContainer container, IConfigurationStore store)
		{
			container.Register(
				Component.For<IDbConnection>().UsingFactoryMethod(() => 
					{
						var connectionString = ConfigurationManager.ConnectionStrings["scrilla"];
						if (connectionString == null)
						{
							connectionString = ConfigurationManager.ConnectionStrings[1];
						}

						var sqlConnection = connectionString == null ? new SqlConnection() : new SqlConnection(connectionString.ConnectionString);
						sqlConnection.Open();
						return sqlConnection;
					}));
			container.Register(Component.For<IDatabase>().ImplementedBy<Db>());
		}
	}
}
