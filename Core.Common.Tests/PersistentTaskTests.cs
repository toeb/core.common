using Core.Common.Reflect;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System;
using System.Reflection;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using Core.Common.Collections;
using System.IO;
using Core.Common.Crypto;

namespace Core.Common.Tests
{

  public class Hel
  {
    static Hel()
    {

        CryptoObject.Serializer = new NewtonsoftJsonSerializer();
    }

  }

 

  public class MethodCall
  {
    private Delegate @delegate;
    public MethodCall()
    {

    }

    public object Target { get; set; }
    public MethodInfo Method { get; set; }


    public static MethodCall FromExpression(Expression<Action> expression)
    {
      var target = expression.GetTarget();
      var instance = expression.GetMethodInfo();




      return new MethodCall();

    }

  }
  public static class Helpers
  {

  }
  public static class Bind
  {

  }

  [TestClass]
  public class MethodCallTests
  {
    [TestMethod]
    public void MethodCallCreate()
    {
      var uut = MethodCall.FromExpression(() => InstanceTwoArgsReturnString(new Bind<string>("asda"), "constant", new Bind<string>("output")));

    }

    private void InstanceTwoArgsReturnString(string bind1, string v, string bind2)
    {
      throw new NotImplementedException();
    }

    public string InstanceTwoArgsReturnTwoStrings(string lhs, string rhs, out string res1)
    {
      res1 = "output";
      return lhs + rhs;
    }

  }


  public class Bind<T>
  {
    public Bind(string contract) { }
    public static implicit operator T(Bind<T> output)
    {
      return default(T);
    }

  }

  class MyTaskContext
  {
    int value = 0;
  }

  [TestClass]
  public class PersistentTaskTests
  {
    private string instanceField = null;
    private static string staticField = null;
    [TestMethod]
    [TestCategory("Persistent")]
    public void ShouldPerfomNullOpStatic()
    {
      staticField = null;
      var uut = new PersistentTaskRunner();

      var result = uut.Run(() => DoNothing());

      Assert.AreEqual("hello", staticField);
    }

    public static void DoNothing() { staticField = "hello"; }


    [TestMethod]
    public void ShouldRunLambdaTask()
    {
      staticField = null;
      var uut = new PersistentTaskRunner();
      var result = uut.Run(() => Lala());
      Assert.AreEqual("1", staticField);
      Assert.AreEqual(null, result);
    }
    private static void Lala()
    {
      staticField = "1";
    }
    [TestMethod]
    public void ShouldReturnResult()
    {
      staticField = null;
      instanceField = "hiho";
      var uut = new PersistentTaskRunner();
      var result = uut.Run(() => ReturnSomthing());
      Assert.AreEqual("hiho", result);
    }
    string ReturnSomthing()
    {
      return instanceField;
    }

    [TestMethod]
    void ShouldUseParameters()
    {
      var uut = new PersistentTaskRunner();
      var result = uut.Run(() => StaticTwoArgsReturnAny(Arg.Create<string>("somthing"), "hello"));

    }
    private static string StaticTwoArgsReturnAny(string lhs, string rhs)
    {
      return lhs + rhs;
    }

  }
  public static class Helpers1
  {
    public static string QualifiedParameterName(this ParameterInfo info)
    {
      return info.Member.DeclaringType.FullName + "." + info.Member.Name + "(" + info.Name + ")";
    }
  }
  public class Arg
  {
    public static Arg<T> Create<T>(string name)
    {
      return new Arg<T>(name);
    }
  }
  public class Arg<T>
  {
    public string Name { get; set; }
    public Arg(string inputName)
    {
      Name = inputName;
    }
    public static implicit operator T(Arg<T> arg)
    {
      return default(T);
    }
  }
  public class PersistentTaskRunner
  {

    private Dictionary<string, object> inputvalues = new Dictionary<string, object>();
    public bool HasParameter(string key)
    {
      lock (inputvalues)
      {
        return inputvalues.ContainsKey(key);
      }
    }
    public void Provide(string key, object value)
    {
      lock (inputvalues)
      {
        inputvalues[key] = value;
      }
    }

    public void Provide(Expression<Action> call, string parameter, object value)
    {
      var param = call.GetMethodInfo().GetParameters().FirstOrDefault(p => p.Name == parameter);
      var paramName = param.QualifiedParameterName();

      Provide(paramName, value);
    }

    private object[] GetParameters(MethodInfo method)
    {
      lock (inputvalues)
      {
        var parameters = method.GetParameters().Select(p => p.QualifiedParameterName()).ToArray();
        if (parameters.Any(p => !inputvalues.ContainsKey(p)))
        {
          return null;
        }
        var values = parameters.Select(p => inputvalues[p]).ToArray();
        foreach (var p in parameters)
        {
          inputvalues.Remove(p);
        }
        return values;
      }
    }
    private void SetParameters(MethodInfo method, object[] parameters)
    {
      lock (inputvalues)
      {

        var parameterInfos = method.GetParameters();
        for (int i = 0; i < parameterInfos.Length; i++)
        {
          var parameterInfo = parameterInfos[i];

          if (parameterInfo.IsOut)
          {
            var parameterName = parameterInfo.QualifiedParameterName();
            inputvalues[parameterName] = parameters[i];
          }
        }
      }

    }
    public object Run(Expression<Action> start)
    {
      var method = start.GetMethodInfo();
      var instance = start.GetTarget();
      var del = instance.GetMethodCallDelegate(start);


      var parameters = Import(method);


      var result = del.DynamicInvoke(parameters);


      Export(method, result, parameters);


      return result;

    }

    private void Export(MethodInfo method, object result, object parameters)
    {
    }

    private object Import(MethodInfo method)
    {
      throw new NotImplementedException();
    }
  }
  public class RegisterPersistentTask
  {


    private void InvokeDelegate(Delegate del1)
    {

    }

    private Delegate GetStartMetod()
    {
      var @delegate = this.GetMethodCallDelegate(() => Start());
      return @delegate;
    }
    public Delegate Start()
    {
      return this.InformUser("Please enter a username and a password!", () => OnUserNameAndPasswordEntered(null, null));
    }
    public void Provide(string name, object value)
    {

    }

    public void Test() { }
    public Delegate OnUserNameAndPasswordEntered(string userName, string password)
    {
      return null;
    }



  }








}