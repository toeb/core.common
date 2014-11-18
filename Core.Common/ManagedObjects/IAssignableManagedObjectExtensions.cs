using Core.Merge;
using Core.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;


namespace Core.ManagedObjects
{
  public static class IAssignableManagedObjectExtensions
  {
    public static void PushPublicProperties(this IAssignableManagedObject self, object @object, IMergeStrategy strategy)
    {
      self.PushPublicProperties(@object, @object.GetType(), strategy);
    }
    public static void PushPublicProperties(this IAssignableManagedObject self, object @object, Type type,IMergeStrategy strategy)
    {
      self.PushProperties(@object, type, prop => prop.CanRead && prop.GetMethod.IsPublic, strategy);
    }
    public static void PushAllProperties(this IAssignableManagedObject self, object @object, Type type, IMergeStrategy strategy)
    {
      self.PushProperties(@object, type, prop => prop.CanRead, strategy);
    }
    public static void PushAnnotatedProperties(this IAssignableManagedObject self, object @object, Type type, IMergeStrategy strategy)
    {
      self.PushProperties(@object, type.GetManageableProperties(), strategy);
    }

    public static void PushProperties(this IAssignableManagedObject self, object @object, Type type, Func<PropertyInfo, bool> predicate, IMergeStrategy strategy)
    {
      self.PushProperties(@object, type.GetProperties().Where(predicate), strategy);
    }
    public static void PushProperties(this  IAssignableManagedObject self, object @object, IEnumerable<PropertyInfo> properties, IMergeStrategy strategy)
    {
      var obj = new ReflectedManagedObject(@object, properties);
      self.PushProperties(obj, strategy);
    }
    public static void PushProperties(this IAssignableManagedObject self, IManagedObject @object, IMergeStrategy strategy)
    {
      foreach (var property in @object.Properties)
      {
        self.PushProperty(property, strategy);
      }
    }
    public static void PushProperty<TModel, TValue>(this IAssignableManagedObject self, TModel instance, Expression<Func<TModel, TValue>> propertyExpression, IMergeStrategy strategy)
    {
      self.PushProperty(instance, instance.PropertyInfoFor(propertyExpression), strategy);
    }

    public static void PushProperty<T>(this IAssignableManagedObject self, string name, T value, IMergeStrategy strategy=null)
    {
      if (strategy == null) strategy = SinkToSourceMergeStrategy.Assignable;
      self.PushProperty(value, name, typeof(T), strategy);
    }

    public static void PushProperty(this IAssignableManagedObject self, object instance, PropertyInfo info, IMergeStrategy strategy)
    {
      //ManagedProperty.Reflected(instance, info)
      var prop = new ReflectedManagedProperty(instance, info);
      self.PushProperty(prop, strategy);
    }
    public static void PushProperty(this IAssignableManagedObject self, object value, string propertyName, Type valueType, IMergeStrategy strategy)
    {
      var prop = new DelegateManagedProperty(propertyName, () => value, null, valueType);
      self.PushProperty(prop, strategy);
    }



  }
}
