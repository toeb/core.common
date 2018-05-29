using Core.Common.MVVM;
using Core.Common.Reflect;
using System;
using System.Windows.Input;

namespace Core.Common.Wpf.Export
{
    /// <summary>
    /// A Wpf Xaml Markup Extension that allows direct binding to methods, Command Properties, and Delegate Properties
    /// Allows specifiing n Data Bound Command Parameters {Command MethodName, {Binding Argument1}, argumentstring2, 3, {Binding Argument4}}
    /// 
    /// </summary>
    public class CommandExtension : CommandExtensionBase
    {

        /// <summary>
        /// Causes the Command Scope to ignore this command
        /// </summary>
        public bool IgnoreScope { get; set; }

        private CommandExtension(object command, object[] parameters) : base(command, parameters) { this.InternalCommand = new CommandExtensionCommand(this); }
        public CommandExtension() : this(null, Array.Empty<object>()) { }
        public CommandExtension(object command) : this(command, Array.Empty<object>()) { }
        public CommandExtension(object command, object arg1) : this(command, new[] { arg1 }) { }
        public CommandExtension(object command, object arg1, object arg2) : this(command, new[] { arg1, arg2 }) { }
        public CommandExtension(object command, object arg1, object arg2, object arg3) : this(command, new[] { arg1, arg2, arg3 }) { }


        private static ICommand ResolveCommand(object target, object command)
        {

            if (command is string methodName)
            {
                if (target == null)
                {
                    return null;
                }
                if (DelegateExtension.ResolveDelegate(target, target?.GetType(),null, methodName) is Delegate del)
                {
                    return ResolveCommand(target, del);
                }


                var value = target.GetProperty(methodName);

                if (value != null)
                {
                    return ResolveCommand(target, value);
                }


                if ((methodName == "" || methodName == ".") && target is Delegate @delegate)
                {
                    return ResolveCommand(target, @delegate);
                }
            }
            else if (command is Delegate @delegate)
            {
                return Core.Common.MVVM.Command.Reflect(@delegate);
            }
            else if (command is ICommand cmd)
            {
                return cmd;
            }
            return null;
        }


        public override ICommand CoerceCommand(object value, object[] parameters)
        {
            TargetObject.AfterLoad(() =>
            {
                var cmd = ResolveCommand(TargetObject?.DataContext, value);
                this.InternalCommand.Command = cmd;

            });

            this.InternalCommand.Parameters = parameters;
            return this.InternalCommand;
        }



        public CommandExtensionCommand InternalCommand { get; }

        public class CommandExtensionCommand : CommandBase
        {
            private ICommand _command;
            public ICommand Command
            {
                get => _command;
                set
                {
                    if (_command != null) _command.CanExecuteChanged -= CommandCanExecuteChanged;
                    _command = value;
                    if (_command != null) _command.CanExecuteChanged += CommandCanExecuteChanged;
                    RaiseCanExecuteChanged();
                }
            }
            public CommandExtension Extension { get; private set; }

            public CommandExtensionCommand(CommandExtension extension)
            {
                this.Extension = extension;
            }

            public string Name { get { return Extension?.TargetObject?.Name; } }

            public bool IgnoreScope { get { return Extension.IgnoreScope; } }

            public object[] Parameters { get; internal set; }

            private void CommandCanExecuteChanged(object sender, EventArgs e)
            {
                RaiseCanExecuteChanged();
            }



            public override bool CanExecute(object parameter)
            {
                if (Command == null)
                {
                    return false;
                }
                return Command.CanExecute(parameter);
            }

            public override void Execute(object parameter)
            {

                if (Command == null)
                {
                    return;
                }
                // if command parameter was set then it is prioritized, else
                parameter = parameter ?? new ArgumentEvaluator(Parameters);

                using (var context = CommandExecutionContext.Require(Command, parameter))
                {
                    if (!IgnoreScope)
                    {
                        CommandScope.NotifyCommandStarted(Extension.TargetObject, context);
                    }
                    Command.Execute(parameter);

                }

            }

        }

    }






}
