using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace scrilla.Data
{
	public interface IUnitOfWork
	{
		void Commit();
	}
}
