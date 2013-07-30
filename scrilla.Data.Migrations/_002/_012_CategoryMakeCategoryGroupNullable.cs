using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scrilla.Data.Migrations._002
{
	[Migration((long)Version._012)]
	public class _012_CategoryMakeCategoryGroupNullable : Migration
	{
		public override void Up()
		{
			Alter.Column("CategoryGroupId").OnTable("Category").AsInt32().Nullable();
		}

		public override void Down()
		{
			Alter.Column("CategoryGroupId").OnTable("Category").AsInt32().NotNullable();
		}
	}
}
