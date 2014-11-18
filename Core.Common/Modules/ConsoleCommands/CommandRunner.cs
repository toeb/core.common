
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.IO;
using System.Reflection;
using Core.Modules.ConsoleInteraction;
using Core.Commands;
using System.Collections;

namespace Core.Modules.ConsoleCommandRunner
{
  [Module]
  [Export(typeof(IConsoleHandler))]
  public class CommandRunnerModule : IConsoleHandler
  {
    private CommandRunnerModule() { }

    #region imports
    [ImportMany("Command", AllowRecomposition = true)]
    IEnumerable<Delegate> Commands { get; set; }
    [Import("ConsoleOutput")]
    TextWriter Console { get; set; }
    #endregion

    #region exports
    [Command]
    void help([Arg] string CommandName)
    {
      if (CommandName == null)
      {
        commands();
        return;
      }
      var commandDelegate = GetCommandByName(CommandName);

      if (commandDelegate == null)
      {
        Console.WriteLine("help: unknown command '" + CommandName + "'");
        return;
      }
      var command = Command(commandDelegate);
      Console.WriteLine("help for '{0}' ({1}) : {2}", command.Name, command.ShortName, command.HelpText);
      Console.WriteLine(" arguments");
      foreach (var info in commandDelegate.Method.GetParameters())
      {
        var arg = Arg(info);
        Console.WriteLine("   --{0} (-{1}){2}{3}: {4}", arg.Name, arg.ShortName, arg.Required ? " required " : "", arg.DefaultValue == null ? "" : "default: " + arg.DefaultValue, arg.HelpText);
      }
    }


    [Command]
    void commands()
    {
      Console.WriteLine("Available commands:");
      foreach (var command in Commands.Distinct())
      {
        var attribute = command.Method.GetCustomAttribute<CommandAttribute>();
        Console.WriteLine(command.Method.Name);
      }
    }

    #endregion

    public string GetValue(string name, string command)
    {
      var i = command.IndexOf("-" + name);
      if (i == -1) return null;
      i = i + 1 + name.Length;
      var j = command.IndexOf("-", i);
      if (j == -1) j = command.Length;

      var val = command.Substring(i, j - i);
      val = val.Trim();
      return val;
    }

    ArgAttribute Arg(ParameterInfo parameter)
    {
      var arg = parameter.GetCustomAttribute<ArgAttribute>() ?? new ArgAttribute();

      if (string.IsNullOrEmpty(arg.Name))
      {
        arg.Name = parameter.Name;
      }
      if (string.IsNullOrEmpty(arg.ShortName))
      {
        arg.ShortName = new string(arg.Name.Where(c => char.IsUpper(c)).ToArray());
        if (string.IsNullOrEmpty(arg.ShortName))
        {
          arg.ShortName = parameter.Name.Substring(0, 1);
        }
      }

      return arg;
    }
    void Invoke(Delegate action, string command)
    {
      var parameters = action.Method.GetParameters();
      var args = new List<object>();

      foreach (var parameter in parameters)
      {
        var arg = Arg(parameter);

        var stringValue = GetValue("-" + arg.Name, command) ?? GetValue(arg.ShortName, command);
        if (stringValue == null && parameters.Count() == 1)
        {
          stringValue = command.Trim();
        }
        if (stringValue == null && arg.Required)
        {
          Console.WriteLine("Cannot run command " + arg.Name + " is required");
        }
        if (parameter.ParameterType.IsAssignableFrom(typeof(string)))
        {
          args.Add(stringValue ?? arg.DefaultValue);
        }
      }

      var result =action.DynamicInvoke(args.ToArray());
      if (result != null)
      {
        if (result is string)
        {
          Console.WriteLine(result);
          return;
        }
        var enumerable = result as IEnumerable;
        if (enumerable != null)
        {
          foreach (var obj in enumerable)
          {
            Console.WriteLine(obj);
          }
        }
        else
        {
          Console.WriteLine(result);
        }
      }
    }

    Delegate GetCommandByName(string name)
    {
      var commandDelegate = Commands.FirstOrDefault(c =>
      {
        var attribute = c.Method.GetCustomAttribute<CommandAttribute>();
        if (!string.IsNullOrEmpty(attribute.Name)) return attribute.Name.ToLower() == name.ToLower();
        return c.Method.Name.ToLower() == name;
      });
      return commandDelegate;
    }


    CommandAttribute Command(Delegate del)
    {
      var attribute = del.Method.GetCustomAttribute<CommandAttribute>() ?? new CommandAttribute();
      if (string.IsNullOrEmpty(attribute.Name)) attribute.Name = del.Method.Name;
      if (string.IsNullOrEmpty(attribute.ShortName)) attribute.ShortName = attribute.Name;
      if (string.IsNullOrEmpty(attribute.HelpText)) attribute.HelpText = "";
      return attribute;
    }



    public bool HandleCommand(string cmd)
    {
      if (Commands == null) return false;
      var delegates = Commands.Distinct();
      var namePart = cmd.Split(' ').FirstOrDefault();
      if (namePart == null) return false;
      foreach (var command in delegates)
      {
        var attribute = Command(command);
        var name = attribute.Name.ToLower().Trim();
        if (namePart.Equals(name, StringComparison.OrdinalIgnoreCase))
        {
          var args = cmd.Substring(name.Length);
          Invoke(command, args);
          return true;
        }
      }
      return false;

    }
    public int Priority
    {
      get { return 1; }
    }



    
  }
}
