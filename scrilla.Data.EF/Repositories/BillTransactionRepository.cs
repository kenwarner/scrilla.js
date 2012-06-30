using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using scrilla.Data.Repositories;
using scrilla.Data.Domain;

namespace scrilla.Data.EF.Repositories
{
	public class BillTransactionRepository : RepositoryBase<BillTransaction>, IBillTransactionRepository
	{
		public BillTransactionRepository(IDatabaseFactory databaseFactory) : base(databaseFactory) { }
	}
}
