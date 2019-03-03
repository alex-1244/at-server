using System;
using System.Collections.Generic;

namespace AtCommon
{
	public class AtHttpRequest
	{
		public Method Method { get; set; }

		public Uri Uri { get; set; }

		public IDictionary<string, string> Parameters { get; set; }
	}
}
