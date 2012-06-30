using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentMigrator;

namespace scrilla.Data.Migrations._001
{
	[Migration((long)Version._002)]
	public class _002_Account : Migration
	{
		public override void Up()
		{
			Create.Table("AccountGroup")
				.WithColumn("Id").AsInt32().Identity().PrimaryKey()
				.WithColumn("Name").AsString()
				.WithColumn("IsActive").AsBoolean()
				.WithColumn("DisplayOrder").AsInt32();

			Create.Table("Account")
				.WithColumn("Id").AsInt32().Identity().PrimaryKey()
				.WithColumn("AccountGroupId").AsInt32()
				.WithColumn("DefaultCategoryId").AsInt32().Nullable()
				.WithColumn("Name").AsString()
				.WithColumn("InitialBalance").AsCurrency()
				.WithColumn("Balance").AsCurrency()
				.WithColumn("BalanceTimestamp").AsDateTime();

			Create.ForeignKey("FK__Account__AccountGroup").FromTable("Account").ForeignColumn("AccountGroupId").ToTable("AccountGroup").PrimaryColumn("Id");
		}

		public override void Down()
		{
			Delete.ForeignKey("FK__Account__AccountGroup").OnTable("Account");

			Delete.Table("Account");
			Delete.Table("AccountGroup");
		}
	}
}
