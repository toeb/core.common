using System;
namespace Core.Common.MVVM
{
  using System.Diagnostics;
  using System.Threading.Tasks;


  public class DelegateCommand : ViewModelBase, IUiCommand
  {
    public Func<object, bool> CanExecuteCallback { get; set; }
    public Action<object> ExecuteCallback { get; set; }

    public event EventHandler CanExecuteChanged;
    public string CommandName { get { return Get<string>(); } set { Set(value); } }


    [DependsOn("CommandName")]
    public string DisplayName { get { return Get<string>() ?? CommandName; } set { Set(value); } }


    public void RaiseCanExecuteChanged()
    {
      if (CanExecuteChanged != null) CanExecuteChanged(this, EventArgs.Empty);
    }
    public DelegateCommand() { }

    public DelegateCommand(Action<object> execute, Func<object, bool> canExecute)
    {
      ExecuteCallback = execute;
      CanExecuteCallback = canExecute;
    }
    public bool CanExecute(object parameter)
    {
      if (CanExecuteCallback == null) return true;
      return CanExecuteCallback(parameter);
    }


    public Task ExecuteAsync(object parameter)
    {
      var task = new Task(() =>
      {
        Trace.WriteLine(CommandName + " executed");
        ExecuteCallback(parameter);
      });
      task.RunSynchronously();
      return task;

    }

    public string CommandId
    {
      get { return null; }
    }

    public override void OnAfterConstruction()
    {
    }

    public override void OnDispose()
    {
    }
  }
}
