using System;
using System.Linq.Expressions;
using Core.Common.Reflect;
namespace Core.Common.MVVM
{
  public static class ViewModelBaseMixins
  {
    public static void Subscribe< TModel,TMember>(this TModel self, Expression<Func<TModel,TMember>> expression, Action<TMember> action)
      where TModel : ViewModelBase
    {
      var memberInfo = self.MemberInfoFor(expression);
      self.Subscribe(memberInfo.Name, obj =>
      {
        action((TMember)obj);
      });
      
    }
  
    public static void Subscribe<TModel, TMember>(this TModel self, Expression<Func<TModel, TMember>> expression, Action action)
      where TModel : ViewModelBase
    {
  
      self.Subscribe(expression, m => action());
    }
  
  }
}
