using System;
using System.Diagnostics;
using System.Windows;
using Core.Common.MVVM;
using System.Windows.Threading;
using System.ComponentModel;
using Core.Common.Wpf.Export;

namespace Core.Common.Wpf
{
    
    public class CommandContextItem : Core.NotifyPropertyChangedBase
    {
        public CommandContextItem(CommandExecutionContext context, DependencyObject owner, TimeSpan? timeout = null)
        {
            EndOfLife = DateTime.Now + (timeout ?? TimeSpan.FromSeconds(2));
            this.CommandContext = context ?? throw new ArgumentNullException(nameof(context));
            this.Owner = owner ?? throw new ArgumentNullException(nameof(owner));


            CommandContext.ForwardChange(this, nameof(CancelCanExecute));

            _id = _idCount++;
        }
        static int _idCount = 0;
        int _id;

        public CommandExecutionContext CommandContext { get; private set; }
        public DependencyObject Owner { get; private set; }

        public override string ToString()
        {
            return _id+" "+CommandContext.ToString();
        }
        public void Remove()
        {
            CommandScope.GetContexts(Owner)?.Remove(this);
        }

        public void Cancel()
        {
            CommandContext.Cancel();
        }



        public bool CancelCanExecute
        {
            get { return CommandContext. CanCancel; }
        }
        public void Pause()
        {
            CommandContext.Pause();
        }

        public bool PauseCanExecute
        {
            get { return CommandContext.CanPause; }
        }

        public bool KeepCanExecute
        {
            get
            {
                return !IsAutoRemoving;
            }
        }

        public void Keep()
        {

            if (_timer != null)
            {
                _timer.Stop();
            }
            IsAutoRemoving = false;
        }

        private DispatcherTimer _timer = null;
        public void AutoRemove(DateTime endOfLife)
        {
            if (_timer != null)
            {
                _timer.Stop();
            }
            EndOfLife = endOfLife;
            IsAutoRemoving = true;

            _timer = new DispatcherTimer();
            _timer.Tick += (sender, args) => Remove();
            _timer.Stop();
            _timer.Interval = endOfLife - DateTime.Now;
            _timer.Start();
        }

        private bool _isAutoRemoving;
        public bool IsAutoRemoving
        {
            get { return _isAutoRemoving; }
            private set
            {
                if (this.ChangeProperty(ref _isAutoRemoving, value))
                {
                    this.RaisePropertyChanged(nameof(KeepCanExecute));
                }
            }
        }


        private DateTime _endOfLife;
        public DateTime EndOfLife { get { return _endOfLife; } private set { this.ChangeProperty(ref _endOfLife, value); } }

    }






}
