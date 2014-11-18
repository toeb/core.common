using Core.Modules;
using System;
using System.ComponentModel.Composition;
using System.Threading;

namespace Core.Modules.Applications.Runner
{


  [Module]
  [Export(typeof(IApplicationRunner))]
  public class ApplicationRunnerModule :  IApplicationRunner, IModule
  {
    private ApplicationRunnerModule() { }
    [Import]
    IApplication CurrentApplication
    {
      get;
      set;
    }
    ManualResetEvent mre = new ManualResetEvent(false);
    private bool stopping = false;
    public bool IsStopping
    {
      get { return stopping; }
    }

    public void Start()
    {
     
      mre.Reset();
      stopping = false;
      if (Starting != null) Starting();
    }



    public void Await()
    {
      mre.WaitOne();
    }

    public event Action Stopping;
    public event Action Starting;



    public void Stop()
    {
      if (Stopping != null) Stopping();
      mre.Set();
    }

    public void Activate()
    {

    }

    public void Deactivate()
    { 
      Stop();
    }
  }
}
