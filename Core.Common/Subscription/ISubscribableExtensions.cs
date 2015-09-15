using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Core.Common.Reflect;
namespace Core
{
  public static class ISubscribableExtensions
  {
    public static void Subscribe<TModel, TValue>(this TModel self, Expression<Func<TModel, TValue>> expression, ValueChangeDelegate del)
      where TModel:ISubscribable
    {
      self.Subscribe(self.PropertyInfoFor(expression).Name, del);
    }
    public static void Subscribe<TModel, TValue>(this ISubscribable self, Expression<Func<TModel, TValue>> expression, ValueChangeDelegate del)
    {
      var name = expression.PropertyInfoFor().Name;
      self.Subscribe(name, del);
    }
    public static void Unsubscribe<TModel, TValue>(this ISubscribable self, Expression<Func<TModel, TValue>> expression, ValueChangeDelegate del)
    {
      var name = expression.PropertyInfoFor().Name;
      self.Unsubscribe(name, del);
    }



    public static ValueChangeDelegate SubscribeChanged<TModel, TValue>(this TModel self, Expression<Func<TModel, TValue>> expression, OnValueChangeDelegate<TValue> del)
      where TModel : ISubscribable
    {

      var name = expression.PropertyInfoFor().Name;
      ValueChangeDelegate wrapped = (sender, args) =>
      {

        if (args.ChangeState != ChangeState.Changed) return;
        if (!args.NewValueAvailable) return;
        if (!args.NewValueAvailable) return;
        del((TValue)args.OldValue, (TValue)args.NewValue);
      };

      self.Subscribe(name, wrapped);
      return wrapped;
    }
    /// <summary>
    /// registers a notification delegate for the specified property.
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="self"></param>
    /// <param name="expression"></param>
    /// <param name="del"></param>
    public static ValueChangeDelegate SubscribeOnChange<TModel, TValue>(this TModel self, Expression<Func<TModel, TValue>> expression, NotifyDelegate del)
      where TModel : ISubscribable
    {
      var name = expression.PropertyInfoFor().Name;
      ValueChangeDelegate wrapped = (sender, args) =>
      {
        if (args.ChangeState != ChangeState.Changed) return;
        del();
      };
      //dict[del] = wrapped;
      self.Subscribe(name, wrapped);
      return wrapped;
    }

  }
}
