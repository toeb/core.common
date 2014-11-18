using System;
using Core.Repositories;
using System.Collections.Generic;
using Core.Store.KeyValue.Implementation;
using Core.Store.KeyValue;
using System.Linq;
using Core.Identification;
using Core.Identifiers;
using Core.Modules.Rules;

namespace Core.Modules.Entities
{
  class EntityService : IEntityService
  {
    public EntityService(IRulesService rulesService)
    {
      this.RulesService = rulesService;
      Sources = new Dictionary<string, IDataSource>();
      Sinks = new Dictionary<string, IDataSink>();

    }
    IDictionary<string, IDataSource> Sources { get; set; }
    IDictionary<string, IDataSink> Sinks { get; set; }


    bool HasDataSink(string contract)
    {
      return Sinks.ContainsKey(contract);
    }
    IDataSink GetDataSink(string contract)
    {
      if (!HasDataSink(contract)) return null;
      return Sinks[contract];
    }
    internal IDataSink RequireDataSink(DataSinkDescription description)
    {
      if (HasDataSink(description.Contract)) return GetDataSink(description.Contract);
      var sink = ConstructDataSink(description);
      //Sinks[contract] = sink;
      //DataSinkAdded(sink);
      return sink;
    }

    private void DataSinkAdded(IDataSink sink)
    {
    }

    private IDataSink ConstructDataSink(DataSinkDescription description)
    {
      var repo = ConstructRepository(description);
      if (!description.DataSinkType.IsAssignableFromValue(repo)) throw new Exception("Could not create a repository matching the data descritpion");
      return repo;
    }

    private IRepository ConstructRepository(DataDescription description)
    {
      var type = description.RequiredType;
      var contract = description.Contract;

      var storeType = typeof(CloningMemoryKeyValueStore<,>);
      var keyProperty = type.GetProperty("Id");
      var keyType = keyProperty.PropertyType;

      object comparer;
      object idProvider;


      if (keyType == null)
      {
        throw new ArgumentException("expecting type to have property  'Guid|int Id'");
      }
      if (keyType == typeof(int))
      {
        comparer = new IntEqualityComparer();
        idProvider = new IntIdentifierProvider();
      }
      else if (keyType == typeof(Guid))
      {
        comparer = new GuidEqualityComparer();
        idProvider = new GuidIdentifierProvider();
      }
      else
      {
        throw new ArgumentException("expecting type to have either an int or guid id");
      }

      var concreteStoreType = storeType.MakeGenericType(keyType, type);
      var storeConstructor = concreteStoreType.GetConstructor(Type.EmptyTypes);
      var store = storeConstructor.Invoke(new object[0]);
      var accessorType = typeof(PropertyIdentityAccessor<,>);
      var concreateAccessorType = accessorType.MakeGenericType(type, keyType);
      var accessorConstructor = concreateAccessorType.GetConstructors().FirstOrDefault();
      var idAccessor = accessorConstructor.Invoke(new object[] { keyProperty });



      var repositoryType = typeof(KeyValueRepository<,>);
      var concreteRepositoryType = repositoryType.MakeGenericType(keyType, type);
      var b = concreteRepositoryType.ContainsGenericParameters;
      var repositoryConstructor = concreteRepositoryType.GetConstructors().FirstOrDefault();
      var repository = repositoryConstructor.Invoke(new object[] { store, idProvider, idAccessor, comparer });
      var castRepository = repository as IRepository;

      Sources[contract] = castRepository;
      Sinks[contract] = castRepository;
      DataSourceAdded(castRepository);
      DataSinkAdded(castRepository);
      return castRepository;
    }

    bool HasDataSource(string contract) { return Sources.ContainsKey(contract); }
    IDataSource GetDataSource(string contract) { if (!HasDataSource(contract))return null; return Sources[contract]; }
    void DataSourceAdded(IDataSource source)
    {

    }
    internal IDataSource RequireDataSource(DataSourceDescription description)
    {
      if (HasDataSource(description.Contract)) return GetDataSource(description.Contract);
      var datasource = ConstructDataSource(description);
      //Sources[contract] = datasource;
      return datasource;
    }


    private IDataSource ConstructDataSource(DataSourceDescription description)
    {
      var repo = ConstructRepository(description);
      if (!description.DataSourceType.IsAssignableFromValue(repo)) throw new Exception("could create data source because the required sourcetype is not available");
      return repo;
    }

    public IEntityContext GetContext(EntityContextDescription description)
    {
      if (description != EntityContextDescription.Default) throw new ArgumentException("this entity service only allows the default context");
      var context = new EntityContext(this, description);
      var rulesDataContext = new RulesDataContext(context) { RulesService = RulesService };


      return rulesDataContext;

    }

    internal void ReleaseContext(EntityContext entityContext)
    {

    }


    public void SetDataSource(DataSourceDescription description, IDataSource dataSource)
    {
      Sources[description.Contract] = dataSource;
    }

    public void SetDataSink(DataSinkDescription description, IDataSink dataSink)
    {
      Sinks[description.Contract] = dataSink;
    }

    public IRulesService RulesService { get; set; }
  }
}
