using System;
using AtServer;

namespace AtCli
{
	public class Cli
	{
		private readonly Server _server;
		private readonly InputParser _parser;

		public Cli(InputParser parser, Server server)
		{
			this._parser = parser;
			this._server = server;
		}

		public void Run()
		{
			var command = this._parser.Parse(Console.ReadLine());

			while (!command.StopAfterExecution)
			{
				command = this._parser.Parse(Console.ReadLine());
				command.Execuete(this._server);
			}
		}
	}
}
