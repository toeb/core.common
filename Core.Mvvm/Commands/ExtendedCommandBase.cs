using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Threading;
using System.Windows;
using System.Collections.Concurrent;

namespace Core.Common.MVVM
{
    public abstract class ExtendedCommandBase : CommandBase,  ICommand
    {
        public event Action AfterExecute;
        public event Action Executing;
        public event Action BeforeExecute;


        protected abstract bool CanExecuteImpl(object parameter);
        // may not throw
        protected abstract Task ExecuteAsyncImpl(CommandExecutionContext parameter);




        public sealed override bool CanExecute(object parameter)
        {
            return CanExecuteImpl(parameter);
        }

        public sealed override void Execute(object parameter)
        {
            
            Trace.TraceInformation("Command Dispatch Starting");
            ExecuteAsync(parameter)
                .ContinueWith(t =>
            {
                if (t.IsFaulted)
                {

                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {

                        throw t.Exception;
                    }));
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
            Trace.TraceInformation("Command Dispatch Finished");
        }



        public override string ToString()
        {
            return CommandId;
        }
        public virtual string CommandId { get { return "CommandBase"; } }







        private bool _allowConcurrentExecution;

        public bool AllowConcurrentExecution
        {
            get { return _allowConcurrentExecution; }
            set { this.ChangeProperty(() => AllowConcurrentExecution, ref _allowConcurrentExecution, value); }
        }

        private bool _isExecuting;
        public bool IsExecuting
        {
            get { return _isExecuting; }
            set { this.ChangeProperty(() => IsExecuting, ref _isExecuting, value); }
        }


        private ConcurrentQueue<CommandExecutionContext> _executionQueue = new ConcurrentQueue<CommandExecutionContext>();


        private Task QueueExecution(CommandExecutionContext context)
        {
            //_executionQueue.Enqueue(context);


            var task = ExecuteAsyncImpl(context);

            if (task.Status == TaskStatus.Created)
            {
                var exception = new InvalidOperationException("Task was not started. ");
                context.Signal.SetException(exception);
                throw exception;
            }

            return task;
        }



        public Task ExecuteAsync(object parameter)
        {
           
            using (var context = CommandExecutionContext.Require(this, parameter)) 
            {

                Trace.TraceInformation($"Beginning Execution of Command {context} on Thread-{Thread.CurrentThread.ManagedThreadId}");



                BeforeExecuteCallback(context);
                context.IsExecuting = true;
                Trace.TraceInformation("queuing command to run...");

                var result = QueueExecution(context);
                if (result == null)
                {
                    Trace.TraceWarning("failed to queue command to run");
                    throw new InvalidOperationException("did not run command");
                }
                else
                {
                    context.CommandTask = result;
                }

                ExecutingCallback(context);

                return context.CommandTask.ContinueWith(t =>
                {
                    AfterExecuteCallback(context);
                    context.Signal.SetResult(context);
                    context.IsExecuting = false;

                    if (t.IsFaulted)
                    {
                        throw t.Exception;

                    }
                    Trace.TraceInformation("command finished  on thread " + Thread.CurrentThread.ManagedThreadId);
                }, CancellationToken.None, TaskContinuationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());




            }
        }


        private void ExecutingCallback(CommandExecutionContext ctx)
        {
            Executing?.Invoke();
        }

        private void BeforeExecuteCallback(CommandExecutionContext ctx)
        {
            BeforeExecute?.Invoke();
        }


        private void AfterExecuteCallback(CommandExecutionContext ctx)
        {
            AfterExecute?.Invoke();
            Trace.TraceInformation(" ... command state" + ctx.CommandTask.Status);


        }
    }
}
