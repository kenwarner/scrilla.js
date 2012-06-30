using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Common;
using System.Data.Entity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using scrilla.Data.Domain;

namespace scrilla.Data.EF.Mapping
{
	public class VendorMap : EntityTypeConfiguration<Vendor>
	{
		public VendorMap()
		{
			// Primary Key
			this.HasKey(t => t.Id);

			// Properties
			this.Property(t => t.Name)
				.IsRequired()
				.HasMaxLength(255);

			// Table & Column Mappings
			this.ToTable("Vendor");
			this.Property(t => t.Id).HasColumnName("Id");
			this.Property(t => t.Name).HasColumnName("Name");
			this.Property(t => t.DefaultCategoryId).HasColumnName("DefaultCategoryId");

			// Relationships
			this.HasOptional(t => t.DefaultCategory)
				.WithMany(t => t.VendorDefaults)
				.HasForeignKey(d => d.DefaultCategoryId);
		}
	}
}

