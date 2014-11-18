using System;
using System.ComponentModel;

namespace Core.Values
{

  public abstract class BinaryBinding : NotifyPropertyChangedBase, IDisposable, IBinding
  {
    private bool reenable = false;
    private BindingState state;
    private IConnector source;
    private IConnector sink;
    protected BinaryBinding()
    {
      state = BindingState.Disabled;
    }
    protected BinaryBinding(IConnector source, IConnector sink, bool enable)
    {
      Source = source;
      Sink = sink;
      if (enable) Enable();
    }
    private static readonly string SourceName = "Source";
    public IConnector Source { get { return source; } set { ChangeIfDifferentAndCallback(ref source, value, SourceChanging, SourceChanged,SourceName); } }

    private void SourceChanging(IConnector oldValue, IConnector newValue)
    {
      if (State == BindingState.Enabled)
      {
        reenable = true;
        Disable();
      }

    }
    private static readonly string SinkName = "Sink";
    public IConnector Sink { get { return sink; } set { ChangeIfDifferentAndCallback(ref sink, value, SinkChanging, SinkChanged, SinkName); } }

    private void SinkChanging(IConnector oldValue, IConnector newValue)
    {
      if (State == BindingState.Enabled)
      {
        reenable = true;
        Disable();
      }

    }

    public void Enable()
    {
      if (State == BindingState.Enabled) return;
      Bind();
      State = BindingState.Enabled;
    }
    public void Disable()
    {
      if (State == BindingState.Disabled) return;
      Unbind();
      State = BindingState.Disabled;
    }

    protected abstract void Bind();
    protected abstract void Unbind();

    private void SourceChanged(IConnector oldValue, IConnector newValue)
    {
      if (reenable)
      {
        reenable = false;
        Enable();
      }
      OnSourceChanged();
    }

    private void SinkChanged(IConnector oldValue, IConnector newValue)
    {

      if (reenable)
      {
        reenable = false;
        Enable();
      }
      OnSinkChanged();
    }
    private void StateChanged(BindingState oldValue, BindingState newValue) { OnStateChanged(); }

    protected virtual void OnSourceChanged() { }
    protected virtual void OnSinkChanged() { }
    protected virtual void OnStateChanged() { }



    public BindingState State
    {
      get { return state; }
      private set { ChangeIfDifferentAndCallback(ref state, value, StateChanging, StateChanged); }
    }

    private void StateChanging(BindingState oldValue, BindingState newValue)
    {

    }



    public void Dispose()
    {
      Disable();
    }
  }
}
