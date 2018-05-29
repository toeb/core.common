using System;

namespace Core.Common.MVVM
{
    public abstract class ExclusiveExecutionScope<T> : ExecutionScope<T> where T : ExclusiveExecutionScope<T>
    {
        public ExclusiveExecutionScope()
        {
            if (Current != null)
            {
                throw new InvalidOperationException($"More than one Scope of type {GetType().Name} is not allowed");
            }
            Current = Self;
        }

        public static T Require(Func<T> factory)
        {
            return Current ?? factory();
        }

        
        
    }
}

