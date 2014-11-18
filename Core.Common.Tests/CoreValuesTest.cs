using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core.TestingUtilities;
using System.ComponentModel;
using System.Linq;
using System.Collections.Generic;
using Core.Merge;
using System.IO;
using Newtonsoft.Json;
using System.Threading;

namespace Core.Values.Test
{
  [TestClass]
  public class CoreValuesTest : TestBase
  {
    [TestMethod]
    public void CreateValue()
    {
      var value = Value.Memory<object>();
      Assert.IsNull(value.Value);
      Assert.IsNotNull(value.ValueInfo);
      Assert.AreEqual(typeof(object), value.ValueInfo.ValueType);
      Assert.IsTrue(value.ValueInfo.IsReadable);
      Assert.IsTrue(value.ValueInfo.IsWriteable);
      Assert.IsFalse(value.ValueInfo.OnlyExactType);
    }
    [TestMethod]
    public void CreateValueTypedValue()
    {
      var value = Value.Memory(55);
      Assert.AreEqual(typeof(int), value.ValueInfo.ValueType);
      Assert.AreEqual(55, value.Value);
    }
    [TestMethod]
    public void SourceValue()
    {
      var val = 3;
      var result = Value.Source(val);
      Assert.AreEqual(3, result.Value);
    }
    [TestMethod]
    public void ReferenceTypeSource()
    {
      var val = new object();
      var result = Value.Source(val);
      Assert.IsTrue(object.ReferenceEquals(val, result.Value));
    }
    [TestMethod]
    public void SinkValue()
    {
      var val = 0;
      var result = Value.Sink<int>((v) => val = v);
      result.Value = 4;
      Assert.AreEqual(4, val);
    }
    [TestMethod]
    public void ConstSource()
    {
      var val = 5;
      var result = Value.ConstSource(val);
      Assert.AreEqual(5, result.Value);
    }
    class MyTestClass { public string MyProperty { get; set; } }
    [TestMethod]
    public void ConstReferenceTypeSource()
    {
      var val = new MyTestClass { MyProperty = "hello" };
      var result = Value.ConstSource(val);
      Assert.IsFalse(object.ReferenceEquals(result.Value, val));
      Assert.AreEqual("hello", result.Value.MyProperty);
    }
    [TestMethod]
    public void DelegateValue()
    {
      var val = 3;
      var del = Value.Delegate(() => val, (v) => val = v);

      Assert.AreEqual(3, del.Value);
      del.Value = 5;
      Assert.AreEqual(5, val);
    }
    [TestMethod]
    public void OneWayBinding()
    {
      var val1 = 1;
      var val2 = 3;
      var del1 = Value.Delegate(() => val1, v => val1 = v);
      var del2 = Value.Delegate(() => val2, v => val2 = v);

      var uut = Binding.OneWay(del1, del2, false);
      // on instanciation value may not be bound
      Assert.AreEqual(3, val2);
      // val2 needs to be set to val1 by one way binding on Enable
      uut.Enable();
      Assert.AreEqual(val1, val2);
      del1.Value = 99;
      Assert.AreEqual(99, val2);
      del2.Value = 22;
      Assert.AreEqual(99, val1);
    }
    [TestMethod]
    public void TwoWayBinding()
    {
      var val1 = 1;
      var val2 = 2;
      var del1 = Value.Delegate(() => val1, v => val1 = v);
      var del2 = Value.Delegate(() => val2, v => val2 = v);

      var uut = Binding.TwoWay(del1, del2, false);
      Assert.AreEqual(1, val1);
      Assert.AreEqual(2, val2);
      uut.Enable();
      Assert.AreEqual(val1, val2);
      del1.Value = 55;
      Assert.AreEqual(val1, val2);
      Assert.AreEqual(55, val2);
      del2.Value = 66;
      Assert.AreEqual(val1, val2);
      Assert.AreEqual(66, val1);
      //Assert.AreEqual(1,val1); Assert.AreEqual(1,val1)  // these conditions are not guaranteed

    }
    [TestMethod]
    public void OneWayBindingSourceToSink()
    {
      var val = 3;
      var sink = Sink.Delegate<int>((i) => val = i);
      var val2 = 5;
      var source = Source.Delegate(() => val2);

      var binding = Binding.OneWayManual(source, sink);
    }
    [TestMethod]
    public void AutoOneWay()
    {
      var val = 3;
      var sink = Sink.Delegate<int>((i) => val = i);
      var val2 = 5;
      var source = Source.Delegate(() => val2);

      var binding = Binding.OneWayManual(source, sink);

    }
    [TestMethod]
    public void CreateDelegateSink()
    {
      int val = 0;
      var uut = Sink.Delegate<int>(i => { val = i; });
      Assert.IsTrue(uut.SinkInfo.IsWriteable);
      Assert.IsTrue(uut.CanConsume(4));
      Assert.AreEqual(typeof(int), uut.SinkInfo.ValueType);
      Assert.IsFalse(uut.SinkInfo.OnlyExactType);
      uut.Value = 5;
      Assert.AreEqual(5, val);
    }
    [TestMethod]
    public void CreateDelegateSource()
    {
      int val = 5;
      var uut = Source.Delegate(() => val);
      Assert.IsTrue(uut.CanProduce());
      Assert.IsTrue(uut.SourceInfo.IsReadable);
      Assert.IsFalse(uut.SourceInfo.OnlyExactType);
      Assert.AreEqual(typeof(int), uut.SourceInfo.ValueType);
      Assert.AreEqual(5, uut.Value);
      val = 6;
      Assert.AreEqual(6, uut.Value);
    }
    [TestMethod]
    public void DelegateNotifyValueChanged()
    {
      int val = 0;
      NotifyValueChangedDelegate notifier;
      var uut = Source.Delegate(() => val, out notifier);
      int newValue = 0;
      uut.ValueChanged += (sender, args) => newValue = uut.Value;
      val = 3;
      notifier();
      Assert.AreEqual(newValue, 3);

    }
    [TestMethod]
    public void Push()
    {
      var a = Value.Memory(3);
      var b = Value.Memory(5);

      a.Push(b, SinkToSourceMergeStrategy.Default);
      Assert.AreEqual(5, a.Value);
    }
    [TestMethod]
    public void Pull()
    {
      var a = Value.Memory("a string");
      var b = Value.Memory("b string");
      a.Pull(b, SinkToSourceMergeStrategy.Default);
      Assert.AreEqual("a string", b.Value);
    }



