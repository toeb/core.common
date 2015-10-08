using Core.Common.Crypto;
using Core.Common.Crypto.External;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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

namespace Core.Common.Tests
{



  [TestClass]
  public class CertificateGeneratorTest
  {
    [TestMethod]
    public void GenerateX5092Certificate()
    {
      IX509Certificate2Generator uut = new BouncyCastleX509Certificate2Generator();
      var cert = uut.Generate("thesubject", "alias", "amdk623d");
      Assert.IsNotNull(cert);
    }
  }
}
