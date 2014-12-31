using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace scrilla.Services.Tests
{
	public class FilterFixture
	{
		[Fact]
		public void Parse_IntegerIsNone()
		{
			Filter<int> sut = Filter<int>.Parse("none");
			sut.Object.Should().Be(0);
		}

		[Fact]
		public void Parse_IntegerIsNull()
		{
			Filter<int> sut = Filter<int>.Parse(null);
			sut.Should().Be(null);
		}

		[Fact]
		public void Parse_IntegerIsOne()
		{
			Filter<int> sut = Filter<int>.Parse("1");
			sut.Object.Should().Be(1);
		}

		[Fact]
		public void Parse_NullableIntegerIsNone()
		{
			Filter<int?> sut = Filter<int?>.Parse("none");
			sut.Object.Should().Be(null);
		}

		[Fact]
		public void Parse_NullableIntegerIsNull()
		{
			Filter<int?> sut = Filter<int?>.Parse(null);
			sut.Should().Be(null);
		}

		[Fact]
		public void Parse_NullableIntegerIsOne()
		{
			Filter<int?> sut = Filter<int?>.Parse("1");
			sut.Object.Should().Be(1);
		}
	}
}
