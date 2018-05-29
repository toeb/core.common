using Core.Common.MVVM;
using Core.Common.Reflect;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Common.MVVM
{

    public static class Command
    {


        public static string GetCommandName(this MethodInfo method)
        {

            return GetCommandAttributeOrDefault(method).CommandName;
        }


        public static string DisplayType(this MethodInfo method)
        {
            return GetCommandAttributeOrDefault(method).DisplayType;
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
            if (instance == null) throw new ArgumentNullException();
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


        // needs to evaluate the arguments in calling thread.
        // 
        public static Task ExecuteMethodLazy(object target, MethodInfo method, object[] parameters)
        {
            Task task = null;
            try
            {
                var methodParameters = method.GetParameters();
                var result = method.Invoke(target, parameters.ConcatOne(Enumerable.Repeat<object>(null, methodParameters.Length)).Take(methodParameters.Length).ToArray());
                if (result is Task)
                {
                    task = result as Task;
                }
                else
                {
                    task = Task.Factory.StartNew(() => result, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
                }
            }
            catch (Exception e)
            {
                task = Task.Factory.StartNew(() => { throw e; }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
            }
            return task;
        }

        public static DelegateCommand Reflect(Delegate @delegate, MethodInfo canExecute)
        {
            Func<object, bool> canExecutePredicate = null;
            if (canExecute != null)
            {
                if (!typeof(bool).IsAssignableFrom(canExecute.ReturnType))
                {
                    throw new InvalidOperationException("can execute method needs to return a bool");
                }

                switch (canExecute.GetParameters().Count())
                {
                    case 0:
                        canExecutePredicate = param => (bool)canExecute.Invoke(@delegate.Target, new object[0]);
                        break;
                    case 1:
                        canExecutePredicate = param => (bool)canExecute.Invoke(@delegate.Target, new object[] { param });
                        break;
                    default:
                        throw new NotImplementedException("multiple parameters are currently not implemented");

                }
            }
            else
            {
                canExecutePredicate = o => true;
            }

            var result = new DelegateCommand(@delegate, canExecutePredicate);


            var attribute = @delegate.Method.GetCommandAttributeOrDefault();
            result.AllowConcurrentExecution = attribute.AllowConcurrentExecution;

            return result;
        }

        public static DelegateCommand Reflect(object source, MethodInfo method)
        {
            var del = method.CreateDelegate(source);
            return Reflect(del);
        }

        public static DelegateCommand Reflect(Delegate del)
        {
            var target = del.Target;
            var method = del.Method;


            var type = method.ReflectedType;
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

            var result = Reflect(del, canExecute);

            if (isProperty)
            {
                var notify = target as INotifyPropertyChanged;
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



        public static DelegateCommand CreateCommand(Action del)
        {

            return Reflect(del.Target, del.Method);
        }


        public static DelegateCommand CreateCommand<T>(Func<T> del)
        {
            return Reflect(del.Target, del.Method);
        }

        public static DelegateCommand CreateCommand<T>(Action<T> del)
        {
            return Reflect(del.Target, del.Method);
        }


        public static DelegateCommand CreateCommand(object target, MethodInfo methodInfo)
        {
            return Command.Reflect(target, methodInfo);
        }



    }
}
