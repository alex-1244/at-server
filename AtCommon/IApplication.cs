using System;
using System.Collections.Generic;
using System.Text;

namespace AtCommon
{
	public interface IApplication
	{
		string Process(AtHttpRequest request);
	}
}
