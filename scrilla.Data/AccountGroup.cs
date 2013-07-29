using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace scrilla.Data
{
	public class AccountGroup
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public bool IsActive { get; set; }
		public int DisplayOrder { get; set; }
	}
}
