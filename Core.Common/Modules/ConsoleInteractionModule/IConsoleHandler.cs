using System.ComponentModel.Composition;

namespace Core.Modules.ConsoleInteraction
{
  [InheritedExport]
  public interface IConsoleHandler
  {
    int Priority { get; }
    bool HandleCommand(string command);
  }
}
