using System;
namespace Core.Common.MVVM
{
    using System.Windows.Input;
    public class RelayCommand : ICommand
    {
        Action action;
        public RelayCommand(Action action)
        {
            this.action = action;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        private void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
            {
                CanExecuteChanged(this, new EventArgs());
            }
        }
        public void Execute(object parameter)
        {
            action();
        }
    }
}
