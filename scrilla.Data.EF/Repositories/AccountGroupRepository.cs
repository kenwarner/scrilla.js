using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using scrilla.Data.Domain;
using scrilla.Data.Repositories;

namespace scrilla.Data.EF.Repositories
{
	public class AccountGroupRepository : RepositoryBase<AccountGroup>, IAccountGroupRepository
	{
		public AccountGroupRepository(IDatabaseFactory databaseFactory) : base(databaseFactory) { }
	}
}
