using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
//using System.Dynamic;
using Core.Common.Reflect;
using System.IO;
using System.Globalization;
using Core.Common;
using Core.Common.Crypto;
using Core.Common.Crypto.External;

namespace Core.Test
{



  class TestObject
  {
    public string a { get; set; }
  }
  [TestClass]
  public class Core
  {

    [TestMethod]
    public void ShouldCreateAStreamFromAString()
    {
      var stream = "hello world".AsStream();
      var result = new StreamReader(stream).ReadToEnd();
      Assert.AreEqual("hello world", result);
    }

    [TestMethod]
    public void ShouldCreateATextReaderFromAString()
    {
      var reader = "hello world".AsReader();
      Assert.AreEqual("hello world", reader.ReadToEnd());
    }

    [TestMethod]
    public void EncryptDecryptObject()
    {
      var cert = new BouncyCastleX509Certificate2Generator().Generate("a", "a", "a");
      var obj = new TestObject() {a ="asd"};
      var str = CryptoObject.Encrypt(obj,cert);
      Assert.IsNotNull(str);

      var result = CryptoObject.Decrypt(str,cert);
      Assert.AreEqual(obj.GetType(), result.GetType());
      var cast = result as TestObject;
      Assert.IsNotNull(cast);
      Assert.AreEqual(obj.a, cast.a);

    }

    [TestMethod]
    public void EncryptDecryptInt()
    {
      
      var cert = new BouncyCastleX509Certificate2Generator().Generate("asd", "asd", "asd");
      
      var enc = Cryptography.Encrypt(cert,"1");
      var dec = Cryptography.Decrypt(cert, enc);
      Assert.AreEqual("1", dec);
    }

    [TestMethod]
    public void DefaultEncryptDecrypt()
    {
      var cert = new BouncyCastleX509Certificate2Generator().Generate("a", "a", "a");

      var enc = Cryptography.Encrypt(cert, "hello");
      var dec = Cryptography.Decrypt(cert,enc);
      Assert.AreEqual("hello", dec, "decryption ouput should have given the input");
    }


    [TestMethod]
    public void EncryptDecrypt()
    {

      // if fails run following as admin in shell with visual studio vars:  makecert -sr LocalMachine -ss my -n CN=WebAPI-Token -sky exchange -pe
      var cert = new  BouncyCastleX509Certificate2Generator().Generate("asdasd","asdasd", "asdasda");
      var encrypted = Cryptography.Encrypt(cert, "hello world");
      var result = Cryptography.Decrypt(cert, encrypted);

      Assert.AreEqual("hello world", result);
    }

    [TestMethod]
    public void HashAndCheckPassword()
    {

      string password = "aPassWord";
      var hash = password.HashPassword();
      Assert.IsNotNull(hash);
      Assert.IsTrue(Cryptography.VerifyHashedPassword(hash, "aPassWord"));
      //different case
      Assert.IsFalse(Cryptography.VerifyHashedPassword(hash, "aPassWorD"));
    }

    [TestMethod]
    public void ShortenString()
    {
      string longString = "lorem ipsum i do not know how to write latin";
      string shortened = longString.Shorten(8);
      Assert.AreEqual("lore ...", shortened);
    }

    [TestMethod]
    public void StringIsNullOrEmptyNull()
    {
      string nullString = null;
      Assert.IsTrue(nullString.IsNullOrEmpty());
    }
    [TestMethod]
    public void StringIsNullOrEmptyEmpty()
    {
      string nullString = "";
      Assert.IsTrue(nullString.IsNullOrEmpty());
    }

    [TestMethod]
    public void StringIsNullOrEmptyNotNullNorEmpty()
    {
      string nullString = " ";
      Assert.IsFalse(nullString.IsNullOrEmpty());
    }

    [TestMethod]
    public void RandomStringTest()
    {
      var list = new List<string>();
      for (int i = 0; i < 50; i++)
      {
        list.Add(StringExtensions.RandomString());
      }
      // every item is different from every other item
      Assert.AreEqual(list.Count(), list.Distinct().Count());
    }


    //[TestMethod]
    //public void NullableIntParse()
    //{
    //  var result = "32323".ParseInt();
    //  Assert.AreEqual(32323, result);
    //}


    [TestMethod]
    public void ShouldCreateADictionaryFromTheDynamicObject()
    {
      var obj = new { a = 4, b = 3 };
      dynamic d = new System.Dynamic.ExpandoObject();
      d.var1 = "asd";
      d.var2 = 1234;
      d.var3 = obj;

      var result = DynamicObject.ToDictionary(d);

      Assert.AreEqual(result["var1"], "asd");
      Assert.AreEqual(result["var2"], 1234);
      Assert.AreEqual(result["var3"], obj);
    }

    [TestMethod]
    public void ShouldReturnThePropertyValueOfANormalObject()
    {
      var result = DynamicObject.GetPropertyValue(new { test = "231" }, "test");
      Assert.AreEqual("231", result);
    }
    [TestMethod]
    public void ShouldReturnADictionaryOfANormalObject()
    {
      var result = DynamicObject.ToDictionary(new { test = "44", test2 = 23 });
      Assert.AreEqual("44", result["test"]);
      Assert.AreEqual(23, result["test2"]);
    }
    [TestMethod]
    public void ShouldReturnThePropertyValueOfADynamicObject()
    {
      dynamic d = new System.Dynamic.ExpandoObject();
      d.test = "asd";
      var result = DynamicObject.GetPropertyValue(d, "test");
      Assert.AreEqual("asd", result);
    }


    [TestMethod]
    public void ShouldShowLeftWinsStrategyWorks()
    {
      var left = new Dictionary<object, object>() { { "a", "1" }, { "b", "2" } };
      var right = new Dictionary<object, object>() { { "a", "4" }, { "c", "2" } };

      left.MergeInplace(right, DictionaryMergeStrategy.LeftWins);

      Assert.AreEqual("1", left["a"]);
      Assert.AreEqual("2", left["b"]);
      Assert.AreEqual("2", left["c"]);

    }

    [TestMethod]
    public void ShouldShowRightWinsStrategyWorks()
    {
      var left = new Dictionary<object, object>() { { "a", "1" }, { "b", "2" } };
      var right = new Dictionary<object, object>() { { "a", "4" }, { "c", "2" } };

      left.MergeInplace(right, DictionaryMergeStrategy.RightWins);

      Assert.AreEqual("4", left["a"]);
      Assert.AreEqual("2", left["b"]);
      Assert.AreEqual("2", left["c"]);

    }


    [TestMethod]
    public void ShouldFormatWithoutException()
    {
     var res = "the whole world {hello} {other:ddMMyyy}".FormatWith( new { hello = "asd", other = DateTime.Now });
    }


    [TestMethod]
    public void ShouldFormatByIndexCorrectly()
    {
      var res = "{0},{1:ddMMyyyy}".FormatWith(new { first = "hello", second = new DateTime(2000, 1, 1) });
      Assert.AreEqual("hello,01012000",res);
    }
    
  }
}