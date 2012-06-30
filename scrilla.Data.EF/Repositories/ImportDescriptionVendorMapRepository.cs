using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using scrilla.Data.Domain;
using scrilla.Data.Repositories;

namespace scrilla.Data.EF.Repositories
{
	public class ImportDescriptionVendorMapRepository : RepositoryBase<ImportDescriptionVendorMap>, IImportDescriptionVendorMapRepository
	{
		public ImportDescriptionVendorMapRepository(IDatabaseFactory databaseFactory) : base(databaseFactory) { }
	}
}
