using DapperExtensions;
using DapperExtensions.Mapper;
using DapperExtensions.Sql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace scrilla.Data.Dapper
{
	public class Db : DapperExtensions.Database
	{
		public Db(IDbConnection connection)
			: this(connection, new SqlGeneratorImpl(
				new DapperExtensionsConfiguration(
					typeof(AutoClassMapper<>),
					new List<Assembly>() { typeof(Db).Assembly },
					new SqlServerDialect())))
		{

		}

		public Db(IDbConnection connection, ISqlGenerator sqlGenerator)
			: base(connection, sqlGenerator)
		{

		}
	}

	public class TransactionMapper : ClassMapper<Transaction>
	{
		public TransactionMapper()
		{
			Map(x => x.AccountName).Ignore();
			Map(x => x.VendorName).Ignore();
			Map(x => x.Subtransactions).Ignore();
			AutoMap();
		}
	}

	public class SubtransactionMapper : ClassMapper<Subtransaction>
	{
		public SubtransactionMapper()
		{
			Map(x => x.CategoryName).Ignore();
			AutoMap();
		}
	}

}
