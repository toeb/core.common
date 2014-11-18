using System;
namespace Core.TypeServices
{

  public interface ITypeService
  {
    ObjectType GetType(Guid id);
    bool HasType(Guid id);
    ObjectType RequireType(Guid id);
    ObjectType RequireType(Type type);
    ObjectType RequireType<T>();
    
    System.Collections.Generic.IEnumerable<ObjectType> Types { get; }
  }
}
