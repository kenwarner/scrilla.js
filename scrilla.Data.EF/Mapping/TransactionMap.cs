using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Common;
using System.Data.Entity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using scrilla.Data.Domain;

namespace scrilla.Data.EF.Mapping
{
	public class TransactionMap : EntityTypeConfiguration<Transaction>
	{
		public TransactionMap()
		{
			// Primary Key
			this.HasKey(t => t.Id);

			// Properties
			// Table & Column Mappings
			this.ToTable("Transaction");
			this.Property(t => t.Id).HasColumnName("Id");
			this.Property(t => t.AccountId).HasColumnName("AccountId");
			this.Property(t => t.Timestamp).HasColumnName("Timestamp");
			this.Property(t => t.OriginalTimestamp).HasColumnName("OriginalTimestamp");
			this.Property(t => t.Amount).HasColumnName("Amount");
			this.Property(t => t.IsReconciled).HasColumnName("IsReconciled");
			this.Property(t => t.VendorId).HasColumnName("VendorId");
			this.Property(t => t.BillTransactionId).HasColumnName("BillTransactionId");
			this.Ignore(t => t.Balance);

			// Relationships
			this.HasRequired(t => t.Account)
				.WithMany(t => t.Transactions)
				.HasForeignKey(d => d.AccountId);

			this.HasOptional(t => t.BillTransaction)
				.WithMany(t => t.Transactions)
				.HasForeignKey(d => d.BillTransactionId);

			this.HasOptional(t => t.Vendor)
				.WithMany(t => t.Transactions)
				.HasForeignKey(d => d.VendorId);
		}
	}
}

