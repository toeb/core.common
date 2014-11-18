using System;
using System.IO;
using Newtonsoft.Json;
using Core.Serialization.Json;

namespace Core
{
  /// <summary>
  /// 
  /// </summary>
  public static class CryptoObject
  {
    /// <summary>
    /// encrypts an object to a strnig
    /// </summary>
    /// <param name="object"></param>
    /// <returns></returns>
    public static string Encrypt(object @object)
    {
      StringWriter stringWriter = new StringWriter();
      JsonTextWriter writer = new JsonTextWriter(stringWriter);
      Serializer.Serialize(writer, @object);
      var result = Cryptography.Encrypt(stringWriter.ToString());
      return result;
    }

    /// <summary>
    /// decrypts an object
    /// </summary>
    /// <param name="encryptedObject"></param>
    /// <returns></returns>
    public static object Decrypt(string encryptedObject)
    {
      var result = Serializer.Deserialize(new JsonTextReader(new StringReader(Cryptography.Decrypt(encryptedObject))));
      return result;
    }

    /// <summary>
    /// encrypts an object and excapes the resulting string
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public static string EncryptAndEscape(object @object)
    {
      var encrypted = Encrypt(@object);
      return Uri.EscapeDataString(encrypted);

    }



    static CryptoObject()
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