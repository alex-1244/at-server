using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace at_server
{
	class Program
	{
		static void Main(string[] args)
		{
			IPAddress address = IPAddress.Parse("127.0.0.1");
			var listener = new TcpListener(address, 9990);

			listener.Start();

			while (true)
			{
				var clientTask = listener.AcceptTcpClientAsync();

				if (clientTask.Result == null) continue;

				var client = clientTask.Result;

				var requestStream = client.GetStream();
				byte[] data = new byte[1024];

				using (var ms = new MemoryStream())
				{
					int numBytesRead = requestStream.Read(data, 0, data.Length);
					ms.Write(data, 0, numBytesRead);

					while (numBytesRead == data.Length)
					{
						numBytesRead = requestStream.Read(data, 0, data.Length);
						ms.Write(data, 0, numBytesRead);
					}

					requestStream.Dispose();
					requestStream.Close();

					var str = Encoding.ASCII.GetString(ms.ToArray(), 0, (int)ms.Length);
					Console.WriteLine(str);
				}
			}

			Console.ReadKey();
		}
	}
}
