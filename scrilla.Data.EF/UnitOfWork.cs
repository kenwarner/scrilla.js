using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace scrilla.Data.EF
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly IDatabaseFactory databaseFactory;
		private scrillaContext dataContext;

		public UnitOfWork(IDatabaseFactory databaseFactory)
		{
			this.databaseFactory = databaseFactory;
		}

		protected scrillaContext DataContext
		{
			get { return dataContext ?? (dataContext = databaseFactory.Get()); }
		}

		public void Commit()
		{
			DataContext.SaveChanges();
		}
	}
}
