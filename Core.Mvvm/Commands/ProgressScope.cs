using System.Linq;
using System.Windows;

namespace Core.Common.MVVM
{

    public class ProgressScope : RecursiveExecutionScope<ProgressScope>
    {
        public ProgressScope()
        {

        }

        double _progress;

        public double Progress
        {
            get
            {
                return _progress;
            }
            set
            {
                if (this.ChangeProperty(ref _progress, value))
                {
                    _progress = value;
                    var parents = PathFromRoot.ToArray();

                    this.RaisePropertyChanged(() => Progress);
                    foreach (var p in parents)
                    {
                        p.RaisePropertyChanged(() => CombinedProgress);
                    }

                }
            }
        }




        int _descendents = 0;
        public override void DescendantAdded(ProgressScope child)
        {
            _descendents++;
            this.RaisePropertyChanged(() => CombinedProgress);
        }
        public override void DescendantRemoved(ProgressScope child)
        {
            this.RaisePropertyChanged(() => CombinedProgress);
        }





        public double CombinedProgress
        {
            get
            {
                var dd = this.GetDescendentsAndSelf().ToArray();
                return (dd.Sum(d => d.Progress) + (_descendents + 1 - dd.Length)) / (_descendents + 1);
            }

        }



    }
}

