namespace AtApplication
{
	public abstract class Response
	{
		public abstract override string ToString();
	}

	public class HttpResponse : Response
	{
		public override string ToString()
		{
			return @"{
						""userId"": 1,
						""id"": 1,
						""title"": ""delectus aut autem"",
						""completed"": false
					}";
		}
	}
}