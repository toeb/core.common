using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Core.Common.Serialization
{

  public interface ISerializer
  {
    void SerializeToStream(Stream stream, object @object);
    object DeserializeFromStream(Stream stream, Type targetType);
  }
  public interface ISerializationService
  {

  }

  public static class DefaultSerializer
  {

  }

}
