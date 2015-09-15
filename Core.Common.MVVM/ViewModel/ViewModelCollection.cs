using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Input;
using Core.Common.Reflect;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using Core.Common.Collections;

namespace Core.Common.MVVM
{
  public class ViewModelCollection<TViewModel, TModel> : ViewModelBase, ICollection<TViewModel>, INotifyCollectionChanged
    where TViewModel : IViewModel
  {
    private ICollection<TModel> modelCollection;
  
  
    public ViewModelCollection()
    {
      modelCollection = null;

    }
  
    private void AddModel(object mode)
    {
  
    }
    private bool ContainsModel(object model) { return false; }
    private bool RemoveModel(object mode) { return false; }
    private IEnumerable Models() { return modelCollection; }
  
  
    public void Add(TViewModel item)
    {
      AddModel(item.Model);
    }
  
    public void Clear()
    {
      foreach (var item in this.ToArray())
      {
        Remove(item);
      }
    }
  
    public bool Contains(TViewModel item)
    {
      return ContainsModel(item.Model);
    }
  
    public void CopyTo(TViewModel[] array, int arrayIndex)
    {
      int index = arrayIndex;
      foreach (var item in this)
      {
        array[index++] = item;
      }
    }
  
    public int Count
    {
      get { return modelCollection.Count; }
    }
  
    public bool IsReadOnly
    {
      get { return modelCollection.IsReadOnly; }
    }
  
    public bool Remove(TViewModel item)
    {
      return RemoveModel(item.Model);
    }
  
    public IEnumerator<TViewModel> GetEnumerator()
    {
      foreach (var model in Models())
      {
        yield return (TViewModel)this.RequireChild(model, typeof(TViewModel), this.Contract);
      }
    }
  
    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }
  
    public event NotifyCollectionChangedEventHandler CollectionChanged;
  
    public string Contract { get; set; }

    public override void OnAfterConstruction()
    {
    }

    public override void OnDispose()
    {
    }
  }
}
