
using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using Core.Extensions;
using System.IO;
using System.Reflection;
using Core.Modules.Applications;
using Core.Modules.Applications.Runner;
using Core.Commands;


namespace Core.Modules.Applications
{
  [Module]
  public class ApplicationConsoleCommandsModule
  {
    private ApplicationConsoleCommandsModule() { }

    [Import("ApplicationContainer")]
    CompositionContainer Container { get; set; }
    [Import]
    IApplication Application { get; set; }


    [Import("ConsoleOutput")]
    TextWriter Console { get; set; }

    [Command(Name="lm",ShortName="lm", HelpText="lists all modules")]
    void ListModules(string search)
    {
      var modules = Application.Modules;
      var parts = search.Split(' ');
      if (parts.Count() != 0)
      {
        modules = parts.SelectMany(part => Application.FindModulesByPartialName(part)).Distinct();
      }

      foreach (var module in modules)
      {
        Console.WriteLine("{0} {1}-{2}", module.IsActive ? "x" : "o", module.ModuleInfo.ModuleName, module.ModuleInfo.Version);
      }

    }
    [Import]

    IApplicationRunner Runner { get; set; }

    [Command(HelpText = "Stops the application")]
    public void stop()
    {
      Runner.Stop();
    }



    [Command(Name="all",ShortName="all",HelpText="discovers and activates all modules in AppDomain")]
    void activateall()
    {
      Application.DiscoverModules();
      Application.Modules
        .Where(m => !m.IsActive)
        .Do(m => Application.ActivateModule(m));
    }

    [Command(Name="exports", HelpText="Shows all exports available in applications composition container")]
    void exports(string search)
    {
      var exports = Container.GetExports(new ImportDefinition(ed => ed.ContractName.ToLower().Contains(search), null, ImportCardinality.ZeroOrMore, true, false));
      Console.WriteLine("Found " + exports.Count() + " exports");
      foreach (var export in exports)
      {
        Console.WriteLine("\t" + export.Definition.ContractName + ": " + export.Value);
      }
    }

    [Command]
    void activate(string module)
    {
      try
      {
        var m = Application.FindModulesByPartialName(module).Single();
        Application.ActivateModule(m);
      }
      catch (Exception e)
      {
        Console.WriteLine("Could not activate module\n" + e);
      }
    }

    [Command]
    void deactivate(string module)
    {

      try
      {
        var m = Application.FindModulesByPartialName(module).Single();
        Application.DeactivateModule(m);
      }
      catch (Exception e)
      {
        Console.WriteLine("Could not deactivate module\n" + e);
      }
    }


    [Command]
    void discover()
    {
      Application.DiscoverModules();
    }
  }
}
