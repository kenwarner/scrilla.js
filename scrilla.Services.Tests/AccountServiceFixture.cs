using DapperExtensions;
using DapperExtensions.Sql;
using scrilla.Data.Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace scrilla.Services.Tests
{
	public class AccountServiceFixture
	{
		#region Setup

		protected SqlConnection _sqlConnection;

		public AccountServiceFixture()
		{
			CreateTestDatabase();

			var connectionString = ConfigurationManager.ConnectionStrings[0].ConnectionString;
			_sqlConnection = new SqlConnection(connectionString);
		}

		private void CreateTestDatabase()
		{
			var startInfo = new ProcessStartInfo()
			{
				WorkingDirectory = @"..\..\..\scrilla.Data.Migrations\tests\",
				FileName = "scratch.bat",
				WindowStyle = ProcessWindowStyle.Hidden
			};
			Process.Start(startInfo).WaitForExit();
		}

		~AccountServiceFixture()
		{
			if (_sqlConnection != null)
				_sqlConnection.Dispose();
		}

		#endregion

		#region GetAllTransactionsTests
		[Fact]
		public void GetAccount()
		{
			//var target = new AccountService(new Db(_sqlConnection));
			//var result = target.GetAllTransactions(new DateTime(2012, 1, 1));
			//Assert.True(result.Result.Count() == 628);

			//result = target.GetAllTransactions();
			//Assert.True(result.Result.Count() == 5208);

			//result = target.GetAllTransactions(SqlDateTime.MinValue.Value, new DateTime(2012, 1, 1));
			//Assert.True(result.Result.Count() == 4580);
		}

		#endregion

	}
}
