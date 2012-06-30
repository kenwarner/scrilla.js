using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentMigrator;

namespace scrilla.Data.Migrations._001
{
	[Migration((long)Version._010)]
	public class _010_ImportDescriptionVendorMap : Migration
	{
		public override void Up()
		{
			Create.Table("ImportDescriptionVendorMap")
				.WithColumn("Id").AsInt32().Identity().PrimaryKey()
				.WithColumn("Description").AsString()
				.WithColumn("VendorId").AsInt32();

			Create.ForeignKey("FK__ImportDescriptionVendorMap__Vendor").FromTable("ImportDescriptionVendorMap").ForeignColumn("VendorId").ToTable("Vendor").PrimaryColumn("Id");
		}

		public override void Down()
		{
			Delete.ForeignKey("FK__ImportDescriptionVendorMap__Vendor").OnTable("ImportDescriptionVendorMap");

			Delete.Table("ImportDescriptionVendorMap");
		}
	}
}
