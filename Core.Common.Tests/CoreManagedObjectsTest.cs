
using Core.Extensions;
using Core.Graph.Directed;
using Core.Values;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using Core;
using System.Reflection;
using Core.Graph;
using System.Linq.Expressions;
using System.Diagnostics.Contracts;
using System.ComponentModel.Composition;
using Core.Merge;
namespace Core.ManagedObjects.Test
{


  class MyManagedObject : ManagedObjectBase
  {
    public MyManagedObject() : base(ManagedObjectInfo.MakeDefault()) { }
    public IManagedProperty RequireProperty(string name, Type type)
    {
      return RequireProperty(new ManagedPropertyInfo(name, true, true, type, false), new Lazy<object>(() => null));
    }
    public new bool RemoveProperty(string name)
    {
      return base.RemoveProperty(name);
    }
  }

  [TestClass]
  public class CoreManagedObjectsTest
  {

    [TestMethod]
    public void CreateManagedProperty()
    {
      var uut = ManagedProperty.Memory("test", 6);
      Assert.AreEqual(6, uut.Value);
      Assert.IsTrue(uut.CanProduce());
      Assert.IsTrue(uut.CanConsume(5));
      Assert.IsTrue(uut.PropertyInfo.IsReadable);
      Assert.IsTrue(uut.PropertyInfo.IsWriteable);
      Assert.AreEqual("test", uut.PropertyInfo.Name);
      Assert.IsFalse(uut.PropertyInfo.OnlyExactType);
      Assert.AreEqual(typeof(int), uut.PropertyInfo.ValueType);
      uut.Value = 88;
      Assert.AreEqual(88, uut.Value);

    }

    [TestMethod]
    public void CreateManagedObject()
    {
      var uut = new MyManagedObject();
      Assert.AreEqual(0, uut.Connectors.Count());
      Assert.AreEqual(0, uut.Edges.Count());
      Assert.AreEqual(0, uut.Properties.Count());

    }
    [TestMethod]
    public void AddProperty()
    {
      var uut = new MyManagedObject();
      var changedProps = new List<string>();
      uut.PropertyChanged += (sender, args) => changedProps.Add(args.PropertyName);

      var property = uut.RequireProperty("hello", typeof(object));
      Assert.AreEqual(1, uut.Properties.Count());
      Assert.IsTrue(changedProps.Contains("Properties"));
      // check that property is added to successors
      var successor = (uut as IConnectable).Successors().OfType<IManagedProperty>().Single();
      Assert.AreEqual(property, successor);
    }

    [TestMethod]
    public void RemoveProperty()
    {
      var uut = new MyManagedObject();
      var changedProps = new List<string>();
      var property = uut.RequireProperty("hello", typeof(int));
      uut.PropertyChanged += (sender, args) => changedProps.Add(args.PropertyName); ;
      var result = uut.RemoveProperty("hello");
      Assert.IsTrue(result);
      Assert.IsFalse(uut.Connectors.Contains(property));
      Assert.IsFalse(uut.Properties.Contains(property));
      Assert.IsFalse(uut.HasProperty("hello"));
    }


    [TestMethod]
    [Timeout(10000)]
    public void AddManyProperties()
    {
      var uut = new MyManagedObject();
      int n = 100000;
      for (int i = 0; i < n; i++)
      {
        uut.RequireProperty("prop" + i, typeof(object));
      }
    }
    class MyClass
    {
      public string Prop { get; set; }
    }
    [TestMethod]
    public void ReflectedManagedProperty()
    {
      var obj = new MyClass();
      obj.Prop = "muuh";
      var prop = obj.PropertyInfoFor(o => o.Prop);
      var uut = new ReflectedManagedProperty(obj, new ReflectedManagedPropertyInfo(prop));
      Assert.AreEqual("muuh", uut.Value);
      uut.Value = "kuul";
      Assert.AreEqual("kuul", obj.Prop);


    }
    [TestMethod]
    public void ReflectedManagedObjectAnonymous()
    {
      var obj = new { PropertyA = "muh", PropertyB = 23 };
      var uut = new ReflectedManagedObject(obj, obj.GetType().GetProperties());
      Assert.AreEqual(2, uut.Properties.Count());
      Assert.AreEqual(2, uut.Properties.Count());
      Assert.IsTrue(uut.GetProperty("PropertyA").PropertyInfo.IsReadable);
      Assert.IsFalse(uut.GetProperty("PropertyA").PropertyInfo.IsWriteable);
      Assert.IsFalse(uut.GetProperty("PropertyA").PropertyInfo.OnlyExactType);
      Assert.AreEqual("muh", uut["PropertyA"]);
      Assert.AreEqual(23, uut["PropertyB"]);
    }

