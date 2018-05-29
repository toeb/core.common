using System;
using System.Windows.Input;
using System.Collections.Generic;
using Core.Common.Injection;

namespace Core.Common.MVVM
{
    public abstract class CommandBase : Core.NotifyPropertyChangedBase, ICommand
    {
        public event EventHandler CanExecuteChanged;

        public abstract bool CanExecute(object parameter);
        public abstract void Execute(object parameter);

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
