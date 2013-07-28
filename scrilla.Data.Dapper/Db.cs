using DapperExtensions;
using DapperExtensions.Sql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scrilla.Data.Dapper
{
	public class Db : DapperExtensions.Database
	{
		public Db (IDbConnection connection)
			: this (connection, new SqlGeneratorImpl(new DapperExtensionsConfiguration()))
		{

		}

		public Db (IDbConnection connection, ISqlGenerator sqlGenerator)
			: base(connection, sqlGenerator)
		{

		}
	}
}
