using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using AtApplication;

namespace AtServer
{
	public class Server
	{
		private const string CommonPart = @"HTTP/1.1 200 OK
Content-Type: application/json; charset=utf-8

";

		private readonly IRequestParser _requestParser;
		private readonly IList<Application> _applications;

		public Server(IRequestParser requestParser)
		{
			this._requestParser = requestParser;
			this._applications = new List<Application>();
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
				{
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

						requestStream.Write(Encoding.ASCII.GetBytes(CommonPart));
						requestStream.Write(result);
					}
				}
			}
		}

		public void RegisterApplication(string applicationPath)
		{
			var dllFile = new FileInfo(applicationPath);
			var dll = Assembly.LoadFile(dllFile.FullName);

			var application = dll
				.GetTypes().FirstOrDefault(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(Application)));

			if (application != null)
			{
				this._applications.Add((Application)Activator.CreateInstance(application));
			}
		}

		private byte[] Process(string requestStr)
		{
			var requestString = requestStr.Split("\r\n")[0];

			var request = this._requestParser.Parse(requestString);

			foreach (var application in this._applications)
			{
				var result = application.Process(request).ToString();

				if (!string.IsNullOrEmpty(result))
				{
					return Encoding.ASCII.GetBytes(result);
				}
			}

			return new byte[0];
		}
	}
}
