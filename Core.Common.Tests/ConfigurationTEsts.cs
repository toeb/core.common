using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Core.Extensions;
using System.ComponentModel;
using System.IO;
using Core.Configuration;
using Core.Annotations;
using Core.Values;
using Core.FileSystem;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using Core.Serialization;

namespace Core.Test
{


  public class TestConfigurationClass : NotifyPropertyChangedBase
  {
    public class Inner
    {
      public string A { get; set; }
    }


    private string testValue1;
    private int testValue2;
    private bool testValue3;
    private Inner inner;
    [DefaultValue("Something")]
    [Display(Name = "Test Value", Description = "The test value number 1")]
    public string TestValue1 { get { return testValue1; } set { ChangeIfDifferent(ref testValue1, value); } }
    public int TestValue2 { get { return testValue2; } set { ChangeIfDifferent(ref testValue2, value); } }
    public bool TestValue3 { get { return testValue3; } set { ChangeIfDifferent(ref testValue3, value); } }
    public Inner TestValue4 { get { return inner; } set { ChangeIfDifferent(ref inner, value); } }

    private ICollection<Inner> multi = new List<Inner>();
    public IEnumerable<Inner> MultipleValues { get { return multi; } set { ChangeCollectionByEnumerableValue(multi, value); } }
    private ISet<string> multiStrings = new HashSet<string>();
    public IEnumerable<string> MultipleStrings { get { return multiStrings; } set { ChangeSetByEnumerableValue(multiStrings, value); } }


    [ExtraData]
    public IDictionary<string, object> ExtraValues { get; set; }
  }


  [TestClass]
  public class CoreConfigurationTest
  {
    [TestInitialize]
    public void Init()
    {

    }

    public class Smpl
    {
      public Smpl smpl { get; set; }
      public Smpl smpl2 { get; set; }
      public string test { get; set; }
    }
    [TestMethod]
    public void DeepCopyTest()
    {

      var s = new Smpl();
      s.smpl = s;
      s.smpl2 = new Smpl() { smpl2 = s, smpl = new Smpl() { test = "nanana" }, test = "uiuiui" };
      s.test = "asd";

      var s2 = new Smpl();

      Reflection.DeepCopy(s, s2);


      Assert.IsTrue(object.ReferenceEquals(s2.smpl, s2));

    }
    [TestMethod]
    public void DeserializeExisting()
    {
      var uut = new TestConfigurationClass();
      uut.ExtraValues = new Dictionary<string, object>();
      uut.ExtraValues["test"] = "muuuuh";

      var ser = new JsonSerializer();
      ser.Converters.Add(new MappedConverter());
      var reader = new StringReader("{'hu':'mu'}");
      var me = new { inner = uut };
      ser.Populate(reader, me);

      Assert.AreEqual(2, uut.ExtraValues.Count);
    }

    [TestMethod]
    public void RoundTripMappedConvert()
    {
      var res = JsonConvert.DeserializeObject<TestConfigurationClass>(@"{ 
        'TestValue1':'muu', 
        'MultipleStrings':['a','b'],
        'MultipleValues':[{'A':'hallo'}],
        'b': [1,4,3,2],
        'c':[{'a':'b'},{}],
        'a': 'asd' 
        }", new MappedConverter());

      var resJson = JsonConvert.SerializeObject(res, Formatting.Indented, new MappedConverter());


      var res2 = JsonConvert.DeserializeObject<TestConfigurationClass>(resJson);

      var res2Josn = JsonConvert.SerializeObject(res, Formatting.Indented, new MappedConverter());

      Assert.AreEqual(res2Josn, resJson);

    }


    [TestMethod]
    public void MappedConvert()
    {
      var res = JsonConvert.DeserializeObject<TestConfigurationClass>(@"{ 
        'TestValue1':'muu', 
        'MultipleStrings':['a','b'],
        'MultipleValues':[{'A':'hallo'}],
        'b': [1,4,3,2],
        'c':[{'a':'b'},{}],
        'a': 'asd' 
        }", new MappedConverter());

      Assert.AreEqual("muu", res.TestValue1);
      Assert.AreEqual(3, res.ExtraValues.Count());

    }
    [TestMethod]
    public void Start()
    {

      var config = new TestConfigurationClass();
      var propertyNames = new List<string>();
      config.PropertyChanged += (sender, args) =>
      {
        propertyNames.Add(args.PropertyName);
      };
      config.TestValue1 = "asd";

      config.MultipleStrings = new[] { "a", "b" };


      var service = new ConfigurationService();



    }

    [TestMethod]
    public async Task ConfigureValue()
    {
      var uut = new Test();
      var service = new ConfigurationService();
      await service.ConfigureAsync("identifier", typeof(Test), uut);


    }

    class Test
    {
      [Configurable]
      [Display(Description = "enables feature 1")]
      [DefaultValue(false)]
      public bool Option1 { get; set; }
      [Configurable]
      [Display(Description = "enables feature 2")]
      [DefaultValue(true)]
      public bool Option2 { get; set; }
      [Configurable]
      [Display(Description = "a integer value ")]
      [DefaultValue(-1)]
      public int Value1 { get; set; }
    }

    [TestMethod]
    public void ShouldOnlySerializeConfigurableProperties()
    {
      var t = new Test();
      var result = JsonConvert.SerializeObject(t, Formatting.Indented, new ConfigurationJsonConverter());

    }


