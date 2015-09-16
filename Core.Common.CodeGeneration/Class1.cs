using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Common.Reflect;
using Core.Graph;

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
    public static string BeginNamespaceCxx(this CodeGenerationContext context,  Type type)
    {
      var namespaces = type.Namespace.Split('.');
      var namespaceString = string.Join("\n", namespaces.Select(ns => "namespace " + ns + " {"));
      return namespaceString;
    }
    public static string FormatNamespaceCxx(this CodeGenerationContext context, Type type)
    {
      return type.Namespace.Replace(".", "::"); 
    }
    public static string EndNamespaceCxx (this CodeGenerationContext context, Type type)
    {
      var namespaces = type.Namespace.Split('.');
      var namespaceString = string.Join("\n", namespaces.Select(ns => "} // namespace "+ns));
      return namespaceString;
    }
    public static string FormatBaseClassesCxx(this CodeGenerationContext context, Type type)
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
  class CodeGenerationContext
  {
    public int IndentationLevel{ get; set; }
    public string CurrentNamespace { get; set; }




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
      var types = new[] { typeof(IMyType), typeof(Lol.IMyType2) }.TopSort(t=>t.GetDirectBaseTypes()).ToArray();
      tt1.Session["Types"] = types;
      
      tt1.Initialize();

      var text = tt1.TransformText();




    }
  }
}
