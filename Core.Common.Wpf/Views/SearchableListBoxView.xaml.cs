using Core.Common.Wpf.ViewModels;
using System.Collections;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Core.Common.Wpf.Views
{
  /// <summary>
  /// Interaction logic for UserControl1.xaml
  /// </summary>
  public partial class SearchableListBoxView : UserControl, INotifyPropertyChanged
  {
    public SearchableListBoxView()
    {
      InitializeComponent();
    }

    SearchableCollectionViewModel viewModel = null;
    public SearchableCollectionViewModel ViewModel
    {
      get
      {
        return viewModel;
      }
      set
      {
        if (viewModel == value) return;
        viewModel = value;
        if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("ViewModel"));
      }
    }

    public static DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(object), typeof(SearchableListBoxView), new PropertyMetadata(new PropertyChangedCallback(SelectedItemChanged)));

    public object SelectedItem
    {
      get { return GetValue(SelectedItemProperty); }
      set { SetValue(SelectedItemProperty, value); }
      
    }

    public static DependencyProperty ShowSearchProperty = DependencyProperty.Register(
      "ShowSearch", 
      typeof(bool), 
      typeof(SearchableListBoxView), 
      new PropertyMetadata(true, new PropertyChangedCallback(ShowSearchChanged)));

    public bool ShowSearch
    {
      get { return (bool)GetValue(ShowSearchProperty); }
      set { SetValue(ShowSearchProperty, value); }

    }

    private static void ShowSearchChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {

    }

    private static void SelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      var @this = d as SearchableListBoxView;
    }

    private void RaisePropertyChanged(string p)
    {
      if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(p));
    }

    public static DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(object), typeof(SearchableListBoxView), new PropertyMetadata(new PropertyChangedCallback(OnItemsSourceChanged)));

    private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      var searchableListBox = d as SearchableListBoxView;
      var vm = new SearchableCollectionViewModel();
      vm.Items = e.NewValue as IEnumerable;
      searchableListBox.ViewModel = vm;
    }

    public object ItemsSource
    {
      get { return GetValue(ItemsSourceProperty); }
      set { SetValue(ItemsSourceProperty, value); }
    }





    public event PropertyChangedEventHandler PropertyChanged;
  }
}
