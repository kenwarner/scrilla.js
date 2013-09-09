using DapperExtensions;
using Ploeh.AutoFixture;
using scrilla.Data.Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace scrilla.Services.Tests
{
	public class BaseFixture<T>
	{
		protected T _sut;
		protected Fixture _fixture;
		protected SqlConnection _sqlConnection;

		/// <summary>
		/// Create a fixture using a newly created database
		/// </summary>
		public BaseFixture()
		{
			CreateTestDatabase();

			OpenConnection("TestsConnectionString");
			CreateFixture();
		}

		/// <summary>
		/// Create a fixture using an existing database
		/// </summary>
		/// <param name="connectionStringName">The name of the connection string</param>
		public BaseFixture(string connectionStringName)
		{
			OpenConnection(connectionStringName);
			CreateFixture();
		}

		private void OpenConnection(string connectionStringName)
		{
			var connectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
			_sqlConnection = new SqlConnection(connectionString);
			_sqlConnection.Open();
		}

		private void CreateFixture()
		{
			_fixture = new Fixture();
			_fixture.Inject<IDatabase>(new Db(_sqlConnection));

			_fixture.Register<IAccountService>(() => _fixture.Create<AccountService>());
			_fixture.Register<IBillService>(() => _fixture.Create<BillService>());
			_fixture.Register<IBudgetService>(() => _fixture.Create<BudgetService>());
			_fixture.Register<ICategoryService>(() => _fixture.Create<CategoryService>());
			_fixture.Register<ITransactionService>(() => _fixture.Create<TransactionService>());
			_fixture.Register<ITransactionImportService>(() => _fixture.Create<TransactionImportService>());
			_fixture.Register<IVendorService>(() => _fixture.Create<VendorService>());
			_sut = _fixture.Create<T>();
		}

		private void CreateTestDatabase()
		{
			var startInfo = new ProcessStartInfo()
			{
				WorkingDirectory = @"..\..\..\scrilla.Data.Migrations\tests\",
				FileName = "scratch.bat",
				WindowStyle = ProcessWindowStyle.Hidden
			};
			var process = Process.Start(startInfo);
			process.WaitForExit();
			Assert.Equal(0, process.ExitCode);
		}

		~BaseFixture()
		{
			if (_sqlConnection != null)
			{
				if (_sqlConnection.State != System.Data.ConnectionState.Closed)
					_sqlConnection.Close();

				_sqlConnection.Dispose();
			}
		}
	}
}
