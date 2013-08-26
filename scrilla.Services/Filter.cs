using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scrilla.Services
{
	/// <summary>
	/// This class allows us to pass nullable types to Service methods and differentiate between not querying for a given parameter and querying that parameter for null values
	///   example usage:
	///   Filter<int?> accountIdFilter = null; // do not query the accountId column
	///   Filter<int?> accountIdFilter = new Filter<int?>(null); // query the accountId column for values of null
	///   Filter<int?> accountIdFilter = new Filter<int?>(1); // query the accountId column for values of 1
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class Filter<T>
	{
		public T Object { get; set; }

		public Filter()
		{

		}

		public Filter(T obj)
		{
			Object = obj;
		}
	}
}
