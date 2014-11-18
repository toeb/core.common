using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.TestingUtilities
{
  /// <summary>
  ///  a base clase for testing a specific type of object
  ///  automatically creates  a uut property which is of specified type T
  ///  and first tries to create it via injection, then via default constructor
  ///  this behaviour can also be overriden in a subclass
  ///  the unit under test is created freshly before every test execution
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public class TypedTestBase<T> : TestBase
  {
    [TestInitialize]
    public void InitUnitUnderTest()
    {
      uut = CreateUut();
    }
    /// <summary>
    /// overridable method for creating the unit under test
    /// </summary>
    /// <returns></returns>
    protected virtual T CreateUut()
    {
      try
      {
        return GetUut<T>();
      }
      catch
      {
        try
        {
          return System.Activator.CreateInstance<T>();
        }
        catch { return default(T); }
      }
    }
    /// <summary>
    /// the unit under test
    /// </summary>
    protected T uut { get; set; }
  }
}
