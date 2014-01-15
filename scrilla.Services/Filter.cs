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
	///   Filter<int?> accountIdFilter = new Filter<int?>("none"); // query the accountId column for values of null
	///   Filter<int?> accountIdFilter = new Filter<int?>("1"); // query the accountId column for values of 1
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

		public static Filter<T> Parse(string obj)
		{
			// if the string is empty, then we don't want to filter on it at all
			if (String.IsNullOrEmpty(obj))
				return null;

			// if the string is "none" then we want to filter on values that are specifically equal to null
			if (obj.Equals("none"))
				return new Filter<T>();

			// otherwise try to parse the string
			object parsed = null;
			switch (Type.GetTypeCode(typeof(T)))
			{
				case TypeCode.Int32:
					parsed = Int32.Parse(obj);
					break;
				case TypeCode.Decimal:
					parsed = Decimal.Parse(obj);
					break;
			}

			return new Filter<T>((T)Convert.ChangeType(parsed, typeof(T)));
		}
	}
}
