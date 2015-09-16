using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Common.Reflect;
namespace Core.Common.CodeGeneration
{
  public interface IMyType
  {

  }
  namespace Lol
  {
    public interface IMyType2  : IMyType3
    {

    }
    public interface IMyType3 : IMyType4
    {

    }
    public interface IMyType4 { }
  }

  public static class TemplateHelpers
  {
    public static string BeginNamespaceCxx(this Type type)
    {
      var namespaces = type.Namespace.Split('.');
      var namespaceString = string.Join("\n", namespaces.Select(ns => "namespace " + ns + " {"));
      return namespaceString;
    }
    public static string FormatNamespaceCxx(this Type type)
    {
      return type.Namespace.Replace(".", "::"); 
    }
    public static string EndNamespaceCxx (this Type type)
    {
      var namespaces = type.Namespace.Split('.');
      var namespaceString = string.Join("\n", namespaces.Select(ns => "} // namespace "+ns));
      return namespaceString;
    }
    public static string FormatBaseClassesCxx(this Type type)
    {

      var baseTypes = type.GetDirectBaseTypes();
      baseTypes = baseTypes.OrderByDescending(t=>t.IsInterface?0:1);
      if (baseTypes.Count() == 0) return "";
      var names = baseTypes.Select(t => t.FormatNamespaceCxx() + "::" + t.Name);
      if (baseTypes.Count() == 1) return " : " + names.First();
      var result = string.Join(",\n", names);
      return result;
    }


    


  }

  [TestClass]
  public class Class1
  {
    [TestMethod]
    [TestCategory("IDL")]
    public void Testit()
    {
      cxx tt1 = new cxx();
      tt1.Session = new Dictionary<string, object>();
      tt1.Session["Types"] = new[] { typeof(IMyType), typeof(Lol.IMyType2) };
      tt1.Initialize();

      var text = tt1.TransformText();




    }
  }
}
