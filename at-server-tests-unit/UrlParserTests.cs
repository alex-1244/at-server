using AtServer;
using Xunit;

namespace at_server_tests_unit
{
	public class UrlParserTests
	{
		[Fact]
		public void UrlParser_should_parse_httpRequest()
		{
			var parser = new UrlParser();

			var result = parser.Parse("GET /qwe1/qwe2?p1=a&p2=b HTTP1.1");

			Assert.Equal(Method.Get, result.Method);
			Assert.Equal("a", result.Parameters["p1"]);
		}
	}
}
