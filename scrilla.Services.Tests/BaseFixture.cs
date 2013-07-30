﻿using DapperExtensions;
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

namespace scrilla.Services.Tests
{
	public class BaseFixture
	{
		protected Fixture _fixture;
		protected SqlConnection _sqlConnection;

		public BaseFixture()
		{
			CreateTestDatabase();

			var connectionString = ConfigurationManager.ConnectionStrings["TestsConnectionString"].ConnectionString;
			_sqlConnection = new SqlConnection(connectionString);
			_sqlConnection.Open();

			_fixture = new Fixture();
			_fixture.Inject<IDatabase>(new Db(_sqlConnection));
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