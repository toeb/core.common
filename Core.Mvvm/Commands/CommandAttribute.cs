
namespace Core.Common.MVVM
{
	public interface ICommandInfo
	{
		string Icon { get; }
		string CommandName { get; }
		string CanExecuteMember { get; }
		bool AllowConcurrentExecution { get; }

	}
	public class CommandAttribute : System.Attribute
	{
		public CommandAttribute() {}
		public CommandAttribute(string name) { CommandName = name;  }

		public bool AllowConcurrentExecution { get; set; }
		public string Icon { get; set; }
		public string DisplayType { get; set; }
		public string CommandName { get; set; }
		public string CanExecuteMember { get; set; }
	}
}
