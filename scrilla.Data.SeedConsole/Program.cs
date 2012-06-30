using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using scrilla.Data.EF;
using scrilla.Services;
using scrilla.Data.EF.Repositories;
using scrilla.Data.Repositories;
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
			IDatabaseFactory databaseFactory = new DatabaseFactory();
			IUnitOfWork unitOfWork = new UnitOfWork(databaseFactory);
			IAccountRepository accountRepository = new AccountRepository(databaseFactory);
			ITransactionRepository transactionRepository = new TransactionRepository(databaseFactory);
			ICategoryRepository categoryRepository = new CategoryRepository(databaseFactory);
			IVendorRepository vendorRepository = new VendorRepository(databaseFactory);
			ICategoryGroupRepository categoryGroupRepository = new CategoryGroupRepository(databaseFactory);
			IBillRepository billRepository = new BillRepository(databaseFactory);
			IBillTransactionRepository billTransactionRepository = new BillTransactionRepository(databaseFactory);
			IBillGroupRepository billGroupRepository = new BillGroupRepository(databaseFactory);
			IBudgetCategoryRepository budgetCategoryRepository = new BudgetCategoryRepository(databaseFactory);
			IAccountGroupRepository accountGroupRepository = new AccountGroupRepository(databaseFactory);
			IImportDescriptionVendorMapRepository importDescriptionVendorMapRepository = new ImportDescriptionVendorMapRepository(databaseFactory);
			IAccountService accountService = new AccountService(unitOfWork, accountRepository, transactionRepository, categoryRepository, vendorRepository, billRepository, billTransactionRepository, billGroupRepository, categoryGroupRepository, budgetCategoryRepository, importDescriptionVendorMapRepository);
			TransactionImporter importer = new TransactionImporter(unitOfWork, accountService, accountRepository, transactionRepository, vendorRepository, categoryGroupRepository, accountGroupRepository, importDescriptionVendorMapRepository);

			importer.Import(filename);
		}
	}
}
