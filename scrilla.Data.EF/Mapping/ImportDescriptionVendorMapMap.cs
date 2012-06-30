using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.ModelConfiguration;
using scrilla.Data.Domain;

namespace scrilla.Data.EF.Mapping
{
	public class ImportDescriptionVendorMapMap : EntityTypeConfiguration<ImportDescriptionVendorMap>
	{
		public ImportDescriptionVendorMapMap()
		{
			// Primary Key
			this.HasKey(t => t.Id);

			this.ToTable("ImportDescriptionVendorMap");

			// Relationships
			this.HasRequired(t => t.Vendor)
				.WithMany(t => t.ImportDescriptionVendorMaps)
				.HasForeignKey(d => d.VendorId);

		}
	}
}
