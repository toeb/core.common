
using Core.TestingUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Core.Store.KeyValue.Tests
{
  [TestClass]
  public class CoreStoreKeyValueTest : TypedTestBase<IKeyValueStore<Guid, object>>
  {
    protected override IKeyValueStore<Guid, object> CreateUut()
    {
      return GetUut<JsonMemoryKeyValueStore<Guid, object>>();
     
    }
    [TestMethod]
    public void CreateStore()
    {
      Assert.IsNotNull(uut);
      Assert.IsFalse(uut.Keys.Any());
    }

    [TestMethod]
    public void StoreSimpleObject()
    {
      uut.Store(Guid.NewGuid(), "hello");
      var it = uut.Values.Single();
      Assert.AreEqual(it, "hello");
    }
    [TestMethod]
    public void OnlyReferenceStored()
    {
      //simple new object
      var obj = new { };
      uut.Store(Guid.NewGuid(), obj);
      var result = uut.Values.Single();
      Assert.IsFalse(object.ReferenceEquals(obj, result));
    }
    [TestMethod]
    public void LoadValueType()
    {
      var id = uut.Store(Guid.NewGuid(), "hello");
      var result = uut.Load(id);
      Assert.AreEqual("hello", result);
    }
    [TestMethod]
    public void LoadSimpleReferenceType()
    {
      // references may not be equal since input object might be in cointnued use
      var obj = new { hiho = "worldo" };
      var id = uut.Store(Guid.NewGuid(), obj);
      var result = uut.Load(id);
      Assert.IsFalse(object.ReferenceEquals(result, obj));
      dynamic r = result;
    }
    class OutClass { public InnerClass Prop { get; set; } public string PropC { get; set; } }
    class InnerClass { public string PropA { get; set; } public string PropB { get; set; } }
    [TestMethod]
    public void LoadBoxedType()
    {
      var obj = new OutClass() { Prop = new InnerClass() { PropA = "hihi", PropB = "bubu" }, PropC = "Naaa" };

      var id = uut.Store(Guid.NewGuid(), obj);
      var result = uut.Load(id);
      Assert.IsNotNull(result);
      Assert.AreEqual(typeof(OutClass), result.GetType());
      var typedResult = result as OutClass;
      Assert.IsNotNull(typedResult);

      Assert.IsFalse(object.ReferenceEquals(obj, result));
      Assert.IsFalse(object.ReferenceEquals(obj.Prop, typedResult.Prop));
    }

    [TestMethod]
    public void LoadTestWrite()
    {
      for (int i = 0; i < 100000; i++)
      {
        uut.Store(Guid.NewGuid(), new { obj = new { obj = new { id = 2323232 }, test = 232323 }, lol = "asdasd" });
      }

    }

    class A
    {
      public int MyProperty { get; set; }
      public string lol { get; set; }
      public A refA { get; set; }
      public object prop3 { get; set; }
    }

    [TestMethod]
    public void LoadTestReadWrite()
    {
      for (int i = 0; i < 100000; i++)
      {
        uut.Store(Guid.NewGuid(), new A(){lol="asdasd", refA=new A(){lol="asdasdasdaaaaaaaaaaaaaaaaa", MyProperty=i},MyProperty=232323});
        uut.Store(Guid.NewGuid(), new { obj = new { obj = new { id = 2323232 }, test = i }, lol = "asdasd" });
      }
      var result = uut.Values.OfType<A>().Where(a=>a.refA.MyProperty%10==0).ToArray();
    }
  }
}
