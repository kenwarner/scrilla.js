using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scrilla.Data.Migrations._002
{

	[Migration((long)Version._011)]
	public class _011_AccountMakeAccountGroupNullable : Migration
	{
		public override void Up()
		{
			Alter.Column("AccountGroupId").OnTable("Account").AsInt32().Nullable();
		}

		public override void Down()
		{
			Alter.Column("AccountGroupId").OnTable("Account").AsInt32().NotNullable();
		}
	}
}
