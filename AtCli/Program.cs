using System.Threading;
using AtServer;

namespace AtCli
{
	class Program
	{
		static void Main(string[] args)
		{
			var cts = new CancellationTokenSource();

			var server = new Server(new UrlParser());
			var workerThread = new Thread(() => server.Start(cts.Token)) { Priority = ThreadPriority.AboveNormal };

			var cli = new Cli(new InputParser(), server);
			cli.Run();

			cts.Cancel();
			workerThread.Join(1000);
		}
	}
}
