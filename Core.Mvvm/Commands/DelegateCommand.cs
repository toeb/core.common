using System;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.Remoting.Messaging;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Core.Common.MVVM
{



    public class DelegateCommand : ExtendedCommandBase, IArgumentScope
    {
        public Func<object, bool> CanExecuteCallback { get; private set; }
        public Delegate Delegate { get; private set; }


        public static DelegateCommand Create(Action action) { return new DelegateCommand(action); }
        public static DelegateCommand Create<T1>(Action<T1> action) { return new DelegateCommand(action); }
        public static DelegateCommand Create<T>(Func<T> action) { return new DelegateCommand(action); }
        public static DelegateCommand Create<T1, T>(Func<T1, T> action) { return new DelegateCommand(action); }




        public DelegateCommand(Delegate execute) : this(execute, null) { }
        public DelegateCommand(Delegate execute, Func<object, bool> canExecute)
        {
            Delegate = execute ?? throw new NotImplementedException();
            CanExecuteCallback = canExecute ?? (o => true);
        }
        protected override bool CanExecuteImpl(object parameter)
        {
            return CanExecuteCallback(parameter);
        }

        public override string CommandId
        {
            get
            {
                return Delegate.Method.DeclaringType.Name + "." + Delegate.Method.Name;
            }
        }


        public ObservableCollection<CommandExecutionContext> ExecutionContexts { get; } = new ObservableCollection<CommandExecutionContext>();

        protected override Task ExecuteAsyncImpl(CommandExecutionContext parameter)
        {
            ExecutionContexts.Add(parameter);
            // need to be invoked here, because the methods might need to be evaluated 
            // in the gui thread
            var parameters = parameter.CapturedParameters;

            // take as many parameters as are available and can be taken            
            parameters = parameters.Take(Delegate.Method.GetParameters().Count()).ToArray();

            if (typeof(Task).IsAssignableFrom(Delegate.Method.ReturnType))
            {
                return Delegate.DynamicInvoke(parameters) as Task;
            }
            else
            {
                try
                {
                    return Task.FromResult(Delegate.DynamicInvoke(parameters));
                }
                catch (Exception e)
                {
                    return Task.Run(() => throw e);
                }


            }
        }

        public object[] Capture(object arguments)
        {
            if (arguments is IMethodArguments args)
            {
                return args.Evaluate(Delegate.Method.GetParameters().Select(p => p.ParameterType).ToArray());
            }
            else
            {
                return new[] { arguments };
            }
        }
    }
}