    [TestMethod]
    public void ReflectedNotifyingManagedObject()
    {
      var obj = new MyClass2();
      var uut = new ReflectedManagedNotifyingObject(obj, typeof(MyClass2).GetProperties());

      bool called = false;
      uut.GetProperty("Prop").ValueChanged += (sender, args) => called = true;
      obj.Prop = "lol";
      Assert.IsTrue(called);
      Assert.AreEqual("lol", uut["Prop"]);
      called = false;
      uut["Prop"] = "naaa";
      Assert.IsTrue(called);
      Assert.AreEqual("naaa", obj.Prop);
    }

    class MyClass2 : NotifyPropertyChangedBase
    {
      private string prop;
      public string Prop { get { return prop; } set { ChangeIfDifferent(ref prop, value); } }
    }
    [TestMethod]
    public void PropertyChangingObjectMangedPropertyUpdates()
    {
      var obj = new MyClass2();
      obj.Prop = "lol";
      var uut = new ReflectedNotifyingManagedProperty(obj, obj.PropertyInfoFor(o => o.Prop));
      var events = new List<object>();
      obj.PropertyChanged += (sender, args) => { events.Add(args); };
      uut.ValueChanged += (sender, args) => { events.Add(args); };
      uut.PropertyChanged += (sender, args) => { events.Add(args); };
      obj.Prop = "fuu";
      Assert.IsTrue(events.OfType<PropertyChangedEventArgs>().Select(p => p.PropertyName).ContainsAll("Prop", "Value"));
      Assert.IsTrue(events.OfType<ValueChangeEventArgs>().Count() == 1);
    }



    class C
    {

    }
    class A : C
    {

    }
    class B : C
    {
    }
   


    [TestMethod]
    public void PushAssignableManagedObject()
    {
      IAssignableManagedObject uut = new AssignableManagedObject();
      uut.PushProperty(new { propA = "valA" }, i => i.propA, new MyMergeStrategy());
      Assert.AreEqual("valA", uut["propA"]);
      var info = uut.GetProperty("propA").PropertyInfo;
      Assert.AreEqual(typeof(string), info.ValueType);
      Assert.AreEqual("propA", info.Name);
      Assert.AreEqual(false, info.OnlyExactType);
      Assert.IsTrue(info.IsWriteable);
      Assert.IsTrue(info.IsReadable);
    }

    [TestMethod]
    public void PushAddManagedObject()
    {

      IAssignableManagedObject uut = new AssignableManagedObject();
      uut.PushProperty(new { propA = "valA" }, i => i.propA, new MyMergeStrategy());
      uut.PushProperty(new { propB = 25 }, i => i.propB, new MyMergeStrategy());

      Assert.AreEqual("valA", uut["propA"]);
      Assert.AreEqual(25, uut["propB"]);
    }
    [TestMethod]
    public void PushOverrideSameTypeManagedObject()
    {
      IAssignableManagedObject uut = new AssignableManagedObject();
      uut.PushProperty(new { propA = "valA" }, i => i.propA, new MyMergeStrategy());
      uut.PushProperty(new { propA = "valB" }, i => i.propA, new MyMergeStrategy());

      Assert.AreEqual("valB", uut["propA"]);

    }
    [TestMethod]
    public void PushOverrideDifferentTypeButCompatible()
    {
      IAssignableManagedObject uut = new AssignableManagedObject();
      uut.PushProperty(new { propA = new C() }, i => i.propA, new MyMergeStrategy());
      uut.PushProperty(new { propA = new B() }, i => i.propA, new MyMergeStrategy());
    }
    [TestMethod]
    public void MergeSameTypes()
    {
      var uut = new MyMergeStrategy();
      var src = Source.Delegate(() => "stringeling");
      string result = "";
      var snk = Sink.Delegate<string>(str => result = str);
      Assert.IsTrue(uut.CanMerge(src, snk));
      uut.Merge(src, snk);
      Assert.AreEqual("stringeling", result);
    }

