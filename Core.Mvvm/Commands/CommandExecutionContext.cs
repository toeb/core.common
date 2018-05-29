using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Core.Common.MVVM
{
    public class CommandExecutionContext : ExclusiveExecutionScope<CommandExecutionContext>
    {
        public static CommandExecutionContext Require(ICommand command, object parameter)
        {
            return Require(() => new CommandExecutionContext(command, parameter));
        }


        public CommandExecutionContext(ICommand command, object parameter)
        {
            if (command is IArgumentScope scope)
            {
                _parameters = scope.Capture(parameter);
            }
            else
            {
                _parameters = new[] { parameter };
            }




            Trace.TraceInformation("command execution context initialized");
            Signal = new TaskCompletionSource<CommandExecutionContext>();
            SignalTask = Signal.Task;
            Command = command;
            Parameter = parameter;
            ProgressScope = new ProgressScope();
            CancellationScope = new CancellationScope();
            ProgressScope.Progress = 1;
        }

        internal void Pause()
        {
            throw new NotImplementedException();
        }


        public override string ToString()
        {
            return $"{Command}({string.Join(", ", _parameters.Select(p => p?.ToString()))})";
        }

        public const string CurrentCommandContextIdentifier = nameof(CommandExecutionContext);
        public ProgressScope ProgressScope { get; private set; }

        public IEnumerable<CancellationScope> CancellationScopes
        {
            get
            {
                yield return CancellationScope;
            }
        }
        public IEnumerable<ProgressScope> ProgressScopes
        {
            get
            {
                yield return ProgressScope;
            }
        }

        protected override void OnDispose()
        {

            if (ProgressScope != null)
            {
                ProgressScope.Dispose();
            }
            if (CancellationScope != null)
            {
                CancellationScope.Dispose();
            }


            base.OnDispose();
        }



        public void Cancel()
        {
            _isCancelling = true;
        }

        private bool _isExecuting;
        public bool IsExecuting
        {
            get { return _isExecuting; }
            set { this.ChangeProperty(ref _isExecuting, value); }
        }

        private bool _isCancelling = false;
        bool IsCancelling { get { return _isCancelling; } }

        bool IsRunning { get; set; }

        ~CommandExecutionContext()
        {
            Console.WriteLine("destructed execution context");
            Dispose();
        }


        public TaskCompletionSource<CommandExecutionContext> Signal { get; private set; }

        public ICommand Command { get; private set; }
        public object Parameter { get; private set; }
        public Task SignalTask { get; internal set; }
        public Task CommandTask { get; internal set; }
        public CancellationToken CancellationToken { get; internal set; }
        public IProgressReporter ProgressReporter { get; internal set; }

        private object _commandResult;
        public object CommandResult
        {
            get
            {
                return _commandResult ?? (_commandResult = ((dynamic)CommandTask).Result);
            }
        }

        private Dictionary<string, object> _data;
        public IDictionary<string, object> Data { get { return _data ?? (_data = new Dictionary<string, object>()); } }

        public object[] CapturedParameters
        {
            get
            {
                return _parameters;
            }

        }

        public CancellationScope CancellationScope { get; private set; }
        public bool CanCancel { get; internal set; }
        public bool CanPause { get; internal set; }

        private object[] _parameters;

    }
}

