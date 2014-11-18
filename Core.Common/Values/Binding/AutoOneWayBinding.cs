using System;
using System.ComponentModel;

namespace Core.Values
{
  class AutoOneWayBinding : SimpleManualOneWayBinding
  {
    public AutoOneWayBinding(ISource source, ISink sink, bool enable = true) : base(source, sink, enable) { }
    public AutoOneWayBinding() { }
    protected override void Bind()
    {
      base.Bind();
      Source.ValueChanged += ValueChanged;
    }

    private void ValueChanged(object sender, ValueChangeEventArgs args)
    {
      Sync();
    }
    protected override void Unbind()
    {
      Source.ValueChanged -= ValueChanged;
      base.Unbind();
    }
  }
}
