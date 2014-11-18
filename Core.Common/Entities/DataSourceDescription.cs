using Core.Repositories;
using System;

namespace Core.Modules.Entities
{
  public class DataSourceDescription : DataDescription
  {
    public DataSourceDescription(Type type) :base(type)
    {
      
    }

    public DataSourceDescription() { }
    public Type DataSourceType { get; set; }


  }
}