    [TestMethod]
    public void ShouldReloadConfigurationWhenChanged()
    {
      var t = new Test();
      t.Option1 = false;
      t.Option2 = true;

      var service = new ConfigurationService();
      service.WriteConfigurationAsync("ShouldReloadConfigurationWhenChanged", typeof(Test), t).Await();
      AutoResetEvent are = new AutoResetEvent(false);
      using (var watch = service.WatchConfiguration("ShouldReloadConfigurationWhenChanged", typeof(Test), t))
      {
        watch.ConfigurationChanged += sender => { if (t.Option1) { are.Set(); } };

        service.WriteConfigurationAsync("ShouldReloadConfigurationWhenChanged", typeof(Test), new Test { Option1 = true, Option2 = false }).Await();
        are.WaitOne(2000);

      }

      Assert.IsTrue(t.Option1);
      Assert.IsFalse(t.Option2);
    }

    [TestMethod]
    public async Task RandomConfigAccess()
    {
      var t = new Test();
      t.Option1 = false;
      t.Option2 = true;

      var service = new ConfigurationService();
      service.WriteConfigurationAsync("RandomConfigAccess", typeof(Test), t).Await();
      AutoResetEvent are = new AutoResetEvent(false);
      var list = new List<object>();
      var invocationList = new List<int>();
      var exceptions = new List<Exception>();
      using (var watch = service.WatchConfiguration("RandomConfigAccess", typeof(Test), t))
      {
        watch.ConfigurationChanged += sender => { list.Add(t.Value1); if (t.Value1 == 1) { are.Set(); } };

        await service.WriteConfigurationAsync("RandomConfigAccess", typeof(Test), new Test { Option1 = true, Option2 = false });

        await TestRandomAccessMultipleThread(async (id) =>
        {
          invocationList.Add(id);
          if (id == 1)
          {
            await Task.Delay(2000);
          }
          try
          {
            await service.WriteConfigurationAsync("RandomConfigAccess", typeof(Test), new Test() { Value1 = id });
          }
          catch (Exception e)
          {
            exceptions.Add(e);
          }
        });

        are.WaitOne(10000);

      }
      Assert.AreEqual(1, t.Value1);
    }


    public async Task TestRandomAccessMultipleThread(Func<int, Task> randomAccessAction, int threadCount = 15, int maxDelayMs = 1000)
    {

      var tasks = new List<Task>();

      Random r = new Random();

      for (int i = 0; i < threadCount; i++)
      {

        var task = Task.Factory.StartNew(async t =>
         {
           await Task.Delay(r.Next(0, maxDelayMs));
           await randomAccessAction((int)t);
         }, i);
        tasks.Add(task);

      }
      await Task.WhenAll(tasks.ToArray());
    }

  }


  public class ConfigurationService
  {

    JsonSerializer serializer;
    public ConfigurationService()
    {
      throw new NotImplementedException();
      FileSystem = new RelativeFileSystem("config");
      serializer = new JsonSerializer();
      serializer.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
      serializer.NullValueHandling = NullValueHandling.Ignore;
      serializer.MissingMemberHandling = MissingMemberHandling.Ignore;
      serializer.PreserveReferencesHandling = PreserveReferencesHandling.Objects;
      serializer.Converters.Add(new ConfigurationJsonConverter());
      serializer.Formatting = Formatting.Indented;
    }

    internal IRelativeFileSystem FileSystem { get; set; }

    internal string GetPath(string key, Type type)
    {
      var path = key + "/" + type.FullName + ".js";
      return path;
    }
    public async Task DeleteConfiguration(string key, Type type)
    {
      var path = GetPath(key, type);
      FileSystem.Delete(path);

    }
    public async Task<bool> ConfigurationExists(string key, Type type)
    {
      var path = GetPath(key, type);
      return FileSystem.IsFile(path);
    }

    public ConfigurationWatch WatchConfiguration(string key, Type type, object value)
    {
      return new ConfigurationWatch(key, type, value, this);
    }

    internal async Task ConfigureAsync(string key, Type type, object value)
    {
      var path = GetPath(key, type);
      if (!FileSystem.IsFile(path))
      {
        await WriteConfigurationAsync(key, type, value);
      }
      else
      {
        await ReadConfigAsync(key, type, value);
      }
    }

    public async Task ReadConfigAsync(string key, Type type, object value)
    {
      //using (await asyncLock.LockAsync())
      //{
      //  var path = GetPath(key, type);
      //  using (var reader = new StreamReader(FileSystem.OpenRead(path)))
      //  {
      //    var result = serializer.Deserialize(reader, type);
      //    Reflection.DeepCopy(result, value);
      //  }
      //}

    }
    //AsyncLock asyncLock = new AsyncLock();
    public async Task WriteConfigurationAsync(string key, Type type, object value)
    {
      //using (await asyncLock.LockAsync())
      //{
      //  var path = GetPath(key, type);
      //  FileSystem.EnsureFileExists(path);
      //  var stream = FileSystem.OpenFile(path, FileAccess.Write);
      //  using (var writer = new StreamWriter(stream))
      //  {
      //    serializer.Serialize(writer, value, type);
      //    await writer.FlushAsync();
      //  }
      //}

    }
  }

}
