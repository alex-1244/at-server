using System;
using System.Linq;
using System.Web;

namespace AtServer
{
	public class UrlParser : IRequestParser
	{
		public AtHttpRequest Parse(string rawRequest)
		{
			var requestParts = rawRequest.Split(" ");

			if (requestParts.Length < 3)
			{
				throw new ArgumentException("request is not well formated");
			}

			var parsedRequest = new AtHttpRequest
			{
				Uri = new Uri(new Uri("http://example.com"), requestParts[1]),
				Method = Enum.Parse<Method>(requestParts[0], true),
			};

			var query = HttpUtility.ParseQueryString(parsedRequest.Uri.Query);

			parsedRequest.Parameters = query.AllKeys.ToDictionary(x => x, x => query[x]);

			return parsedRequest;
		}
	}
}
