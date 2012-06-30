using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace scrilla.Data.EF
{
	public class DatabaseFactory : Disposable, IDatabaseFactory
	{
		private scrillaContext dataContext;
		public scrillaContext Get()
		{
			return dataContext ?? (dataContext = new scrillaContext());
		}
		protected override void DisposeCore()
		{
			if (dataContext != null)
				dataContext.Dispose();
		}
	}
}
