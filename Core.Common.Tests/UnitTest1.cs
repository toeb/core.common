using Core.Common.Crypto;
using Core.Common.Data;
using Core.Common.Reflect;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
namespace Core.Common.Tests
{

  public enum Gender
  {
    Male,
    Female
  }
  [Serializable]
  [DebuggerDisplay("Person {FirstName}")]
  public class Person
  {
    public Person()
    {
      Children = new List<Person>();
    }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public Gender Gender { get; set; }
    public Person Father { get; set; }
    public Person Mother { get; set; }
    public List<Person> Children { get; set; }
  }

  [TestClass]
  public class UnitTest1
  {
    [Serializable]
    public class MyValue
    {
      public string A { get; set; }
      public string B { get; set; }
    }
    [TestMethod]
    public void ObjectHash1()
    {


      Assert.AreEqual(Reflect.Reflection.ObjectHash(1234), Reflect.Reflection.ObjectHash(1234));
      Assert.AreEqual(Reflect.Reflection.ObjectHash(new { a = 1, b = 2 }), Reflect.Reflection.ObjectHash(new { a = 1, b = 2 })); // same type

      var hash1 = new MyValue { A = "a", B = "b" }.ComputeHash();
      var hash2 = new MyValue { A = "a", B = "b" }.ComputeHash();
      var hash3 = new MyValue { A = "a", B = "c" }.ComputeHash();

      Assert.AreEqual(hash1, hash2);
      Assert.AreNotEqual(hash1, hash3);

    }



    [TestMethod]
    public void TestMethod1()
    {
      var uut = new SimpleDataContext();



      var t = new Person() { FirstName = "Tobias" };
      var l = new Person() { FirstName = "Lena" };

      var r = new Person() { FirstName = "Rosemarie" };
      var p = new Person() { FirstName = "Paul" };


      p.Children.Add(t);
      p.Children.Add(l);

      r.Children.Add(t);
      r.Children.Add(l);


      t.Mother = r;
      t.Father = p;

      l.Mother = r;
      l.Father = p;




      uut.Entry(t).State = EntityState.Attached;

      //uut.Set<Person>().Add(t);

      var successors = Reflect.Reflection.Bfs(t).ToArray();



      Assert.AreEqual(4, uut.Entries.Count());


      uut.Save();

      Assert.IsTrue(uut.Entries.All(e => e.State == EntityState.Unmodified));



    }

    [TestMethod]
    public void RoundTripSimple()
    {
      var uut = new JsonDataContext();

      uut.Set<Person>().Add(new Person() { FirstName = "Tobi" });
      uut.Save();

      Assert.AreEqual(1, uut.Set<Person>().Count);

      var uut2 = new JsonDataContext();
      uut2.Refresh();

      Assert.IsTrue(uut2.Set<Person>().Count() == 1);
      var e1 = uut.Entries.Single();
      var e2 = uut2.Entries.Single();

      var v1 = e2.Value;

      uut2.Refresh();


      Assert.IsTrue(object.ReferenceEquals(v1, e2.Value));

      uut2.Refresh();
      Assert.IsTrue(uut2.Entries.Count() == 1);
      Assert.IsFalse(object.ReferenceEquals(e1.Value, e2.Value));
      Assert.AreEqual(e1.Hash, e2.Hash);
      Assert.AreEqual(e1.Id, e2.Id);
    }

    [TestMethod]
    public void RoundTripWithRelation()
    {
      var uut1 = new JsonDataContext();
      var people = uut1.Set<Person>();

      var p1 = new Person() { FirstName = "Tobi" };
      var p2 = new Person() { FirstName = "Paul" };

      p1.Father = p2;
      p2.Children.Add(p1);


      people.Add(p1);

      Assert.AreEqual(2, people.Count);


      uut1.Save();


      var uut2 = new JsonDataContext();


      uut2.Refresh();

      Assert.AreEqual(people.Count(), uut2.Set<Person>().Count());
    }

    [TestMethod]
    public void Many()
    {
      var uut = new JsonDataContext();
      for (int i = 0; i < 1000; i++)
      {
        uut.Set<Person>().Add(new Person() { FirstName = "T" + i, Children = { new Person() { FirstName = "C1" + i }, new Person() { FirstName = "C2" + i } } });

      }

      uut.Save();


      uut = new JsonDataContext();


      uut.Refresh();
    }
  }
  public class JsonDataContext : SerializingDataContext
  {
    protected override string Serialize(IEnumerable<IEntry> entries)
    {
      return JsonConvert.SerializeObject(entries, Formatting.Indented, settings);
    }

