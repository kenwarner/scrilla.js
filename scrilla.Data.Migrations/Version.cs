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

		_011 = 11 + VersionMonth.October2011,
		_012 = 12 + VersionMonth.October2011,
		_013 = 13 + VersionMonth.October2011,
		_014 = 14 + VersionMonth.October2011,
		_015 = 15 + VersionMonth.October2011,
		_016 = 16 + VersionMonth.October2011,
		_017 = 17 + VersionMonth.October2011,
		_018 = 18 + VersionMonth.October2011,
		_019 = 19 + VersionMonth.October2011,
		_020 = 20 + VersionMonth.October2011,
	}
}
