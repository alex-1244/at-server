using AtServer;

namespace AtCli.Commands
{
	internal class AddApplicationCommand : ICommand
	{
		internal const string CommandPart = "add-application ";
		private readonly string _applicationAssemblyLocation;

		public AddApplicationCommand(string applicationAssemblyLocation)
		{
			this._applicationAssemblyLocation = applicationAssemblyLocation.Substring(0, CommandPart.Length);
		}

		public void Execuete(Server server)
		{
			server.RegisterApplication(this._applicationAssemblyLocation);
		}

		public bool StopAfterExecution { get; } = false;
	}
}