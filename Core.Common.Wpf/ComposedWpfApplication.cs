using Core.Common.Applications;
using Core.Common.MVVM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Core.Common.Wpf
{
  /// <summary>
  ///  A Composed Application creates all parts of an application dynamically using a composition container and
  ///  a reference to an WpfApplication.
  /// </summary>
  public class ComposedWpfApplication :
    ComposedApplication,
    IViewManager,
    IDispatcher,
    IViewProvider
  {
    private Application application;

    public ComposedWpfApplication(Application application, CompositionContainer container)
      : base(container)
    {
      this.application = application;
      application.Startup += (sender, args) => this.Startup();
      application.Exit += (sender, args) => this.Shutdown();
      application.DispatcherUnhandledException += ExceptionHandler;
    }
    protected override void OnStarting()
    {

      Container.ComposeExportedValue(ViewManager);
    }

    /// <summary>
    /// Global UI Exception Handler
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ExceptionHandler(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
    {
      var result = MessageBox.Show(e.Exception.ToString() + "\n\n Continue?", "Exception", MessageBoxButton.YesNo, MessageBoxImage.Error, MessageBoxResult.Yes, MessageBoxOptions.None);
      if (result == MessageBoxResult.Yes)
      {
        e.Handled = true;
      }
    }

    # region Exports
    [Export]
    public override IDispatcher Dispatcher { get { return this; } }
    [Export]
    public IViewManager ViewManager { get { return this; } }
    #endregion

    protected override void StartViewModel(IViewModel viewModel)
    {
      ShowView(viewModel);
    }
    protected override void DoShutdown()
    {
      application.Shutdown();
    }
    /// <summary>
    /// Creates a view for the specified viewmodel
    /// </summary>
    /// <param name="viewmodel"></param>
    /// <returns></returns>
    public IView CreateViewFor(object viewmodel)
    {
      var exports = Container.GetExports<IView, IViewMetadata>();
      var export = exports.FirstOrDefault(exp => exp.Metadata.ViewModelType.IsAssignableFrom(viewmodel.GetType()));
      if (export == null) return null;
      var control = export.Value;
      return control;
    }


    /// <summary>
    /// Creates a window for the specified viewModel by
    /// getting a view and setting it up inside a window if necessary
    /// </summary>
    /// <param name="viewModel"></param>
    /// <returns></returns>
    private Window CreateWindowFor(object viewModel)
    {
      var view = CreateViewFor(viewModel) as ContentControl;
      if (view == null) return null;
      var window = view as Window;
      if (window == null)
      {
        window = new Window();
        window.Content = view;
        view.Width = double.NaN;
        view.Height = double.NaN;
        window.Width = double.NaN;
        window.Height = double.NaN;
      }
      window.Tag = this;
      window.DataContext = viewModel;

      return window;
    }

    /// <summary>
    /// shows a view to the user by creating a new window
    /// </summary>
    /// <param name="viewmodel"></param>
    public IViewHandle ShowView(object viewmodel)
    {
      
      IViewHandle viewHandle = viewHandles.FirstOrDefault(v=>v.ViewModel==viewmodel);
      if (viewHandle != null)
      {
        viewHandle.Activate();
        return viewHandle;
      }
      foreach (var submanager in SubviewManagers)
      {
        viewHandle = submanager.ShowView(viewmodel);
        if (viewHandle != null) break;
      }

      if (viewHandle == null)
      {
        var window = CreateWindowFor(viewmodel);
        if (window == null)
          throw new Exception("could not show view for " + viewmodel);

        window.Show();
        viewHandle = new WindowViewHandle(this, viewmodel, window);

      }
      if (viewHandle == null) return null;
      viewHandles.Add(viewHandle);
      return viewHandle;
    }

    ICollection<IViewHandle> viewHandles = new List<IViewHandle>();

    class WindowViewHandle : ViewHandle
    {
      private ComposedWpfApplication self;
      public WindowViewHandle(ComposedWpfApplication self, object viewModel, Window window):base(viewModel)
      {
        this.Window = window;
        this.self = self;
      }

      public Window Window { get; set; }


      public override void Close()
      {
        Window.Close();
        self.viewHandles.Remove(this);
      }

      public override void Activate()
      {
        Window.Activate();
      }
    }


    public IEnumerable<ISubViewManager> SubviewManagers
    {
      get
      {

        return Container.GetExportedValues<ISubViewManager>();
      }
    }

    public void Dispatch(Action action)
    {
      Application.Current.Dispatcher.Invoke(action);
    }

    public IEnumerable<IFactory<IView>> FindViews(Func<IViewMetadata, bool> predicate)
    {


      var exports = Container.GetExports<IView, IViewMetadata>();
      var filteredExports = exports.Where(l => predicate(l.Metadata));

      foreach (var export in filteredExports)
      {
        yield return new DelegateFactory<IView>(() => export.Value);
      }

    }


  }

  




}
