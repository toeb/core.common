namespace Core.Common.MVVM
{
    public interface IProgressReporter
    {
        void Report(double progress);
        void SetText(string text);
        void Complete();
    }


    

}
