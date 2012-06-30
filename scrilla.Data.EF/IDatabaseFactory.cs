using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace scrilla.Data.EF
{
	public interface IDatabaseFactory : IDisposable
	{
		scrillaContext Get();
	}
}
