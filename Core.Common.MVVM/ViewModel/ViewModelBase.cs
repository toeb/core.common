
using Core.Common.MVVM.ViewModel;
using Core.Common.Reflect;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace Core.Common.MVVM
{

    public class ITypeService
    {
        [Import]
        IReflectionService ReflectionService { get; set; }

        
    }
    public class IInternatializationService
    {
        [Import]
        ITypeService TypeService { get; set; }

        public string Translate(string uri, object item) { return "no translation"; }
    }

    /// <summary>
    /// Base class for basic view models.
    /// 
    /// Properties which are of type colleciton are automatically injected with Observable Collections when Get is called
    /// Commands marked with CommandAttribute are automatically added to CommandList
    /// 
    /// </summary>
    public abstract class ViewModelBase : BasePropertyObject,
        IViewModel,
        IViewManager,
        ISearchable,
        IPartImportsSatisfiedNotification
    {
        
        private PropertySetterDelegate innerSetter;
        private PropertyGetterDelegate innerGetter;
        private CommandProvider commandProvider;
        private ResourceProvider resourceProvider;
        private IDispatcher dispatcher;


        [Import]
        public CommandProvider Commands { get { return commandProvider; } set { commandProvider = value; value.Context = this; } }



        [Import]
        public ResourceProvider Resources
        {
            get { return resourceProvider; }
            set { resourceProvider = value; resourceProvider.Context = this; }
        }


        [Import]
        public IViewModelProvider ViewModelProvider { get; set; }


        [Import]
        public IViewManager ViewManager { get; set; }


        [Import]
        public IDispatcher Dispatcher
        {
            get { return dispatcher; }
            set
            {
                this.dispatcher = value;
                setter = new DispatchingSetter(innerSetter, value);
            }
        }

        public void Dispatch(Action action)
        {
            Dispatcher.Dispatch(action);
        }

        protected MessageViewModel NotifyUser(string title, string message)
        {
            var vm = ViewModelProvider.Require<MessageViewModel>();
            vm.Title = title;
            vm.Message = message;
            ViewManager.ShowView(vm);
            return vm;
        }





        private SubscriptionProvider subscriptions;
        [Import]
        SubscriptionProvider Subscriptions
        {
            get { return subscriptions; }
            set
            {
                subscriptions = value;
                if (value != null) { value.Context = this; }
            }
        }
        
        public Subscription Subscribe(string propertyName, Action<object> action)
        {
            return subscriptions.Subscribe(propertyName, action);
        }



        void RegisterDependees()
        {
            var properties = GetType().PropertiesWith<DependsOnAttribute>();
            foreach (var property in properties)
            {
                var info = property.Item1;
                foreach (var attr in info.GetCustomAttributes<DependsOnAttribute>())
                {
                    Subscribe(attr.PropertyName, val => RaisePropertyChanged(info.Name));
                }
            }
        }

        protected virtual IViewHandle ShowCustomView(object viewModel)
        {
            return null;
        }
        public IViewHandle ShowView(object viewModel)
        {
            var customView = ShowCustomView(viewModel);
            if (customView != null) return customView;
            return ViewManager.ShowView(viewModel);
        }


        public ViewModelBase()
        {

            innerSetter = setter;
            innerGetter = getter;
            getter = new ObservableCollectionGetter(getter, setter);
            dispose = DoDispose;
        }

        private void DoDispose(object @object)
        {
            OnDispose();
        }

        public abstract void OnAfterConstruction();
        public abstract void OnDispose();

        public void OnImportsSatisfied()
        {
            RegisterDependees();
            this.Subscribe(m => m.Model, OnModelChanged);
            OnAfterConstruction();
        }

        protected virtual void OnModelChanged(object @object) { }


        public virtual object Model
        {
            get
            {
                return Get<object>();
            }
            set
            {
                Set(value);
            }
        }



        private IDictionary<Tuple<object, Type, string>, IViewModel> children = new Dictionary<Tuple<object, Type, string>, IViewModel>();

        public T GetChild<T>(object value, string viewModelContract = null) where T : class,IViewModel
        {
            return GetChild(value, typeof(T), viewModelContract) as T;
        }
        public T RequireChild<T>(object value = null, string viewModelContract = null) where T : class,IViewModel
        {
            return RequireChild(value, typeof(T), viewModelContract) as T;
        }
        public IViewModel RequireChild(object value, Type viewModelType = null, string viewModelContract = null)
        {

            lock (children)
            {
                var key = Tuple.Create(value, viewModelType, viewModelContract);
                if (children.ContainsKey(key)) return children[key];
                var viewModel = CreateChild(value, viewModelType, viewModelContract);
                children[key] = viewModel;
                return viewModel;
            }
        }
        public IViewModel GetChild(object value, Type viewModelType = null, string viewModelContract = null)
        {
            lock (children)
            {
                var key = Tuple.Create(value, viewModelType, viewModelContract);
                if (!children.ContainsKey(key)) return null;
                return children[key];
            }
        }
        public IViewModel CreateChild(object value, Type viewModelType = null, string viewModelContract = null)
        {
            var modelType = value == null ? null : value.GetType();
            // find viewmodel by comparing metadata to input
            var viewModel = ViewModelProvider.RequireViewModel(metadata =>
            {
                if (modelType != null && modelType == metadata.ModelType) return true;
                if (viewModelType != null && viewModelType == metadata.ViewModelType) return true;
                if (!string.IsNullOrEmpty(viewModelContract) && viewModelContract == metadata.Contract) return true;
                return false;
            }) as IViewModel;

            // setup connections
            viewModel.Model = value;
            viewModel.Parent = this;

            return viewModel;
        }


        public virtual IViewModel Parent
        {
            get;
            set;
        }

        public virtual IEnumerable<SearchTerm> SearchTerms
        {
            get
            {
                var searchable = Model as ISearchable;
                if (searchable == null) return new SearchTerm[0];
                return searchable.SearchTerms;
            }
        }

        public ViewHandle ViewHandle { get; set; }
    }


}