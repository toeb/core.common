using System;
using System.IO;
using Core.Common.Crypto;
using System.Security.Cryptography.X509Certificates;

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
    public static string Encrypt(object @object, X509Certificate2 certificate)
    {
      StringWriter stringWriter = new StringWriter();
      Serializer.Serialize(stringWriter, @object);
      var result = Cryptography.Encrypt(certificate, stringWriter.ToString());
      return result;
    }

    /// <summary>
    /// decrypts an object
    /// </summary>
    /// <param name="encryptedObject"></param>
    /// <returns></returns>
    public static object Decrypt(string encryptedObject, X509Certificate2 certificate)
    {
      var result = Serializer.Deserialize(new StringReader(Cryptography.Decrypt(certificate, encryptedObject)));
      return result;
    }

    /// <summary>
    /// encrypts an object and excapes the resulting string
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public static string EncryptAndEscape(object @object, X509Certificate2 certificate)
    {
      var encrypted = Encrypt(@object,certificate);
      return Uri.EscapeDataString(encrypted);

    }


    public static ISerializer Serializer { get; set; }
    
  }
}