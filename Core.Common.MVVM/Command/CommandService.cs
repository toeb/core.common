using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;

namespace Core.Common.MVVM
{

  [Export(typeof(ICommandService))]
  public class CommandService : ICommandService
  {
    [ImportingConstructor]
    public CommandService([ImportMany] IEnumerable<IUiCommand> globalCommands)
    {
      foreach (var command in globalCommands)
      {
        RegisterCommand(null, command);
      }
    }
    class CommandEntry
    {
      public object context;
      public object parentContext;
      public string key;
      public IUiCommand command;
    }
    private static ICollection<CommandEntry> commands = new List<CommandEntry>();
  
    public IUiCommand GetCommandById(object context, string id)
    {
      var cmd = Command.Reflect(context, id);

      return cmd;

      //var entry = GetCommandEntries(context).FirstOrDefault(c => c.key == id);
      //if (entry == null) return null;
      //return entry.command;
    }
    private IEnumerable<CommandEntry> GetCommandEntries(object context)
    {
     return  commands.Where(c => c.context == context)
        .Concat(GetCommandEntries(GetParentContext(context)));
    }
  
    public IEnumerable<IUiCommand> GetCommands(object context)
    {
      if (context == null) return Enumerable.Empty<IUiCommand>();
      var commands = Command.ReflectCommands(context);
      return commands;


      //return GetCommandEntries(context).Select(c => c.command).ToArray();
    }
  
    public void RegisterCommand(object context, IUiCommand command)
    {
      var entry = new CommandEntry();
      entry.context = context;
      entry.command = command;
      entry.parentContext = GetParentContext(context);
      entry.key = GetCommandKey(command);
  
    }
  
    private string GetCommandKey(IUiCommand command)
    {
      return command.CommandId;
    }
  
    private object GetParentContext(object context)
    {
      return null;
    }
  }
}
