using Core.Common.MVVM;
using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Windows.Data;

namespace Core.Common.Wpf.ViewModels
{
  [ViewModel(typeof(SearchableCollectionViewModel), ModelType = typeof(ICollection))]
  [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.NonShared)]
  public class SearchableCollectionViewModel : ViewModelBase
  {
    /// <summary> The view source </summary>
    private CollectionViewSource viewSource;

    /// <summary> Gets source for the. </summary>
    ///
    /// <value> The source. </value>
    public CollectionViewSource Source { get { return viewSource; } }

    /// <summary> Gets or sets the filter function. </summary>
    ///
    /// <value> The filter function. </value>
    public Func<object, string, bool> FilterFunction
    {
      get
      {
        return Get<Func<object, string, bool>>("FilterFunction");
      }
      set
      {
        Set(value, "FilterFunction");
      }
    }

    /// <summary> Gets or sets the clear search string command. </summary>
    ///
    /// <value> The clear search string command. </value>
    public void ClearSearchString()
    {
      SearchString = "";
    }

    /// <summary> Default constructor. </summary>
    ///
    /// <remarks> Tobi, 30.03.2012. </remarks>
    public SearchableCollectionViewModel()
    {
      viewSource = new CollectionViewSource();
      FilterFunction = SearchExtensions.SearchPredicate;

    }

    private void ModelChanged()
    {
      viewSource.Source = Items;
    }

    private void UpdateViewSource()
    {
      if (viewSource.View == null) return;
      if (viewSource.View.Filter == null) viewSource.View.Filter = FilterView;
      viewSource.View.Refresh();


    }

    private bool FilterView(object obj)
    {
      if (FilterFunction == null) return true;
      if (string.IsNullOrEmpty(SearchString)) return true;
      return FilterFunction(obj, SearchString);
    }


    /// <summary> Gets or sets the items. </summary>
    ///
    /// <value> The items. </value>
    [DependsOn("Model")]
    public IEnumerable Items
    {
      get
      {
        return Model as IEnumerable;
      }
      set
      {
        Model = value;
      }
    }

    /// <summary> Gets the view. </summary>
    ///
    /// <value> The view. </value>
    public ICollectionView View
    {
      get
      {
        return viewSource.View;
      }
    }

    /// <summary> Gets or sets the search string. </summary>
    ///
    /// <value> The search string. </value>
    public string SearchString
    {
      get
      {
        return Get<string>("SearchString");
      }
      set
      {
        Set(value, "SearchString");
      }
    }

    public override void OnAfterConstruction()
    {

        this.Subscribe(m => m.FilterFunction, UpdateViewSource);
        this.Subscribe(m => m.Items, UpdateViewSource);
        this.Subscribe(m => m.SearchString, UpdateViewSource);
        this.Subscribe(m => m.Model, ModelChanged);
    }

    public override void OnDispose()
    {
    }
  }
}
