using System;
using AtServer;

namespace AtCli.Commands
{
	internal class UnknownCommand : ICommand
	{
		public void Execuete(Server server)
		{
			Console.WriteLine("Unknown command");
		}

		public bool StopAfterExecution { get; } = false;
	}
}