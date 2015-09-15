using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common.MVVM
{
  public interface IUiCommand
  {
    string CommandId { get; }

    Task ExecuteAsync(object parameter);
    bool CanExecute(object parameter);
    event EventHandler CanExecuteChanged;



  }

  public static class IUiCommandExtensions
  {
    public static void Execute(this IUiCommand command, object parameter)
    {
      command.ExecuteAsync(parameter).Wait();
    }
  }
}
