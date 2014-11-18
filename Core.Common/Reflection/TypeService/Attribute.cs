using Core.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Core.TypeServices
{
  public class Attribute{


    public Attribute(ObjectType attributeType, System.Attribute attr)
    {
      // TODO: Complete member initialization


      Properties = Core.Anonymous.ToDictionary(attr,true);
      Name = attr.GetType().Name;
      FullName = attr.GetType().FullName;
    }

    public string Name { get; set; }
    public string FullName { get; set; }

    public IDictionary<string, object> Properties { get; set; }

  }
}
