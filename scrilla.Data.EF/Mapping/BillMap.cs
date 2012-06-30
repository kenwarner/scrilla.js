using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.ModelConfiguration;
using scrilla.Data.Domain;

namespace scrilla.Data.EF.Mapping
{
	public class BillMap : EntityTypeConfiguration<Bill>
	{
		public BillMap()
		{
			// Primary Key
			this.HasKey(t => t.Id);

			// Properties
			this.Property(t => t.Name)
				.IsRequired()
				.HasMaxLength(255);

			// Table & Column Mappings
			this.ToTable("Bill");
			this.Property(t => t.Id).HasColumnName("Id");
			this.Property(t => t.BillGroupId).HasColumnName("BillGroupId");
			this.Property(t => t.VendorId).HasColumnName("VendorId");
			this.Property(t => t.CategoryId).HasColumnName("CategoryId");
			this.Property(t => t.Name).HasColumnName("Name");
			this.Property(t => t.Amount).HasColumnName("Amount");
			this.Property(t => t.StartDate).HasColumnName("StartDate");
			this.Property(t => t.StartDate2).HasColumnName("StartDate2");
			this.Property(t => t.EndDate).HasColumnName("EndDate");
			this.Property(t => t.EndDate2).HasColumnName("EndDate2");
			this.Property(t => t.RecurrenceFrequency).HasColumnName("RecurrenceFrequency");

			// Relationships
			this.HasRequired(t => t.BillGroup)
				.WithMany(t => t.Bills)
				.HasForeignKey(d => d.BillGroupId);

			//this.HasOptional(t => t.Category)
			//    .WithMany(t => t.Bills)
			//    .HasForeignKey(d => d.CategoryId);

			this.HasOptional(t => t.Vendor)
				.WithMany(t => t.Bills)
				.HasForeignKey(d => d.VendorId);

		}
	}
}
