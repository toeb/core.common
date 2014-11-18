using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Modules.Rules
{
  public class RuleContext : IRuleContext
  {
    private Dictionary<string, object> data;
    public RuleContext()
    {
      data = new Dictionary<string, object>();
    }
    public RuleContext(IDictionary<string, object> initData)
    {
      this.data = new Dictionary<string, object>();
      Replace(initData);
    }
    public void Replace(IDictionary<string, object> values)
    {
      foreach (var value in values)
      {
        data[value.Key] = value.Value;
      }
    }
    public void Replace(object @object)
    {
      Replace(Anonymous.ToDictionary(@object));
    }

    public string Action
    {
      get
      {
        return this.Get("Action") as string;
      }
      set
      {
        Data["Action"] = value;
      }
    }

    public object Resource
    {
      get
      {
        return this.Get("Resource");
      }
      set
      {
        Data["Resource"] = value;
      }
    }


    public string ExtensionPoint
    {
      get { return this.Get<string>("ExtensionPoint"); }
      set { Data["ExtensionPoint"] = value; }
    }

    public IDictionary<string, object> Data
    {
      get
      {
        return data;
      }
      set
      {
        data.Clear();
        foreach (var v in value)
        {
          data[v.Key] = v.Value;
        }
      }
    }
  }
}
