using System;
using System.IO;
using Newtonsoft.Json;
using Core.Serialization.Json;

namespace Core.Modules.Messaging.Crypto
{
  /// <summary>
  /// 
  /// </summary>
  public static class CryptoMessageExtensions
  {
    public static string CreateCryptoMessage(object @object)
    {
      StringWriter stringWriter = new StringWriter();
      JsonTextWriter writer = new JsonTextWriter(stringWriter);
      Serializer.Serialize(writer, @object);
      var result = Cryptography.Encrypt(stringWriter.ToString());
      return result;
    }
    public static object DecryptCrytpoMessage(string encrypted)
    {
      var result = Serializer.Deserialize(new JsonTextReader(new StringReader(Cryptography.Decrypt(encrypted))));
      return result;
    }

    /// <summary>
    /// creates 
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public static string CreateAndEscapeCryptoMessage(object message)
    {
      var encrypted = CreateCryptoMessage(message);
      return Uri.EscapeDataString(encrypted);

    }



    static CryptoMessageExtensions()
    {
      Serializer = new JsonSerializer();
      Serializer.TypeNameHandling = TypeNameHandling.Objects;
      Serializer.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
      Serializer.TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Full;
      Serializer.MissingMemberHandling = MissingMemberHandling.Ignore;
      Serializer.NullValueHandling = NullValueHandling.Ignore;
      Serializer.Binder = new GuidBinder();
    }

    public static JsonSerializer Serializer { get; set; }

  }
}