using System;
using AtCli.Commands;

namespace AtCli
{
	public class InputParser
	{
		public ICommand Parse(string commandString)
		{
			if (commandString.IndexOf(AddApplicationCommand.CommandPart, StringComparison.Ordinal) == 0)
			{
				return new AddApplicationCommand(commandString);
			}

			if (commandString == "quit")
			{
				return new StopServerCommand();
			}

			return new UnknownCommand();
		}
	}
}