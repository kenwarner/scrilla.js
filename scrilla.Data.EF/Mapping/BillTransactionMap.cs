using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Common;
using System.Data.Entity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using scrilla.Data.Domain;

namespace scrilla.Data.EF.Mapping
{
	public class BillTransactionMap : EntityTypeConfiguration<BillTransaction>
	{
		public BillTransactionMap()
		{
			// Primary Key
			this.HasKey(t => t.Id);

			// Properties
			// Table & Column Mappings
			this.ToTable("BillTransaction");
			this.Property(t => t.Id).HasColumnName("Id");
			this.Property(t => t.BillId).HasColumnName("BillId");
			this.Property(t => t.Amount).HasColumnName("Amount");
			this.Property(t => t.IsPaid).HasColumnName("IsPaid");
			this.Property(t => t.CategoryId).HasColumnName("CategoryId");
			this.Property(t => t.Timestamp).HasColumnName("Timestamp");
			this.Property(t => t.VendorId).HasColumnName("VendorId");
			this.Property(t => t.OriginalAmount).HasColumnName("OriginalAmount");
			this.Property(t => t.OriginalVendorId).HasColumnName("OriginalVendorId");
			this.Property(t => t.OriginalCategoryId).HasColumnName("OriginalCategoryId");
			this.Property(t => t.OriginalTimestamp).HasColumnName("OriginalTimestamp");

			// Relationships
			this.HasRequired(t => t.Bill)
				.WithMany(t => t.BillTransactions)
				.HasForeignKey(d => d.BillId);
				
			//this.HasRequired(t => t.Category)
			//    .WithMany(t => t.BillTransactions)
			//    .HasForeignKey(d => d.CategoryId);
				
			//this.HasRequired(t => t.OriginalCategory)
			//    .WithMany(t => t.BillTransactions1)
			//    .HasForeignKey(d => d.OriginalCategoryId);
				
			//this.HasRequired(t => t.Vendor)
			//    .WithMany(t => t.BillTransactions)
			//    .HasForeignKey(d => d.OriginalVendorId);
				
			//this.HasRequired(t => t.Vendor1)
			//    .WithMany(t => t.BillTransactions1)
			//    .HasForeignKey(d => d.VendorId);
		}
	}
}

