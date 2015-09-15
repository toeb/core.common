using Core.Common.Collections;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Core.Common.Tests
{
  public static class MethodCallExpressionHelper
  {

    public static object GetTarget(this Expression<Action> expr)
    {
      var lambda = expr as LambdaExpression;
      if (lambda == null) return null;
      var methodCall = lambda.Body as MethodCallExpression;
      if (methodCall== null) return null;
      var constant = methodCall.Object as ConstantExpression;
      if (constant== null) return null;
      var instance = constant.Value;
      return instance;
    }

    public static MethodInfo GetMethodInfo(this Expression<Action> expr)
    {
      var call = expr.Body as MethodCallExpression;
      if (call == null) return null;
      return call.Method;
    }
    public static MethodInfo GetMethodCallFromExpression<T>(this Expression<Action<T>> expr)
    {
      var call = expr.Body as MethodCallExpression;

      if (call == null) return null;
      return call.Method;
    }

    public static Delegate GetMethodCallDelegate(this object @object, Expression<Action> expr)
    {
      var method = expr.GetMethodInfo();
      var types = method.GetParameters().Select(m => m.ParameterType).Concat(method.ReturnType).ToArray();
      var delegateType = Expression.GetDelegateType(types);

      Delegate result;
      if (method.IsStatic)
      {
        result = Delegate.CreateDelegate(delegateType, method);
      }
      else
      {
        result = Delegate.CreateDelegate(delegateType, @object, method);
      }

      return result;
    }

    public static Delegate InformUser(this object @object, string request, Expression<Action> expr)
    {
      return @object.GetMethodCallDelegate(expr);
    }



  }
}