    [TestMethod]
    public void AbstractValueUnderlyingValueChangedEvents()
    {
      var uut = new MyValue();
      int valueChanged = 0;
      int valueConsumed = 0;
      int valueProduced = 0;
      uut.ValueChanged += (sender, args) => { valueChanged++; };
      uut.ValueConsumed += (sender, args) => { valueConsumed++; };
      uut.ValueProduced += (sender, args) => { valueProduced++; };

      uut.TheValue = "asd";
      Assert.AreEqual(1, valueChanged);
      Assert.AreEqual(0, valueConsumed);
      Assert.AreEqual(0, valueProduced);


    }

    [TestMethod]
    public void AbstractValueConsumeValueEvent()
    {
      var uut = new MyValue();
      int valueChanged = 0;
      int valueConsumed = 0;
      int valueProduced = 0;
      uut.ValueChanged += (sender, args) => { valueChanged++; };
      uut.ValueConsumed += (sender, args) => { valueConsumed++; };
      uut.ValueProduced += (sender, args) => { valueProduced++; };

      uut.Value = "hello";
      Assert.AreEqual(0, valueChanged);
      Assert.AreEqual(1, valueConsumed);
      Assert.AreEqual(0, valueProduced);


    }


    [TestMethod]
    public void AbstractValueProducedValueEvent()
    {
      var uut = new MyValue();
      int valueChanged = 0;
      int valueConsumed = 0;
      int valueProduced = 0;
      uut.ValueChanged += (sender, args) => { valueChanged++; };
      uut.ValueConsumed += (sender, args) => { valueConsumed++; };
      uut.ValueProduced += (sender, args) => { valueProduced++; };

      var val = uut.Value;

      Assert.AreEqual(0, valueChanged);
      Assert.AreEqual(0, valueConsumed);
      Assert.AreEqual(1, valueProduced);
    }




  }






  class MyValue : AbstractValue
  {
    public MyValue() : base(true, true, null, false) { }
    public object obj;
    internal object TheValue
    {
      get
      {
        return obj;
      }
      set
      {
        obj = value;
        NotifyValueChanged(obj, value);
      }
    }
    protected override void ConsumeValue(object value)
    {

    }

    protected override object ProduceValue()
    {
      return TheValue;
    }
  }



  class MyConnectableObjectValue : AbstractConnectableObjectValue
  {
    MyConnectableObjectValue() : base(Core.Values.ValueInfo.MakeDefault()) { }

    protected override void ConsumeValue(object value)
    {
      throw new NotImplementedException();
    }

    protected override object ProduceValue()
    {
      throw new NotImplementedException();
    }
  }

}
