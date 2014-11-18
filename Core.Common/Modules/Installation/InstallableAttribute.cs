using System.ComponentModel.Composition;
using System.Linq;
namespace Core.Modules.Installation
{
  public class InstallableAttribute : ExportAttribute
  {
    public InstallableAttribute() : base(InstallableContractName) { }
    public InstallableAttribute(string requirements)
      : base(InstallableContractName)
    {
      InstallationRequirements =  requirements.Split(';').Select(str => str.Trim()).ToArray();
    }
    public const string InstallableContractName = "InstallableContract";
    public string InstallationName { get; set; }
    public string InstallationVersion { get; set; }
    public string[] InstallationRequirements { get; set; }
  }




}
