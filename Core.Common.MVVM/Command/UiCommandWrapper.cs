using Core.Common.Reflect;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Windows.Input;
using Core.Common.Collections;
using System.Linq;
using System.Diagnostics;
using System;
using System.ComponentModel;

namespace Core.Common.MVVM
{
    public class UiCommandWrapper : ICommand
    {
      private IUiCommand command;
      public UiCommandWrapper(IUiCommand command)
      {
        this.command = command;
        this.command.CanExecuteChanged += HandleExecuteChanged;
      }
    
      public string DisplayName
      {
        get
        {
          return command.GetProperty("DisplayName") as string;
        }
      }
      public string CommandName
      {
        get
        {
          return command.GetProperty("CommandName") as string;
        }
      }
    
      private void HandleExecuteChanged(object sender, System.EventArgs e)
      {
        Trace.WriteLine(CommandName + " can execute changed");
        if (CanExecuteChanged != null) CanExecuteChanged(this, e);
      }
    
    
      public bool CanExecute(object parameter)
      {
        return command.CanExecute(parameter);
      }
    
      public event System.EventHandler CanExecuteChanged;
    
      public void Execute(object parameter)
      {
        command.ExecuteAsync(parameter);
      }
    }
}
