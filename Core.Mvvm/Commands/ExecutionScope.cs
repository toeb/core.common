using System;
using System.Diagnostics;
using System.Runtime.Remoting.Messaging;

namespace Core.Common.MVVM
{


    public abstract class ExecutionScope<T> : Core.NotifyPropertyChangedBase, IDisposable where T : ExecutionScope<T>
    {
        public static string LogicalName { get { return typeof(T).Name; } }
        protected T Self { get { return (T)this; } }
        public static T Current
        {
            get
            {
                return CallContextEx.GetOrDefault<T>(LogicalName);
            }
            protected set
            {
                CallContext.LogicalSetData(LogicalName, value);
            }
        }

        bool _isDisposed;

        public bool IsDisposed
        {
            get { return _isDisposed; }
        }
        protected virtual void OnDispose()
        {
            if (_isDisposed)
            {
                return;
            }
            _isDisposed = true;
            CallContext.FreeNamedDataSlot(LogicalName);
        }
        public void Dispose()
        {
            OnDispose();
            Trace.TraceInformation($"Closed {this}");

        }

        ~ExecutionScope()
        {
            if(!_isDisposed)
            {
                throw new InvalidOperationException("you forgot to dispose execution scope");
            }
        }

    }
}

