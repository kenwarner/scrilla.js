using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using scrilla.Data.Repositories;
using scrilla.Data.Domain;

namespace scrilla.Data.EF.Repositories
{
	public class BudgetCategoryRepository : RepositoryBase<BudgetCategory>, IBudgetCategoryRepository
	{
		public BudgetCategoryRepository(IDatabaseFactory databaseFactory) : base(databaseFactory) { }
	}
}
