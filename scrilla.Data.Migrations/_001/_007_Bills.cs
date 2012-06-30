using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentMigrator;

namespace scrilla.Data.Migrations._001
{
	[Migration((long)Version._007)]
	public class _007_Bills : Migration
	{
		public override void Up()
		{
			#region BillGroup

			Create.Table("BillGroup")
				.WithColumn("Id").AsInt32().Identity().PrimaryKey()
				.WithColumn("Name").AsString()
				.WithColumn("IsActive").AsBoolean()
				.WithColumn("DisplayOrder").AsInt32();

			#endregion

			#region Bill

			Create.Table("Bill")
				.WithColumn("Id").AsInt32().Identity().PrimaryKey()
				.WithColumn("BillGroupId").AsInt32()
				.WithColumn("VendorId").AsInt32().Nullable()
				.WithColumn("CategoryId").AsInt32().Nullable()
				.WithColumn("Name").AsString()
				.WithColumn("Amount").AsCurrency()
				.WithColumn("StartDate").AsDateTime()
				.WithColumn("StartDate2").AsDateTime().Nullable()
				.WithColumn("EndDate").AsDateTime()
				.WithColumn("EndDate2").AsDateTime().Nullable()
				.WithColumn("RecurrenceFrequency").AsInt32();

			Create.ForeignKey("FK__Bill__BillGroup").FromTable("Bill").ForeignColumn("BillGroupId").ToTable("BillGroup").PrimaryColumn("Id");
			Create.ForeignKey("FK__Bill__Vendor").FromTable("Bill").ForeignColumn("VendorId").ToTable("Vendor").PrimaryColumn("Id");
			Create.ForeignKey("FK__Bill__Category").FromTable("Bill").ForeignColumn("CategoryId").ToTable("Category").PrimaryColumn("Id");

			#endregion

			#region BillTransaction

			Create.Table("BillTransaction")
				.WithColumn("Id").AsInt32().Identity().PrimaryKey()
				.WithColumn("BillId").AsInt32()
				.WithColumn("CategoryId").AsInt32().Nullable()
				.WithColumn("VendorId").AsInt32().Nullable()
				.WithColumn("OriginalVendorId").AsInt32().Nullable()
				.WithColumn("OriginalCategoryId").AsInt32().Nullable()
				.WithColumn("Timestamp").AsDateTime()
				.WithColumn("Amount").AsCurrency()
				.WithColumn("IsPaid").AsBoolean()
				.WithColumn("OriginalTimestamp").AsDateTime()
				.WithColumn("OriginalAmount").AsCurrency();

			Alter.Table("Transaction")
				.AddColumn("BillTransactionId").AsInt32().Nullable();

			Create.ForeignKey("FK__BillTransaction__Bill").FromTable("BillTransaction").ForeignColumn("BillId").ToTable("Bill").PrimaryColumn("Id");
			Create.ForeignKey("FK__BillTransaction__Category").FromTable("BillTransaction").ForeignColumn("CategoryId").ToTable("Category").PrimaryColumn("Id");
			Create.ForeignKey("FK__BillTransaction__Vendor").FromTable("BillTransaction").ForeignColumn("VendorId").ToTable("Vendor").PrimaryColumn("Id");
			Create.ForeignKey("FK__BillTransaction__OriginalCategory").FromTable("BillTransaction").ForeignColumn("OriginalCategoryId").ToTable("Category").PrimaryColumn("Id");
			Create.ForeignKey("FK__BillTransaction__OriginalVendor").FromTable("BillTransaction").ForeignColumn("OriginalVendorId").ToTable("Vendor").PrimaryColumn("Id");
			Create.ForeignKey("FK__Transaction__BillTransaction").FromTable("Transaction").ForeignColumn("BillTransactionId").ToTable("BillTransaction").PrimaryColumn("Id");

			#endregion
		}

		public override void Down()
		{
			Delete.ForeignKey("FK__BillTransaction__Bill").OnTable("BillTransaction");
			Delete.ForeignKey("FK__BillTransaction__Category").OnTable("BillTransaction");
			Delete.ForeignKey("FK__BillTransaction__Vendor").OnTable("BillTransaction");
			Delete.ForeignKey("FK__BillTransaction__OriginalCategory").OnTable("BillTransaction");
			Delete.ForeignKey("FK__BillTransaction__OriginalVendor").OnTable("BillTransaction");
			Delete.ForeignKey("FK__Transaction__BillTransaction").OnTable("Transaction");
			Delete.Column("BillTransactionId").FromTable("Transaction");
			Delete.Table("BillTransaction");

			Delete.ForeignKey("FK__Bill__BillGroup").OnTable("Bill");
			Delete.ForeignKey("FK__Bill__Vendor").OnTable("Bill");
			Delete.ForeignKey("FK__Bill__Category").OnTable("Bill");
			Delete.Table("Bill");

			Delete.Table("BillGroup");
		}
	}
}
