using Core.Values;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Core.ManagedObjects
{
  public interface IPropertyInfo : IValueInfo, IIdentifiable<string>
  {
    string Name { get; }
  }
}
