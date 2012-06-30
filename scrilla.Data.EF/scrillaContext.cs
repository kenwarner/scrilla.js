using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using scrilla.Data.EF.Mapping;
using scrilla.Data.Domain;

namespace scrilla.Data.EF
{
	public class scrillaContext : DbContext
	{
		static scrillaContext()
		{
			Database.SetInitializer<scrillaContext>(null);
		}

		public DbSet<AccountGroup> AccountGroups { get; set; }
		public DbSet<Account> Accounts { get; set; }
		public DbSet<BillGroup> BillGroups { get; set; }
		public DbSet<Bill> Bills { get; set; }
		public DbSet<BillTransaction> BillTransactions { get; set; }
		public DbSet<BudgetCategory> BudgetCategories { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<CategoryGroup> CategoryGroups { get; set; }
		public DbSet<Subtransaction> Subtransactions { get; set; }
		public DbSet<Transaction> Transactions { get; set; }
		public DbSet<Vendor> Vendors { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Conventions.Remove<IncludeMetadataConvention>();
			modelBuilder.Configurations.Add(new AccountGroupMap());
			modelBuilder.Configurations.Add(new AccountMap());
			modelBuilder.Configurations.Add(new AccountNameMapMap());
			modelBuilder.Configurations.Add(new BillGroupMap());
			modelBuilder.Configurations.Add(new BillMap());
			modelBuilder.Configurations.Add(new BillTransactionMap());
			modelBuilder.Configurations.Add(new BudgetCategoryMap());
			modelBuilder.Configurations.Add(new CategoryGroupMap());
			modelBuilder.Configurations.Add(new CategoryMap());
			modelBuilder.Configurations.Add(new ImportDescriptionVendorMapMap());
			modelBuilder.Configurations.Add(new SubtransactionMap());
			modelBuilder.Configurations.Add(new TransactionMap());
			modelBuilder.Configurations.Add(new VendorMap());
		}
	}
}

