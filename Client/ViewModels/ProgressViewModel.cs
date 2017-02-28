namespace HomeHub.Client.ViewModels
{
    using System;

    public class ProgressViewModel : ViewModelBase
    {
        private static ProgressViewModel _instace = null;

        private bool isBlockingProgress;
        private bool isNonBlockingProgress;
        private string blockingProgressText;
        private string nonBlockingProgressText;

        private ProgressViewModel()
        {
            isBlockingProgress = false;
            isNonBlockingProgress = false;
            blockingProgressText = String.Empty;
            nonBlockingProgressText = String.Empty;
        }
        
        public static ProgressViewModel Instance
        {
            get
            {
                if (_instace == null)
                {
                    _instace = new ProgressViewModel();
                }
                return _instace;
            }
        }

        public bool IsBlockingProgress
        {
            get
            {
                return isBlockingProgress;
            }

            set
            {
                SetProperty(ref isBlockingProgress, value);
            }
        }

        public string BlockingProgressText
        {
            get
            {
                return blockingProgressText;
            }

            set
            {
                SetProperty(ref blockingProgressText, value);
            }
        }

        public bool IsNonBlockingProgress
        {
            get
            {
                return isNonBlockingProgress;
            }
            set
            {
                SetProperty(ref isNonBlockingProgress, value);
            }
        }

        public string NonBlockingProgressText
        {
            get
            {
                return nonBlockingProgressText;
            }

            set
            {
                SetProperty(ref nonBlockingProgressText, value);
            }
        }
    }
}
