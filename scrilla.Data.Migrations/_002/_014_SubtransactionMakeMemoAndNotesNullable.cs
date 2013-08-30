using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scrilla.Data.Migrations._002
{
	[Migration((long)Version._014)]
	public class _014_SubtransactionMakeMemoAndNotesNullable : Migration
	{
		public override void Up()
		{
			Alter.Column("Memo").OnTable("Subtransaction").AsString().Nullable();
			Alter.Column("Notes").OnTable("Subtransaction").AsString().Nullable();
		}

		public override void Down()
		{
			Alter.Column("Memo").OnTable("Subtransaction").AsString().NotNullable();
			Alter.Column("Notes").OnTable("Subtransaction").AsString().NotNullable();
		}
	}
}
