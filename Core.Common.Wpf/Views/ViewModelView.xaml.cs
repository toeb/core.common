using Core.Common.MVVM;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Core.Common.Wpf.Views
{
  /// <summary>
  /// Interaction logic for ViewModelView.xaml
  /// </summary>
  /// 
  public partial class ViewModelView : UserControl
  {
    public ViewModelView()
    {
      InitializeComponent();
      Loaded += ViewLoaded;
      DataContextChanged += OnDataContextChanged;
    }

    public void UpdateView()
    {
      var ancestores = this.Ancestors<FrameworkElement>().ToArray();
      var frameworkElememt= this.Ancestors<FrameworkElement>().FirstOrDefault(fe => fe.Tag is IViewProvider);
      if (frameworkElememt == null) return;// throw new Exception("missing viewprovider in a parent");
      var viewManager = frameworkElememt.Tag as IViewProvider;
      var viewModel = DataContext as IViewModel;
      object view = null;
      if (viewModel != null)
      {
        view = viewManager.CreateView(viewModel);
        if (view == null) view = new Label() { Content = "No View Found for " + viewModel.GetType(), HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center, VerticalContentAlignment = System.Windows.VerticalAlignment.Center };

      }
      else
      {
        view = new Label() { Content = "No Viewmodel Selected" };
      }

      try
      {
        this.Content = view;
      }
      catch (Exception e)
      {

      }
    }

    private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      UpdateView();
    }
    private void ViewLoaded(object sender, RoutedEventArgs e)
    {
      UpdateView();
    }





  }

}
