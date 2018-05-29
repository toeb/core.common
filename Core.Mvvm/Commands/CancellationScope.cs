using System;

namespace Core.Common.MVVM
{
    public class CancellationScope : RecursiveExecutionScope<CancellationScope>
    {
        private static int _idCount = 0;
        private int _id = _idCount++;
        public CancellationScope()
        {
            Console.WriteLine("cancellation scope created " + _id);
        }
        public override string ToString() => $"CancellationScope-{_id}";

        private bool _isCancelRequest;
        public bool IsCancelRequested
        {
            get { return _isCancelRequest; }
            private set
            {
                if (this.ChangeProperty(ref _isCancelRequest, value))
                {
                    foreach(var child in Children)
                    {
                        child.Cancel();
                    }

                }
            }
        }
        
        public void CancelCancellation()
        {

        }

        public void Cancel()
        {
            
            Console.WriteLine("requesting cancellation in scope " + _id);

            IsCancelRequested = true;
        }
    }
}

