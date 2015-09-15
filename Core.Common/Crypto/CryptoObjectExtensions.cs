using System;
using System.IO;
using Core.Common.Crypto;

namespace Core
{

  public interface ISerializer
  {
    void Serialize(StringWriter writer, object value);
    object Deserialize(StringReader reader);
  }
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
      Serializer.Serialize(stringWriter, @object);
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
      var result = Serializer.Deserialize(new StringReader(Cryptography.Decrypt(encryptedObject)));
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

    }

    public static ISerializer Serializer { get; set; }
    
  }
}