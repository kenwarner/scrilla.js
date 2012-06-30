using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.ModelConfiguration;
using scrilla.Data.Domain;

namespace scrilla.Data.EF.Mapping
{
	public class CategoryGroupMap : EntityTypeConfiguration<CategoryGroup>
	{
		public CategoryGroupMap()
		{
			// Primary Key
			this.HasKey(t => t.Id);

			// Properties
			this.Property(t => t.Name)
				.IsRequired()
				.HasMaxLength(255);

			// Table & Column Mappings
			this.ToTable("CategoryGroup");
			this.Property(t => t.Id).HasColumnName("Id");
			this.Property(t => t.Name).HasColumnName("Name");
			this.Property(t => t.DisplayOrder).HasColumnName("DisplayOrder");
		}
	}
}
