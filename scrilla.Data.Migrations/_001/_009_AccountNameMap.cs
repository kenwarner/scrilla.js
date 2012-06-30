using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentMigrator;

namespace scrilla.Data.Migrations._001
{
	[Migration((long)Version._009)]
	public class _009_AccountNameMap : Migration
	{
		public override void Up()
		{
			Create.Table("AccountNameMap")
				.WithColumn("Id").AsInt32().Identity().PrimaryKey()
				.WithColumn("AccountId").AsInt32()
				.WithColumn("Name").AsString();

			Create.ForeignKey("FK__AccountNameMap__Account").FromTable("AccountNameMap").ForeignColumn("AccountId").ToTable("Account").PrimaryColumn("Id");
		}

		public override void Down()
		{
			Delete.ForeignKey("FK__AccountNameMap__Account").OnTable("Account");

			Delete.Table("AccountNameMap");
		}
	}
}
