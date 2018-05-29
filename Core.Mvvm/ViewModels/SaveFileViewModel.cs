using Core.Common.Injection;
using System.ComponentModel.Composition;

namespace Core.Common.MVVM
{

    public class SaveFileViewModel : NotifyPropertyChangedBase
    {
        [ImportingConstructor]
        private SaveFileViewModel([Import] IInjectionService ijs) { }
        public static implicit operator bool(SaveFileViewModel self)
        {
            if (self == null)
            {
                return false;
            }
            return self.Success.HasValue && self.Success.Value;
        }
        private bool? _success;
        public bool? Success { get => _success; set => this.Change(ref _success, value); }
        private string[] _fileNames;
        public string[] FileNames { get => _fileNames; set => this.Change(ref _fileNames, value); }


    }
}
