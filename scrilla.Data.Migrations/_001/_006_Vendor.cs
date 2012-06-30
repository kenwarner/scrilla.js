using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentMigrator;

namespace scrilla.Data.Migrations._001
{
	[Migration((long)Version._006)]
	public class _006_Vendor : Migration
	{
		public override void Up()
		{
			Create.Table("Vendor")
				.WithColumn("Id").AsInt32().Identity().PrimaryKey()
				.WithColumn("Name").AsString()
				.WithColumn("DefaultCategoryId").AsInt32().Nullable();

			Alter.Table("Transaction")
				.AddColumn("VendorId").AsInt32().Nullable();

			Create.ForeignKey("FK__Transaction__Vendor").FromTable("Transaction").ForeignColumn("VendorId").ToTable("Vendor").PrimaryColumn("Id");
			Create.ForeignKey("FK__Vendor__Category").FromTable("Vendor").ForeignColumn("DefaultCategoryId").ToTable("Category").PrimaryColumn("Id");
		}

		public override void Down()
		{
			Delete.ForeignKey("FK__Transaction__Vendor").OnTable("Transaction");
			Delete.ForeignKey("FK__Vendor__Category").OnTable("Vendor");
			Delete.Column("VendorId").FromTable("Transaction");

			Delete.Table("Vendor");
		}
	}
}
