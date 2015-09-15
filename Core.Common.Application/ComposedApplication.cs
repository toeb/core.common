using Core.Common.MVVM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;

namespace Core.Common.Applications
{
  public abstract class ComposedApplication :
    IComposedApplication,
    IViewModelProvider,
    IApplicationService
  {
    private CompositionContainer container;
    protected ComposedApplication(CompositionContainer container)
    {
      this.container = container;
    }

    #region Exports
    [Export]
    public IComposedApplication ThisApplication { get { return this; } }
    [Export]
    public IViewModelProvider ViewModelProvider { get { return this; } }
    [Export]
    public IApplicationService ApplicationService { get { return this; } }


    [Export]
    public virtual IDispatcher Dispatcher { get { return SimpleDispatcher.Instance; } }
    #endregion




    protected void Startup()
    {
      container.ComposeExportedValue(Dispatcher);
      container.ComposeExportedValue(ThisApplication);
      container.ComposeExportedValue(ViewModelProvider);
      container.ComposeExportedValue(ApplicationService);
      OnStarting();
      var it1 = container.GetExports<IViewManager>();

      var exports = Container.GetExports<IViewModel, IApplicationViewMetadata>();
      if (exports.Count() == 0)
      {
        Shutdown();
        throw new Exception("failed to find a startup viewmodel");
      }

      foreach (var export in exports)
      {
        var viewModel = export.Value;
        StartViewModel(viewModel);
      }
      OnStarted();
    }

    protected virtual void OnStarted()
    {
    }

    protected virtual void OnStarting()
    {
    }

    protected abstract void StartViewModel(IViewModel viewModel);


    /// <summary>
    /// The Root Composition Container
    /// </summary>
    public CompositionContainer Container
    {
      get { return container; }
    }

    /// <summary>
    /// shutsdown the managed application
    /// </summary>
    /// 
    public void Shutdown()
    {
      isShuttingDown = true;
      OnShuttingDown();
      if (!isShuttingDown) DoShutdown();
      OnShutdown();
    }
    bool isShuttingDown = false;

    protected virtual void OnShutdown()
    {

    }

    protected virtual void OnShuttingDown()
    {
      if (BeforeShutdown != null) BeforeShutdown(this, EventArgs.Empty);

    }

    protected abstract void DoShutdown();




    public IEnumerable<IFactory<object>> FindViewModels(Func<IViewModelMetadata, bool> predicate)
    {
      var exports = Container.GetExports<IViewModel, IViewModelMetadata>();


      var viewModels = exports.Where(export => predicate(export.Metadata));


      foreach (var viewModel in viewModels)
      {
        yield return new DelegateFactory<object>(() => viewModel.Value);
      }

    }



    public event EventHandler BeforeShutdown;
  }
}
