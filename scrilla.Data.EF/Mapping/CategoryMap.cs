using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.ModelConfiguration;
using scrilla.Data.Domain;

namespace scrilla.Data.EF.Mapping
{
	public class CategoryMap : EntityTypeConfiguration<Category>
	{
		public CategoryMap()
		{
			// Primary Key
			this.HasKey(t => t.Id);

			// Properties
			this.Property(t => t.Name)
				.IsRequired()
				.HasMaxLength(255);

			// Table & Column Mappings
			this.ToTable("Category");
			this.Property(t => t.Id).HasColumnName("Id");
			this.Property(t => t.CategoryGroupId).HasColumnName("CategoryGroupId");
			this.Property(t => t.Name).HasColumnName("Name");

			// Relationships
			this.HasRequired(t => t.CategoryGroup)
				.WithMany(t => t.Categories)
				.HasForeignKey(d => d.CategoryGroupId);
		}
	}
}
