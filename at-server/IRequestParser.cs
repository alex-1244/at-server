namespace AtServer
{
	public interface IRequestParser
	{
		AtHttpRequest Parse(string rawRequest);
	}
}