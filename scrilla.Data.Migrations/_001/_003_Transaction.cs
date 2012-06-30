using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentMigrator;

namespace scrilla.Data.Migrations._001
{
	[Migration((long)Version._003)]
	public class _003_Transaction : Migration
	{
		public override void Up()
		{
			Create.Table("Transaction")
				.WithColumn("Id").AsInt32().Identity().PrimaryKey()
				.WithColumn("AccountId").AsInt32()
				.WithColumn("Timestamp").AsDateTime()
				.WithColumn("OriginalTimestamp").AsDateTime()
				.WithColumn("Amount").AsCurrency()
				.WithColumn("IsReconciled").AsBoolean();

			Create.ForeignKey("FK__Transaction__Account").FromTable("Transaction").ForeignColumn("AccountId").ToTable("Account").PrimaryColumn("Id");
		}

		public override void Down()
		{
			Delete.ForeignKey("FK__Transaction__Account").OnTable("Transaction");

			Delete.Table("Transaction");
		}
	}
}
