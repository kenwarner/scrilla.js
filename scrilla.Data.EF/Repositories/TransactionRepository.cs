using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using scrilla.Data.Repositories;
using scrilla.Data.Domain;

namespace scrilla.Data.EF.Repositories
{
	public class TransactionRepository : RepositoryBase<Transaction>, ITransactionRepository
	{
		public TransactionRepository(IDatabaseFactory databaseFactory) : base(databaseFactory) { }
	}
}
