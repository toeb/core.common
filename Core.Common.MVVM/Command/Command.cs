using System;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using Core.Common.Reflect;
using System.ComponentModel;

namespace Core.Common.MVVM
{
  public static class Command
  {
    public static string GetCommandName(this MethodInfo method)
    {
  
      return GetCommandAttributeOrDefault(method).CommandName;
    }
  
    public static CommandAttribute GetCommandAttributeOrDefault(this MethodInfo method)
    {
      var attr = method.GetCustomAttribute<CommandAttribute>() ?? new CommandAttribute();
      if (string.IsNullOrEmpty(attr.CommandName)) attr.CommandName = method.Name;
      return attr;
  
    }
    public static MethodInfo GetCommandMethod(this Type type, string commandName)
    {
      var methods = type.MethodsWith<CommandAttribute>().ToArray();
      methods = methods.Where(m => GetCommandAttributeOrDefault(m.Item1).CommandName == commandName).ToArray();
  
      if (methods.Count() == 0)
      {
        methods = type.GetMethods()
          .Where(m => m.Name == commandName)
          .Select(m => Tuple.Create(m, new CommandAttribute(commandName)))
          .ToArray();
      }
      if (methods.Count() == 0) return null;
      if (methods.Count() > 1) return null;
      var method = methods.SingleOrDefault();
      return method.Item1;
    }
  
    public static DelegateCommand[] ReflectCommands(object instance)
    {
      if(instance == null)throw new ArgumentNullException();
      var type = instance.GetType();
      var methods = type.GetCommandMethods();
      var commands = methods.Select(cmd => Command.Reflect(instance, cmd)).ToArray();
      return commands;
    }
  
    public static MethodInfo[] GetCommandMethods(this Type type)
    {
      var commandMethods = type
    .GetMethods()
    .Where(m => m.IsPublic && m.GetParameters().Count() == 0 && m.GetCustomAttributes(typeof(CommandAttribute), true).Any())
    .ToArray();
      return commandMethods;
    }
  
    public static DelegateCommand Reflect(object source, MethodInfo method, MethodInfo canExecute)
    {
      var result = new DelegateCommand();
      result.DisplayName = method.GetDisplayName() ?? method.Name; ;
      result.CommandName = method.Name;

      
  
      if (canExecute != null)
      {
        if (!typeof(bool).IsAssignableFrom(canExecute.ReturnType)) throw new InvalidOperationException("can execute method needs to return a bool");
        switch (canExecute.GetParameters().Count())
        {
          case 0:
            result.CanExecuteCallback = param => (bool)canExecute.Invoke(source, new object[0]);
            break;
          case 1:
            result.CanExecuteCallback = param => (bool)canExecute.Invoke(source, new object[] { param });
            break;
          default:
            throw new NotImplementedException("multiple parameters are currently not implemented");
  
        }
      }
  
      switch (method.GetParameters().Count())
      {
        case 0:
          result.ExecuteCallback = param => method.Invoke(source, new object[0]);
          break;
        case 1:
          result.ExecuteCallback = param => method.Invoke(source, new[] { param });
          break;
        default:
          throw new NotImplementedException("Multiple Parameters are currently not implemented");
      }
  
      return result;
    }
  
    public static DelegateCommand Reflect(object source, MethodInfo method)
    {
      if (source == null) return null;
      var type = source.GetType();
      var commandName = method.Name;
      var canExecuteName = commandName + "CanExecute";
      var canExecute = type.GetMethod(canExecuteName);
      bool isProperty = false;
      if (canExecute == null)
      {
        var prop = type.GetProperty(canExecuteName);
        if (prop != null)
        {
          canExecute = prop.GetAccessors().SingleOrDefault(m => m.Name.StartsWith("get_"));
          if (canExecute != null) { isProperty = true; }
        }
      }
  
      var result = Reflect(source, method, canExecute);
  
      if (isProperty)
      {
        var notify = source as INotifyPropertyChanged;
        if (notify != null)
        {
          notify.PropertyChanged += (sender, args) =>
          {
            if (args.PropertyName == canExecuteName)
            {
              result.RaiseCanExecuteChanged();
            }
          };
        }
      }
  
      return result;
    }
    public static DelegateCommand Reflect(object source, string commandName)
    {
      if (source == null) return null;
      var type = source.GetType();
      var method = GetCommandMethod(type, commandName);
      if (method == null) return null;
  
      return Reflect(source, method);
    }
  }
}
