using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace scrilla.Data
{
	public class CategoryGroup
	{
		public CategoryGroup()
		{
			this.Categories = new List<Category>();
		}

		public int Id { get; set; }
		public string Name { get; set; }
		public int DisplayOrder { get; set; }

		public virtual ICollection<Category> Categories { get; set; }
	}
}
