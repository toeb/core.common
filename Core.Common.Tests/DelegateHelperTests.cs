using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Core.Common.Tests
{
  [TestClass]
  public class DelegateHelperTests
  {

    [TestMethod]
    public void ShouldGetInstanceForInstanceMethodExpression()
    {
      var uut = MethodCallExpressionHelper.GetTarget(() => ShouldGetInstanceForInstanceMethodExpression());
      Assert.AreEqual(this, uut);
    }
    [TestMethod]
    public void ShouldNotGetInstanceForStaticCall()
    {
      var uut = MethodCallExpressionHelper.GetTarget(() => StaticNoArgsReturnAny());
      Assert.IsNull(uut);
    }

    public static void StaticNoArgsReturnVoid() { }
    public static string StaticNoArgsReturnAny() { return "hello"; }
    public static string StaticTwoArgsReturnAny(string a, string b) { return a + b; }
    [TestMethod]
    public void CreateDelegateStaticNoArgsReturnVoid()
    {
      var uut = MethodCallExpressionHelper.GetMethodCallDelegate(null, () => StaticNoArgsReturnVoid());
      Assert.IsNotNull(uut);
      Assert.AreEqual("StaticNoArgsReturnVoid", uut.Method.Name);
      var result = uut.DynamicInvoke();
      Assert.IsNull(result);
    }

    [TestMethod]
    public void CreateDelegateStaticNoArgsReturnAny()
    {
      var uut = MethodCallExpressionHelper.GetMethodCallDelegate(null, () => StaticNoArgsReturnAny());
      Assert.AreEqual("StaticNoArgsReturnAny", uut.Method.Name);
      Assert.AreEqual("hello", uut.DynamicInvoke());
    }

    [TestMethod]
    public void CreateDelegateStaticTwoArgsReturnAny()
    {
      var uut = MethodCallExpressionHelper.GetMethodCallDelegate(null, () => StaticTwoArgsReturnAny(null, null));
      Assert.AreEqual("StaticTwoArgsReturnAny", uut.Method.Name);
      Assert.AreEqual("abcdef", uut.DynamicInvoke("abc", "def"));
    }
    private string instanceField;
    public void InstanceNoArgsReturnVoid() { instanceField = "InstanceNoArgsReturnVoid"; }
    [TestMethod]
    public void CreateDelegateInstanceNoArgsReturnAny()
    {
      var uut = this.GetMethodCallDelegate(() => InstanceNoArgsReturnVoid());
      Assert.IsNotNull(uut);
      Assert.AreEqual("InstanceNoArgsReturnVoid", uut.Method.Name);
      Assert.AreEqual(null, uut.DynamicInvoke());
      Assert.AreEqual("InstanceNoArgsReturnVoid", instanceField);
    }


    public string InstanceTwoArgsReturnAny(string a, string b)
    {
      return a + instanceField + b;
    }

    [TestMethod]
    public void CreateDelegateInstanceTwoArgsReturnAny()
    {
      var uut = this.GetMethodCallDelegate(() => InstanceTwoArgsReturnAny(null, null));
      Assert.IsNotNull(uut);
      instanceField = "hello";

      Assert.AreEqual(InstanceTwoArgsReturnAny("a", "b"), uut.DynamicInvoke("a", "b"));
    }

  }
}