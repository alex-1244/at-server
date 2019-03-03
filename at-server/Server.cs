using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace AtServer
{
	public class Server
	{
		private readonly IRequestParser requestParser;

		public Server(IRequestParser requestParser)
		{
			this.requestParser = requestParser;
		}

		public void Start(CancellationToken token)
		{
			var address = IPAddress.Parse("127.0.0.1");
			var listener = new TcpListener(address, 9990);

			listener.Start();

			while (!token.IsCancellationRequested)
			{
				var clientTask = listener.AcceptTcpClientAsync();

				if (clientTask.Result == null)
				{
					continue;
				}

				var client = clientTask.Result;

				byte[] data = new byte[1024];

				using (var requestStream = client.GetStream())
				using (var ms = new MemoryStream())
				{
					int numBytesRead = requestStream.Read(data, 0, data.Length);
					ms.Write(data, 0, numBytesRead);

					while (numBytesRead == data.Length)
					{
						numBytesRead = requestStream.Read(data, 0, data.Length);
						ms.Write(data, 0, numBytesRead);
					}

					data = ms.ToArray();

					var requestString = Encoding.ASCII.GetString(data, 0, data.Length);
					Console.WriteLine(requestString);

					var result = this.Process(requestString);

					var str = @"HTTP/1.1 200 OK
Content-Type: application/json; charset=utf-8

					{
						""userId"": 1,
						""id"": 1,
						""title"": ""delectus aut autem"",
						""completed"": false
					}
					";

					requestStream.Write(Encoding.ASCII.GetBytes(str));
				}
			}
		}

		private byte[] Process(string requestStr)
		{
			var requestString = requestStr.Split("\r\n")[0];

			var request = this.requestParser.Parse(requestString);

			return null;
		}
	}
}
