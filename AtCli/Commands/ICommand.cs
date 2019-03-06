using AtServer;

namespace AtCli.Commands
{
	public interface ICommand
	{
		void Execuete(Server server);

		bool StopAfterExecution { get; }
	}
}
