using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentMigrator;

namespace scrilla.Data.Migrations._001
{
	[Migration((long)Version._004)]
	public class _004_Subtransaction : Migration
	{
		public override void Up()
		{
			Create.Table("Subtransaction")
				.WithColumn("Id").AsInt32().Identity().PrimaryKey()
				.WithColumn("TransactionId").AsInt32()
				.WithColumn("CategoryId").AsInt32().Nullable()
				.WithColumn("Amount").AsCurrency()
				.WithColumn("Memo").AsString()
				.WithColumn("Notes").AsString()
				.WithColumn("IsExcludedFromBudget").AsBoolean()
				.WithColumn("IsTransfer").AsBoolean();

			Create.ForeignKey("FK__Subtransaction__Transaction").FromTable("Subtransaction").ForeignColumn("TransactionId").ToTable("Transaction").PrimaryColumn("Id");
		}

		public override void Down()
		{
			Delete.ForeignKey("FK__Subtransaction__Transaction").OnTable("Subtransaction");

			Delete.Table("Subtransaction");
		}
	}
}
