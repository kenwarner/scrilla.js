using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentMigrator;

namespace scrilla.Data.Migrations._001
{
	[Migration((long)Version._005)]
	public class _005_Category : Migration
	{
		public override void Up()
		{
			Create.Table("CategoryGroup")
				.WithColumn("Id").AsInt32().Identity().PrimaryKey()
				.WithColumn("Name").AsString()
				.WithColumn("DisplayOrder").AsInt32();

			Create.Table("Category")
				.WithColumn("Id").AsInt32().Identity().PrimaryKey()
				.WithColumn("CategoryGroupId").AsInt32()
				.WithColumn("Name").AsString();

			Create.ForeignKey("FK__Category__CategoryGroup").FromTable("Category").ForeignColumn("CategoryGroupId").ToTable("CategoryGroup").PrimaryColumn("Id");
			Create.ForeignKey("FK__Subtransaction__Category").FromTable("Subtransaction").ForeignColumn("CategoryId").ToTable("Category").PrimaryColumn("Id");
			Create.ForeignKey("FK__Account__Category").FromTable("Account").ForeignColumn("DefaultCategoryId").ToTable("Category").PrimaryColumn("Id");
		}

		public override void Down()
		{
			Delete.ForeignKey("FK__Account__Category").OnTable("Account");
			Delete.ForeignKey("FK__Subtransaction__Category").OnTable("Subtransaction");
			Delete.ForeignKey("FK__Category__CategoryGroup").OnTable("Category");

			Delete.Table("Category");
			Delete.Table("CategoryGroup");
		}
	}
}
