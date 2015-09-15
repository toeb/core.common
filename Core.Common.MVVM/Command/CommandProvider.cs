
using Core.Common.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Input;
namespace Core.Common.MVVM
{


  [Export]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class CommandProvider : ObservableCollection<ICommand>
  {
    private IDictionary<string, ICommand> commands;
    public CommandProvider()
    {
      commands = new Dictionary<string, ICommand>();
    } 



    private void Update()
    {

      this.commands.Clear();
      var commands =  CommandService.GetCommands(Context);
      this.SetCollection(commands.Select(c => new UiCommandWrapper(c)));
    }

    [Import]
    public ICommandService CommandService { get; set; }


    public ICommand this[string commandName]
    {
      get
      {
        ICommand cmd;
        var success = commands.TryGetValue(commandName, out cmd);
        if (success && cmd != null) return cmd;

        var command = CommandService.GetCommandById(Context, commandName);
        if (command == null) return null;
        cmd = new UiCommandWrapper(command);
        commands[commandName] = cmd;
        return cmd;
      }
    }

    object context;
    public object Context
    {
      get { return context; }
      set
      {
        context = value;
        Update();
      }
    }

  }
}

