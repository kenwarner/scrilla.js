using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scrilla.Data.Migrations._002
{
	[Migration((long)Version._013)]
	public class _013_BillMakeBillGroupNullable : Migration
	{
		public override void Up()
		{
			Alter.Column("BillGroupId").OnTable("Bill").AsInt32().Nullable();
		}

		public override void Down()
		{
			Alter.Column("BillGroupId").OnTable("Bill").AsInt32().NotNullable();
		}
	}
}
