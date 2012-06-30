using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.ModelConfiguration;
using scrilla.Data.Domain;

namespace scrilla.Data.EF.Mapping
{
	public class AccountNameMapMap : EntityTypeConfiguration<AccountNameMap>
	{
		public AccountNameMapMap()
		{
			// Primary Key
			this.HasKey(t => t.Id);

			// Properties
			this.Property(t => t.Name)
				.IsRequired()
				.HasMaxLength(255);

			// Table & Column Mappings
			this.ToTable("AccountNameMap");
			this.Property(t => t.Id).HasColumnName("Id");
			this.Property(t => t.AccountId).HasColumnName("AccountId");
			this.Property(t => t.Name).HasColumnName("Name");

			// Relationships
			this.HasRequired(t => t.Account)
				.WithMany(t => t.AccountNameMaps)
				.HasForeignKey(d => d.AccountId);

		}
	}
}
