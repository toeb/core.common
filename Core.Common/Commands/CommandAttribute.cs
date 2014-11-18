using System;
using System.ComponentModel.Composition;

namespace Core.Commands
{
  [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property)]
  [MetadataAttribute]
  public class CommandAttribute : ExportAttribute
  {
    public CommandAttribute(string name) : base("Command", typeof(Delegate)) { Name = name; }
    public CommandAttribute() : base("Command", typeof(Delegate)) { Name = null; }
    public string HelpText{ get; set; }
    public string ShortName { get; set; }
    public string Name { get; set; }
  }
}
