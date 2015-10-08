namespace Core.Common.Crypto
{
  public interface IX509Certificate2Generator
  {
    System.Security.Cryptography.X509Certificates.X509Certificate2 Generate(string subjectName, string certAlias, string password);
  }
}