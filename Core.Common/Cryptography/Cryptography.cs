using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Core
{


  public static class Cryptography
  {
    private const int TokenSizeInBytes = 16;
    private const int Pbkdf2Count = 1000;
    private const int Pbkdf2SubkeyLength = 256 / 8;
    private const int SaltSize = 128 / 8;

    /// <summary>
    /// returns a md5 hash
    /// @deprecated 
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static string GetMd5Hash(this string input)
    {
      // Convert the input string to a byte array and compute the hash.
      byte[] data = MD5.Create().ComputeHash(System.Text.Encoding.UTF8.GetBytes(input));

      // Create a new Stringbuilder to collect the bytes
      // and create a string.
      StringBuilder sBuilder = new StringBuilder();

      // Loop through each byte of the hashed data 
      // and format each one as a hexadecimal string.
      for (int i = 0; i < data.Length; i++)
      {
        sBuilder.Append(data[i].ToString("x2"));
      }

      // Return the hexadecimal string.
      return sBuilder.ToString();
    }

    static Guid MakeCryptoGuid()
    {
      // Get 16 cryptographically random bytes
      RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
      byte[] data = new byte[16];
      rng.GetBytes(data);

      // Mark it as a version 4 GUID
      data[7] = (byte)((data[7] | (byte)0x40) & (byte)0x4f);
      data[8] = (byte)((data[8] | (byte)0x80) & (byte)0xbf);

      return new Guid(data);
    }

    public static string GenerateSalt(int byteLength = SaltSize)
    {
      byte[] Buff = new byte[byteLength];
      using (var Prng = new RNGCryptoServiceProvider())
      {
        Prng.GetBytes(Buff);
      }

      return Convert.ToBase64String(Buff);
    }


    public static string Hash(this string input, string algorithm = "sha256")
    {
      if (input == null)
      {
        throw new ArgumentNullException("input");
      }

      return Hash(Encoding.UTF8.GetBytes(input), algorithm);
    }

    public static string Hash(this byte[] input, string algorithm = "sha256")
    {
      if (input == null)
      {
        throw new ArgumentNullException("input");
      }

      using (HashAlgorithm alg = HashAlgorithm.Create(algorithm))
      {
        if (alg != null)
        {
          byte[] hashData = alg.ComputeHash(input);
          return BinaryToHex(hashData);
        }
        else
        {
          throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "not supported hash alg", algorithm));
        }
      }
    }

    public static string SHA1(this string input)
    {
      return Hash(input, "sha1");
    }
    public static string SHA256(this string input)
    {
      return Hash(input, "sha256");
    }

    /* =======================
     * HASHED PASSWORD FORMATS
     * =======================
     * 
     * Version 0:
     * PBKDF2 with HMAC-SHA1, 128-bit salt, 256-bit subkey, 1000 iterations.
     * (See also: SDL crypto guidelines v5.1, Part III)
     * Format: { 0x00, salt, subkey }
     */

    public static string HashPassword(this string password)
    {
      if (password == null)
      {
        throw new ArgumentNullException("password");
      }

      byte[] salt;
      byte[] subkey;
      using (var deriveBytes = new Rfc2898DeriveBytes(password, SaltSize, Pbkdf2Count))
      {
        salt = deriveBytes.Salt;
        subkey = deriveBytes.GetBytes(Pbkdf2SubkeyLength);
      }

      byte[] outputBytes = new byte[1 + SaltSize + Pbkdf2SubkeyLength];
      Buffer.BlockCopy(salt, 0, outputBytes, 1, SaltSize);
      Buffer.BlockCopy(subkey, 0, outputBytes, 1 + SaltSize, Pbkdf2SubkeyLength);
      return Convert.ToBase64String(outputBytes);
    }

    // hashedPassword must be of the format of HashWithPassword (salt + Hash(salt+input)
    public static bool VerifyHashedPassword(string hashedPassword, string password)
    {
      if (hashedPassword == null)
      {
        throw new ArgumentNullException("hashedPassword");
      }
      if (password == null)
      {
        throw new ArgumentNullException("password");
      }

      byte[] hashedPasswordBytes = Convert.FromBase64String(hashedPassword);

      // Verify a version 0 (see comment above) password hash.

      if (hashedPasswordBytes.Length != (1 + SaltSize + Pbkdf2SubkeyLength) || hashedPasswordBytes[0] != (byte)0x00)
      {
        // Wrong length or version header.
        return false;
      }

      byte[] salt = new byte[SaltSize];
      Buffer.BlockCopy(hashedPasswordBytes, 1, salt, 0, SaltSize);
      byte[] storedSubkey = new byte[Pbkdf2SubkeyLength];
      Buffer.BlockCopy(hashedPasswordBytes, 1 + SaltSize, storedSubkey, 0, Pbkdf2SubkeyLength);

      byte[] generatedSubkey;
      using (var deriveBytes = new Rfc2898DeriveBytes(password, salt, Pbkdf2Count))
      {
        generatedSubkey = deriveBytes.GetBytes(Pbkdf2SubkeyLength);
      }
      return ByteArraysEqual(storedSubkey, generatedSubkey);
    }

    internal static string BinaryToHex(byte[] data)
    {
      char[] hex = new char[data.Length * 2];

      for (int iter = 0; iter < data.Length; iter++)
      {
        byte hexChar = ((byte)(data[iter] >> 4));
        hex[iter * 2] = (char)(hexChar > 9 ? hexChar + 0x37 : hexChar + 0x30);
        hexChar = ((byte)(data[iter] & 0xF));
        hex[iter * 2 + 1] = (char)(hexChar > 9 ? hexChar + 0x37 : hexChar + 0x30);
      }
      return new string(hex);
    }

    // Compares two byte arrays for equality. The method is specifically written so that the loop is not optimized.
    [MethodImpl(MethodImplOptions.NoOptimization)]
    private static bool ByteArraysEqual(byte[] a, byte[] b)
    {
      if (Object.ReferenceEquals(a, b))
      {
        return true;
      }

      if (a == null || b == null || a.Length != b.Length)
      {
        return false;
      }

      bool areSame = true;
      for (int i = 0; i < a.Length; i++)
      {
        areSame &= (a[i] == b[i]);
      }
      return areSame;
    }
    /// <summary>
    /// 
    ///                                        name
    /// makecert -sr LocalMachine -ss my -n CN=WebAPI-Token -sky exchange -pe
    /// </summary>
    /// <param name="subjectName"></param>
    /// <returns></returns>
    public static X509Certificate2 GetX509Certificate(string subjectName)
    {
      X509Store certificateStore = new X509Store(StoreName.My, StoreLocation.LocalMachine);
      certificateStore.Open(OpenFlags.ReadOnly);
      X509Certificate2 certificate;

      try
      {
        certificate = certificateStore.Certificates.OfType<X509Certificate2>()
          .FirstOrDefault(cert => cert.SubjectName.Name.Equals(subjectName, StringComparison.OrdinalIgnoreCase));
      }
      finally
      {
        certificateStore.Close();
      }

      if (certificate == null)
        throw new Exception(string.Format("Certificate '{0}' not found.", subjectName));

      return certificate;
    }


    /// <summary>
    ///                                        name              encrytion allowed
    /// makecert -sr LocalMachine -ss my -n CN=WebAPI-Token -sky exchange -pe
    /// </summary>
    /// <param name="certificate"></param>
    /// <param name="plainToken"></param>
    /// <returns></returns>
    public static string Encrypt(X509Certificate2 certificate, string plainToken)
    {
      RSACryptoServiceProvider cryptoProvidor = (RSACryptoServiceProvider)certificate.PublicKey.Key;
      byte[] encryptedTokenBytes = cryptoProvidor.Encrypt(System.Text.Encoding.UTF8.GetBytes(plainToken), true);
      return Convert.ToBase64String(encryptedTokenBytes);
    }

    public static string Decrypt(X509Certificate2 certificate, string encryptedToken)
    {
      RSACryptoServiceProvider cryptoProvidor = (RSACryptoServiceProvider)certificate.PrivateKey;
      byte[] decryptedTokenBytes = cryptoProvidor.Decrypt(Convert.FromBase64String(encryptedToken), true);
      return Encoding.UTF8.GetString(decryptedTokenBytes);
    }

    /// <summary>
    /// uses default certificate "DefaultEncryptionCertificate"
    ///                                        name              encrytion allowed
    /// makecert -sr LocalMachine -ss my -n CN=DefaultEncryptionCertificate -sky exchange -pe
    /// </summary>
    /// <param name="certificate"></param>
    /// <param name="plainToken"></param>
    /// <returns></returns>
    public static string Encrypt(string plaintext)
    {
      var cert = GetX509Certificate("CN=DefaultEncryptionCertificate");
      return Encrypt(cert, plaintext);
    }
    public static string Decrypt(string encrypted)
    {
      var cert = GetX509Certificate("CN=DefaultEncryptionCertificate");
      return Decrypt(cert, encrypted);
    }


    public static string HashFile(string filePath)
    {
      using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
      {
        return HashStream(fs);
      }
    }

    public static string HashASCIIString(string value, Encoding encoding = null)
    {
      encoding = encoding ?? Encoding.ASCII;
      var bytes = encoding.GetBytes(value);
      var stream = new MemoryStream(bytes);
      var result = HashStream(stream);
      return result;
    }
    public static string HashStream(Stream stream)
    {
      StringBuilder sb = new StringBuilder();

      if (stream != null)
      {
        stream.Seek(0, SeekOrigin.Begin);

        MD5 md5 = MD5CryptoServiceProvider.Create();
        byte[] hash = md5.ComputeHash(stream);
        foreach (byte b in hash)
          sb.Append(b.ToString("x2"));

        stream.Seek(0, SeekOrigin.Begin);
      }

      return sb.ToString();
    }
  }
}
