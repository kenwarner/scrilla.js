using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace scrilla.js.Web.Models
{
	public static class ExtensionMethods
	{
		public static HtmlString ToHtmlUrlDate(this DateTime datetime)
		{
			return new HtmlString(datetime.ToString("yyyy-MM-dd"));
		}

		public static string ToUrlDate(this DateTime datetime)
		{
			return datetime.ToString("yyyy-MM-dd");
		}

		public static HtmlString ToHtmlCurrency(this decimal value)
		{
			return new HtmlString(value.ToString("$#,##0.00;-$#,##0.00"));
		}

		public static string ToCurrency(this decimal value)
		{
			return value.ToString("$#,##0.00;-$#,##0.00");
		}
	}
}