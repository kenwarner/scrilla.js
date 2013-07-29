using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using scrilla.Services;

using System.Data.SqlClient;
using System.Configuration;
using Dapper;
using Dapper.Contrib.Extensions;

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
			throw new NotImplementedException();
			//IAccountService accountService = new AccountService(();
			//TransactionImporter importer = new TransactionImporter();

			//importer.Import(filename);
		}
	}
}
