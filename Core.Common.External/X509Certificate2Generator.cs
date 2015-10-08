using Core.Common.Crypto;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Prng;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common.Crypto.External
{
  public class BouncyCastleX509Certificate2Generator : IX509Certificate2Generator
  {

    /// <summary>
    /// 
    /// </summary>
    /// <param name="subjectName"></param>
    /// <param name="certAlias"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public System.Security.Cryptography.X509Certificates.X509Certificate2 Generate(string subjectName, string certAlias, string password)
    {
      AsymmetricCipherKeyPair kp;
      var cert = GenerateCertificate(subjectName, out kp);
      var x5092 = ConvertCertificate(cert, kp, certAlias, password);
      return x5092;
    }



    private X509Certificate GenerateCertificate(string subjectName, out AsymmetricCipherKeyPair kp)
    {
      var kpgen = new RsaKeyPairGenerator();

      // certificate strength 1024 bits
      kpgen.Init(new KeyGenerationParameters(
            new SecureRandom(new CryptoApiRandomGenerator()), 1024));

      kp = kpgen.GenerateKeyPair();

      var gen = new X509V3CertificateGenerator();

      var certName = new X509Name("CN=" + subjectName);
      var serialNo = BigInteger.ProbablePrime(120, new Random());

      gen.SetSerialNumber(serialNo);
      gen.SetSubjectDN(certName);
      gen.SetIssuerDN(certName);
      gen.SetNotAfter(DateTime.Now.AddYears(100));
      gen.SetNotBefore(DateTime.Now.Subtract(new TimeSpan(7, 0, 0, 0)));
      gen.SetSignatureAlgorithm("SHA1withRSA");
      gen.SetPublicKey(kp.Public);

      gen.AddExtension(
          X509Extensions.AuthorityKeyIdentifier.Id,
          false,
          new AuthorityKeyIdentifier(
              SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(kp.Public),
              new GeneralNames(new GeneralName(certName)),
              serialNo));

      /* 
       1.3.6.1.5.5.7.3.1 - id_kp_serverAuth 
       1.3.6.1.5.5.7.3.2 - id_kp_clientAuth 
       1.3.6.1.5.5.7.3.3 - id_kp_codeSigning 
       1.3.6.1.5.5.7.3.4 - id_kp_emailProtection 
       1.3.6.1.5.5.7.3.5 - id-kp-ipsecEndSystem 
       1.3.6.1.5.5.7.3.6 - id-kp-ipsecTunnel 
       1.3.6.1.5.5.7.3.7 - id-kp-ipsecUser 
       1.3.6.1.5.5.7.3.8 - id_kp_timeStamping 
       1.3.6.1.5.5.7.3.9 - OCSPSigning
       */
      gen.AddExtension(
          X509Extensions.ExtendedKeyUsage.Id,
          false,
          new ExtendedKeyUsage(new[] { KeyPurposeID.IdKPServerAuth }));

      var newCert = gen.Generate(kp.Private);

      return newCert;
    }


    private void SaveToStream(
        X509Certificate newCert,
        AsymmetricCipherKeyPair kp,
        Stream stream,
        string CertAlias,
        string Password)
    {
      var newStore = new Pkcs12Store();

      var certEntry = new X509CertificateEntry(newCert);

      newStore.SetCertificateEntry(
          CertAlias,
          certEntry
          );

      newStore.SetKeyEntry(
          CertAlias,
          new AsymmetricKeyEntry(kp.Private),
          new[] { certEntry }
          );

      newStore.Save(
          stream,
          Password.ToCharArray(),
          new SecureRandom(new CryptoApiRandomGenerator())
          );
    }


    private System.Security.Cryptography.X509Certificates.X509Certificate2 ConvertCertificate(X509Certificate newCert, AsymmetricCipherKeyPair kp, string certAlias, string password)
    {
      using (var stream = new MemoryStream())
      {
        SaveToStream(newCert, kp, stream, certAlias, password);
        var x5092 = new System.Security.Cryptography.X509Certificates.X509Certificate2(stream.ToArray(), password);
        return x5092;
      }
    }


    private System.Security.Cryptography.X509Certificates.X509Certificate2 ReadX509Certificate2(string FilePath, string Pwd)
    {
      var x5092 = new System.Security.Cryptography.X509Certificates.X509Certificate2(FilePath, Pwd);

      return x5092;
    }


  }
}
