using AtCommon;

namespace AtApplication
{
	public class Application1 : Application
	{
		public override Response Process(AtHttpRequest req)
		{
			return new HttpResponse();
		}
	}
}
