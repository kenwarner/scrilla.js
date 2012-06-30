using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.ModelConfiguration;
using scrilla.Data.Domain;

namespace scrilla.Data.EF.Mapping
{
	public class SubtransactionMap : EntityTypeConfiguration<Subtransaction>
	{
		public SubtransactionMap()
		{
			// Primary Key
			this.HasKey(t => t.Id);

			// Properties
			this.Property(t => t.Memo)
				.IsRequired()
				.HasMaxLength(255);

			this.Property(t => t.Notes)
				.IsRequired()
				.HasMaxLength(255);

			// Table & Column Mappings
			this.ToTable("Subtransaction");
			this.Property(t => t.Id).HasColumnName("Id");
			this.Property(t => t.TransactionId).HasColumnName("TransactionId");
			this.Property(t => t.CategoryId).HasColumnName("CategoryId");
			this.Property(t => t.Amount).HasColumnName("Amount");
			this.Property(t => t.Memo).HasColumnName("Memo");
			this.Property(t => t.Notes).HasColumnName("Notes");
			this.Property(t => t.IsTransfer).HasColumnName("IsTransfer");

			// Relationships
			this.HasOptional(t => t.Category)
				.WithMany(t => t.Subtransactions)
				.HasForeignKey(d => d.CategoryId);

			this.HasRequired(t => t.Transaction)
				.WithMany(t => t.Subtransactions)
				.HasForeignKey(d => d.TransactionId);
		}
	}

}
