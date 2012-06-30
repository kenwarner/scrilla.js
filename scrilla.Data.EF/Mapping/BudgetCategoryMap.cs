using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.ModelConfiguration;
using scrilla.Data.Domain;

namespace scrilla.Data.EF.Mapping
{
	public class BudgetCategoryMap : EntityTypeConfiguration<BudgetCategory>
	{
		public BudgetCategoryMap()
		{
			// Primary Key
			this.HasKey(t => t.Id);

			// Properties
			// Table & Column Mappings
			this.ToTable("BudgetCategory");
			this.Property(t => t.Id).HasColumnName("Id");
			this.Property(t => t.CategoryId).HasColumnName("CategoryId");
			this.Property(t => t.Month).HasColumnName("Month");
			this.Property(t => t.Amount).HasColumnName("Amount");

			// Relationships
			this.HasRequired(t => t.Category)
				.WithMany(t => t.BudgetCategories)
				.HasForeignKey(d => d.CategoryId);

		}
	}
}
