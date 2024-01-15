using FileUtility.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileUtility.Commands
{
    public class FileCommandManager
    {
        private readonly List<IFileCommand> commands = new List<IFileCommand>();
        private short order = 1;


        public void AddCommand(IFileCommand command)
        {
            command.Order = order++;
            commands.Add(command);
        }

        public bool ProcessCommands()
        {
            foreach (var command in commands)
            {
                command.StartStopwatch();
                command.SetPrevFile();
                command.SetNewFile();
                command.Execute();
                command.EndStopwatch();
                command.SetActivityLog();
                command.SetCurrentFile();
                command.AddTempFile();
                command.CompleteTransaction();
            }
            return true;
        }
    }
}
