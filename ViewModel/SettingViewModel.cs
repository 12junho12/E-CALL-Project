using AUA.AiS_FruiT;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using log4net;
using System;

namespace CargoLampTest.ViewModel
{
    /// <summary>
    /// https://www.codeproject.com/Articles/323187/MVVMLight-Using-Two-Views
    /// </summary>
    public class SettingViewModel : ViewModelBase
    {
        private static readonly ILog pLogger = LogHelper.GetLoggerRollingFileAppender("Root");
        private static readonly ILog eLogger = LogHelper.GetLoggerRollingFileAppender("Exception");
        public RelayCommand ConnectionCommand { get; set; }

        public SettingViewModel()
        {

            StartCommand = new RelayCommand(StartProcess, true);
            StopCommand = new RelayCommand(StopProcess, true);

        }

        private double _measureValue1;
        public double MeasureValue1
        {
            get { return _measureValue1; }
            set { _measureValue1 = value; RaisePropertyChanged("MeasureValue1"); }
        }
        private double _measureValue2;
        public double MeasureValue2
        {
            get { return _measureValue2; }
            set { _measureValue2 = value; RaisePropertyChanged("MeasureValue2"); }
        }

        #region property

        private bool _IsStartButtonEnabled = true;
        public bool IsStartButtonEnabled
        {
            get { return _IsStartButtonEnabled; }
            set { _IsStartButtonEnabled = value; RaisePropertyChanged("IsStartButtonEnabled"); }
        }

        private bool _IsStopButtonEnabled = false;
        public bool IsStopButtonEnabled
        {
            get { return _IsStopButtonEnabled; }
            set { _IsStopButtonEnabled = value; RaisePropertyChanged("IsStopButtonEnabled"); }
        }
        #endregion

        #region RelayCommand
        public RelayCommand StartCommand { get; set; }
        void StartProcess()
        {
            IsStartButtonEnabled = false;
            IsStopButtonEnabled = true;



        }
        public RelayCommand StopCommand { get; set; }
        void StopProcess()
        {

            IsStartButtonEnabled = true;
            IsStopButtonEnabled = false;


        }
        #endregion

    }
}