    protected override IEnumerable<SimpleEntry> Deserialize(string txt)
    {
      var result = JsonConvert.DeserializeObject<SimpleEntry[]>(txt, settings);
      foreach (var entry in result)
      {
        entry.Id = Guid.Parse(entry.Id as string);
      }
      return result;
    }
    JsonSerializerSettings settings = new JsonSerializerSettings()
    {
      PreserveReferencesHandling = PreserveReferencesHandling.Objects,
      TypeNameHandling = TypeNameHandling.Objects,
      ReferenceLoopHandling = ReferenceLoopHandling.Serialize
    };
  }

  public enum RecordState
  {
    New,
    NeedsUpDate,
    UpToDate,
    Modified,
    NeedsMerge,
    Detached
  }
  //}
  //public class RecordState
  //{

  //}
  //public class NewState { }
  //public class NeedsMergeState { }
  //public class UpToDateState { }
  //public class ModifiedRecordState { }
  //public class DetachedRecordState :ModifiedRecordState{ }
  //public class NeedsUpdateState :RecordState{ }

  public class Record : IDisposable
  {
    internal Record(RecordSet records, object entity)
    {
      if (entity == null) throw new ArgumentNullException("entity");
      if (entity is ValueType) throw new ArgumentException("entity");
      this.records = records;
      State = RecordState.Detached;
      this.id = null;
      this.entity = entity;
    }
    private object id;
    private object entity;
    private RecordSet records;
    private RecordState state;
    public object Id
    {
      get { return id; }
      set
      {
        if (id != null) throw new InvalidOperationException();
        id = value;
        records.recordsById[id] = this;
      }
    }

    public RecordState State { get { return state; } set { state = records.transition(this, value); } }
    public object Entity { get { return entity; } }


    public void Dispose()
    {
    }
  }
  public class RecordSet
  {
    public RecordSet(Func<Record, RecordState, RecordState> transition)
    {
      this.transition = transition;
    }
    private ISet<Record> records = new HashSet<Record>();
    internal Dictionary<object, Record> recordsById = new Dictionary<object, Record>();
    internal Dictionary<object, Record> recordsByAddress = new Dictionary<object, Record>();


    public Record GetById(object id)
    {
      if (id == null) return null;
      lock (this)
      {
        Record record;
        if (!recordsById.TryGetValue(id, out record)) return default(Record);
        return record;
      }
    }
    public Record GetByEntity(object entity)
    {
      if (entity == null) return null;
      lock (this)
      {
        Record record;
        if (!recordsByAddress.TryGetValue(entity, out record)) return default(Record);
        return record;
      }
    }
    public Record this[object entity]
    {
      get
      {
        if (entity == null) return null;
        lock (this)
        {
          Record record;
          if (!recordsByAddress.TryGetValue(entity, out record))
          {
            record = new Record(this, entity);
            records.Add(record);
            recordsByAddress[entity] = record;
          }
          return record;
        }
      }
    }
    public IEnumerable<Record> Records { get { return records; } }



    internal Func<Record, RecordState, RecordState> transition;
  }

  [TestClass]
  public class RecordTests
  {
    [TestMethod]
    public void Test1()
    {
      var uut = new RecordSet((record, state) => state);

      var o1 = new object();
      var o2 = new object();
      var o3 = new object();


      var r1 = uut[o1];

      Assert.IsNotNull(r1);


      Assert.AreEqual(1, uut.Records.Count());

      var r12 = uut[o1];

      Assert.IsTrue(object.ReferenceEquals(r12, r1));
      Assert.AreEqual(1, uut.Records.Count());

      var r2 = uut[o2];

      Assert.AreEqual(2, uut.Records.Count());

      var r3 = uut[o3];
    }
  }


  public class DataOperations
  {
    public void Attach(object entity)
    {


      // if exists in database and data is older/invalid:
      //entry.State = Entity.NeedsUpdate;



      // if exist in database but data is newer
      //entry.State = EntityState.New;

      // if exists in database and data is different.
      //entry.State = EntityState.NeedsMerge
    }

    public void Detach(object entity)
    {

    }
    public void Invalidate(object entity) // or id
    {


      // if modified => needsmerge

      // if up to date => needs update
    }

    public void Save(object entity)
    {
      // if not new, modified  faiure
    }

    public void Refresh(object entity)
    {
      // if not needs update failure


    }

    public void Update(object entity)
    {
      // if not up to date failure

      // state = Modified
    }

  }




}
