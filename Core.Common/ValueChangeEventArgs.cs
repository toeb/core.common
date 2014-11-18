using System;

namespace Core
{
  public enum ChangeState
  {
    Changing,
    Changed
  }
  public class ValueChangeEventArgs : EventArgs
  {
    public ValueChangeEventArgs(ChangeState state, object olValue, object newValue)
    {
      this.OldValue = olValue;
      this.NewValue = newValue;
    }
    public ValueChangeEventArgs(ChangeState state, object newValue)
    {
      OldValue = null;
      NewValue = newValue;
      OldValueAvailable = false;
    }
    public ValueChangeEventArgs(ChangeState state)
    {
      OldValueAvailable = false;
      NewValueAvailable = false;
    }
    public ChangeState ChangeState { get; private set; }
    public bool OldValueAvailable { get; private set; }
    public bool NewValueAvailable { get; private set; }

    public object OldValue { get; private set; }
    public object NewValue { get; private set; }
  }
  public class ValueProducedEventArgs : EventArgs
  {
    public ValueProducedEventArgs() { IsValueAvailable = false; }
    public ValueProducedEventArgs(object producedValue) { IsValueAvailable = true; ProducedValue = producedValue; }
    public object ProducedValue { get; private set; }
    public bool IsValueAvailable { get; private set; }
  }
  public class ValueConsumedEventArgs : EventArgs
  {
    public ValueConsumedEventArgs() { IsValueAvailable = false; }
    public ValueConsumedEventArgs(object consumedValue) { IsValueAvailable = true; ConsumedValue = consumedValue; }
    public object ConsumedValue { get; private set; }
    public bool IsValueAvailable { get; private set; }
  }
}
