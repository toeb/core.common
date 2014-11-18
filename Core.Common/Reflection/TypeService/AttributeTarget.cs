using Core.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Core.TypeServices
{
  public class AttributeTarget
  {
    public AttributeTarget() { AllAttributes = new Attribute[0]; Attributes = new Dictionary<string, Attribute>(); }
    public IEnumerable<Attribute> AllAttributes { get; set; }
    public IDictionary<string, Attribute> Attributes { get; set; }
  }
}
