using Core.Merge;
using Core.Values;
using System;

namespace Core.ManagedObjects
{
  public class AssignableManagedObject : ManagedObjectBase, IAssignableManagedObject, IExtensibleManagedObject
  {
    public AssignableManagedObject(ManagedObjectInfo info) : base(info) { }
    public AssignableManagedObject()
      : base(ManagedObjectInfo.MakeDefault())
    {

    }
    public void PushProperty(string property, ISource source, IMergeStrategy strategy){
    
      var sink = this.GetPropertyOrNull(property);
      if (sink == null)
      {
        ManagedPropertyInfo info = new ManagedPropertyInfo(property,true,true,source.ConnectorInfo.ValueType, false);
        sink = RequireProperty(info, new Lazy<object>(() => source.Value));
      }
      Merge(source,sink,strategy);
    }
    public void PullProperty(string property, ISink sink, IMergeStrategy strategy){
    
      var source = this.GetPropertyOrNull(property);
      if (source == null) return ;
      Merge(source,sink,strategy);
      return ;
    }
    public void PushProperty(IManagedProperty source, IMergeStrategy strategy)
    {
      var name = source.PropertyInfo.Name;
      PushProperty(name,source,strategy);
    }

    protected virtual void Merge(ISource source,ISink sink,IMergeStrategy strategy)
    {
      if (!strategy.CanMerge(source, sink)) throw new PropertyMergeException();
      strategy.Merge(source, sink);
    }
    public void PullProperty(IManagedProperty sink, IMergeStrategy strategy)
    {
      var name = sink.PropertyInfo.Name;
      PullProperty(name,sink,strategy);
    }
    #region Extension Points
    protected virtual void OnPushing( ISource source, IManagedProperty sink, IMergeStrategy strategy) { }
    protected virtual void OnPushed(ISource source, IManagedProperty sink, IMergeStrategy strategy) { }
    protected virtual void OnPulling(IManagedProperty source, ISink sink, IMergeStrategy strategy) { }
    protected virtual void OnPulled(IManagedProperty source, ISink sink, IMergeStrategy strategy) { }

    #endregion



    #region extensible managed object
    public new IManagedProperty RequireProperty(IPropertyInfo info, Lazy<object> initialValue)
    {
      return base.RequireProperty(info, initialValue);
    }

    public new bool RemoveProperty(IManagedProperty property)
    {
      return base.RemoveProperty(property);
    }

    #endregion

  }
}
