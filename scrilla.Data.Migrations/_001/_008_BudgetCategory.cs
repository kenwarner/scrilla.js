using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentMigrator;

namespace scrilla.Data.Migrations._001
{
	[Migration((long)Version._008)]
	public class _008_BudgetCategory : Migration
	{
		public override void Up()
		{
			Create.Table("BudgetCategory")
				.WithColumn("Id").AsInt32().Identity().PrimaryKey()
				.WithColumn("CategoryId").AsInt32()
				.WithColumn("Month").AsDateTime()
				.WithColumn("Amount").AsCurrency();

			Create.ForeignKey("FK__BudgetCategory__Category").FromTable("BudgetCategory").ForeignColumn("CategoryId").ToTable("Category").PrimaryColumn("Id");
		}

		public override void Down()
		{
			Delete.ForeignKey("FK__BudgetCategory__Category").OnTable("BudgetCategory");

			Delete.Table("BudgetCategory");
		}
	}
}
