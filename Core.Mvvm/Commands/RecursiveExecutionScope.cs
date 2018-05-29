using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Threading;

namespace Core.Common.MVVM
{
    public abstract class RecursiveExecutionScope<T> : ExecutionScope<T>, IDirectedGraphNode<T> where T : RecursiveExecutionScope<T>
    {
        int idCounter = 0;
        private int _id;
        private T _parent;
        private SynchronizationContext _syncContext;
        private T[] _children = new T[] { };
        protected override Action<Action> Dispatcher => a => _syncContext.Post(o => a(), null);






        private RecursiveExecutionScope(T parent)
        {
            Contract.Assert(this.GetType() == typeof(T));
            Contract.Assert(parent == null || this.GetType() == parent.GetType());

            _parent = parent;
            if (_parent != null)
            {
                _id = ++_parent.idCounter;
                _parent.AddChild(Self);
                _syncContext = _parent._syncContext;
            }
            else
            {
                _syncContext = SynchronizationContext.Current;
            }
            Current = Self;
            Trace.TraceInformation($"Created {this}");
        }


        protected RecursiveExecutionScope() : this(Current)
        {

        }


        public override string ToString()
        {
            return $"{LogicalName}: [{string.Join(".", PathFromRoot.Select(p => p._id.ToString()))}]";
        }
        public IEnumerable<T> PathFromRoot
        {
            get
            {
                return this.GetAncestorsAndSelf().Cast<T>().Reverse();
            }
        }
        public T Parent
        {
            get
            {
                return _parent;
            }
        }




        public T[] Children
        {
            get
            {
                return _children;
            }
            private set
            {
                this.ChangeProperty(ref _children, value);
            }
        }



        public virtual void DescendantAdded(T child) { }
        public virtual void DescendantRemoved(T child) { }

        private void AddChild(T scope)
        {
            Children = Children.Concat(new[] { scope }).ToArray();
            foreach (var ancestor in this.GetAncestorsAndSelf().ToArray())
            {
                ancestor.DescendantAdded(scope);
            }
        }


        protected override void OnDispose()
        {
            if (_children.Any())
            {
                return;
            }
            if (_parent != null)
            {
                _parent.RemoveChild((T)this);
                CallContext.LogicalSetData(LogicalName, _parent);
            }
            else
            {
                base.OnDispose();
            }

        }




        private void RemoveChild(T scope)
        {
            if (!Children.Contains(scope))
            {
                return;
            }

            Children = Children.Except(new[] { scope }).ToArray();

            foreach (var ancestor in this.GetAncestorsAndSelf().ToArray())
            {
                ancestor.DescendantRemoved(scope);
            }

            if (Children.Length == 0)
            {
                Dispose();
            }
        }



        IEnumerable<T> IDirectedGraphNode<T>.GetChildren()
        {
            return this.Children;
        }

        IEnumerable<T> IDirectedGraphNode<T>.GetParents()
        {
            yield return this.Parent;
        }
    }
}

