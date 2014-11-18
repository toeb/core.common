using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Core.Initialization
{

  public interface IInitializerOrderMetaData
  {
    [DefaultValue(Int32.MaxValue)]
    int Order { get; }
  }

}
