using Core;
using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using Core.Modules.Applications.Runner;

namespace Core.Modules.ConsoleInteraction
{
  [Module]
  public class ConsoleInteractionModule : IModule
  {
    #region Imports
    [Import]
    IApplicationRunner Runner { get; set; }
    [Import]
    IReflectionService Reflection { get; set; }
    [ImportMany(AllowRecomposition = true)]
    IEnumerable<IConsoleHandler> Handlers { get; set; }

    #endregion


    #region Exports
    [Export("ConsoleOutput")]
    TextWriter ConsoleOutput { get { return System.Console.Out; } }

    [Export("ConsoleInput")]
    TextReader ConsoleInput { get { return System.Console.In; } }

    #endregion

    private ConsoleInteractionModule() { }



    public void Activate()
    {
      this.ConsoleInteractionTask = Task.Factory.StartNew(Run);
    }
    public void Deactivate()
    { 
    }
    void Run()
    {
      // infinite loop
      while (true)
      {
        if (Runner.IsStopping) break;
        ConsoleOutput.Write("> ");
        var input = ConsoleInput.ReadLine();
        bool handled = false;
        foreach (var handler in Handlers.OrderBy(h => h.Priority))
        {
          try
          {
            handled = handler.HandleCommand(input);
          }
          catch (Exception e)
          {
            ConsoleOutput.WriteLine(e);
          }
          if (handled) break;
        }
        if (!handled) ConsoleOutput.WriteLine("unknown command '" + input + "'");
        Console.WriteLine();
      }

    }


    public Task ConsoleInteractionTask { get; set; }


  }
}
