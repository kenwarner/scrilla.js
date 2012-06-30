using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Common;
using System.Data.Entity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using scrilla.Data.Domain;

namespace scrilla.Data.EF.Mapping
{
	public class AccountMap : EntityTypeConfiguration<Account>
	{
		public AccountMap()
		{
			// Primary Key
			this.HasKey(t => t.Id);

			// Properties
			this.Property(t => t.Name)
				.IsRequired()
				.HasMaxLength(255);

			// Table & Column Mappings
			this.ToTable("Account");
			this.Property(t => t.Id).HasColumnName("Id");
			this.Property(t => t.Name).HasColumnName("Name");
			this.Property(t => t.InitialBalance).HasColumnName("InitialBalance");
			this.Property(t => t.Balance).HasColumnName("Balance");
			this.Property(t => t.BalanceTimestamp).HasColumnName("BalanceTimestamp");

			// Relationships
			this.HasRequired(t => t.AccountGroup)
				.WithMany(t => t.Accounts)
				.HasForeignKey(d => d.AccountGroupId);
		}
	}
}