    [TestMethod]
    public void MergeIntoBaseType()
    {
      var uut = new MyMergeStrategy();
      var b = new B();
      var src = Source.Delegate(() => b);
      C result = null;
      var snk = Sink.Delegate<C>(c => result = c);
      Assert.IsTrue(uut.CanMerge(src, snk));
      uut.Merge(src, snk);
      Assert.IsTrue(object.ReferenceEquals(b, result));

    }

    [TestMethod]
    public void MergeIntoCommonAncestor()
    {
      var uut = new MyMergeStrategy();
      var b = new B();
      var src = Source.Delegate(() => b);
      var snk = Value.Memory<object>(new A(), true, true, typeof(A), false);
      Assert.IsTrue(uut.CanMerge(src, snk));
      Assert.AreEqual(typeof(A), snk.ValueInfo.ValueType);
      uut.Merge(src, snk);
      Assert.IsTrue(object.ReferenceEquals(b, snk.Value));
      Assert.AreEqual(typeof(C), snk.ValueInfo.ValueType);
    }

    [TestMethod]
    public void AssignWholeObject()
    {
      var obj = new { a = "muh", b = 1 };
      var uut = new AssignableManagedObject();
      uut.PushPublicProperties(obj, new MyMergeStrategy());
      Assert.IsTrue(uut.HasProperty("a"));
      Assert.IsTrue(uut.HasProperty("b"));
    }

    [TestMethod]
    public void PullExact()
    {
      var uut = new AssignableManagedObject();
      uut.PushPublicProperties(new { a = "bb", b = 2323 }, SinkToSourceMergeStrategy.Default);
      int res = 0;
      uut.PullProperty(new DelegateManagedProperty<int>("b", null, (r) => res = r), SinkToSourceMergeStrategy.Default);
      Assert.AreEqual(2323, res);
    }
    [TestMethod]
    public void PullAssignable()
    {
      var uut = new AssignableManagedObject();
      uut.PushPublicProperties(new { a = "bb", b = 2323 }, SinkToSourceMergeStrategy.Default);
      object res = 0;
      uut.PullProperty(new DelegateManagedProperty<object>("b", null, (r) => res = r), SinkToSourceMergeStrategy.Default);
      Assert.AreEqual(2323, res);
    }


    [TestMethod]
    [Timeout(10000)]
    public void TestManyPushs()
    {

      int n = (int)1e6;//10mio pushs
      IAssignableManagedObject uut = new AssignableManagedObject();
      int i = 0;
      var prop = new DelegateManagedProperty<int>("test", () => i, null);
      for (i = 0; i < n; i++)
      {
        uut.PushProperty(prop, new MyMergeStrategy());
      }
    }

    [TestMethod]
    [Timeout(5000)]
    public void TestManyObjectPushs()
    {
      var obj = new { a = "aaaaa", b = "33333", c = 23232, d = new { a = 2323 } };
      int n = (int)1e5;

      IAssignableManagedObject uut = new AssignableManagedObject();
      for (int i = 0; i < n; i++)
      {
        uut.PushPublicProperties(obj, SinkToSourceMergeStrategy.ExactType);
      }
    }
  }



  class MyMergeStrategy : CompositeMergeStrategy
  {
    public MyMergeStrategy()
    {

      Strategies = new[] { 
        new ExactTypeMergeStrategy(),
        new AssignableMergeStrategy(),
        new CommonAncestorMergeStrategy()
      };

    }
  }

}
