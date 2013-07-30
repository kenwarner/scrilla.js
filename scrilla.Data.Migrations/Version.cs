using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace scrilla.Data.Migrations
{
	internal static class VersionMonth
	{
		public const int September2011 = 201109000;
		public const int October2011 = 201110000;
		public const int November2011 = 201111000;
		public const int December2011 = 201112000;

		public const int January2012 = 201201000;
		public const int February2012 = 201202000;
		public const int March2012 = 201203000;
		public const int April2012 = 201204000;
		public const int May2012 = 201205000;
		public const int June2012 = 201206000;
		public const int July2012 = 201207000;
		public const int August2012 = 201208000;
		public const int September2012 = 201209000;
		public const int October2012 = 201210000;
		public const int November2012 = 201211000;
		public const int December2012 = 201212000;

		public const int January2013 = 201301000;
		public const int February2013 = 201302000;
		public const int March2013 = 201303000;
		public const int April2013 = 201304000;
		public const int May2013 = 201305000;
		public const int June2013 = 201306000;
		public const int July2013 = 201307000;
		public const int August2013 = 201308000;
		public const int September2013 = 201309000;
		public const int October2013 = 201310000;
		public const int November2013 = 201311000;
		public const int December2013 = 201312000;

	}

	internal enum Version : long
	{
		_001 = 1 + VersionMonth.September2011,
		_002 = 2 + VersionMonth.September2011,
		_003 = 3 + VersionMonth.September2011,
		_004 = 4 + VersionMonth.September2011,
		_005 = 5 + VersionMonth.September2011,
		_006 = 6 + VersionMonth.September2011,
		_007 = 7 + VersionMonth.September2011,
		_008 = 8 + VersionMonth.September2011,
		_009 = 9 + VersionMonth.September2011,
		_010 = 10 + VersionMonth.September2011,

		_011 = 11 + VersionMonth.July2013,
		_012 = 12 + VersionMonth.July2013,
	}
}
