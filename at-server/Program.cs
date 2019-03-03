using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace AtServer
{
	class Program
	{
		static void Main(string[] args)
		{
			var cts = new CancellationTokenSource();
			
			var server = new Server(new UrlParser());
			server.Start(cts.Token);

			string line = Console.ReadLine();
			while (line != "quit")
			{


				line = Console.ReadLine();
			}

			cts.Cancel();
		}
	}
}
