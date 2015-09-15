using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;

namespace Core.Common.MVVM
{
  public interface ICommandService
  {
  
    IEnumerable<IUiCommand> GetCommands(object Context);  
    IUiCommand GetCommandById(object Context, string commandName);
  }
}
