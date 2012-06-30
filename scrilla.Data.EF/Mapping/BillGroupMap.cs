using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.ModelConfiguration;
using scrilla.Data.Domain;

namespace scrilla.Data.EF.Mapping
{
	public class BillGroupMap : EntityTypeConfiguration<BillGroup>
	{
		public BillGroupMap()
		{
			// Primary Key
			this.HasKey(t => t.Id);

			// Properties
			this.Property(t => t.Name)
				.IsRequired()
				.HasMaxLength(255);

			// Table & Column Mappings
			this.ToTable("BillGroup");
			this.Property(t => t.Id).HasColumnName("Id");
			this.Property(t => t.Name).HasColumnName("Name");
			this.Property(t => t.IsActive).HasColumnName("IsActive");
			this.Property(t => t.DisplayOrder).HasColumnName("DisplayOrder");
		}
	}
}
