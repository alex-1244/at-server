using AtServer;

namespace AtCli.Commands
{
	internal class StopServerCommand : ICommand
	{
		public void Execuete(Server server)
		{
		}

		public bool StopAfterExecution { get; } = true;
	}
}