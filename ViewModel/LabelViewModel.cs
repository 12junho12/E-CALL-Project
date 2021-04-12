using AUA.AiS_FruiT;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using log4net;
using System;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Windows.Threading;


namespace CargoLampTest.ViewModel
{
    public class LabelViewModel : ViewModelBase
    {
        private static readonly ILog pLogger = LogHelper.GetLoggerRollingFileAppender("Root");
        private static readonly ILog eLogger = LogHelper.GetLoggerRollingFileAppender("Exception");

        public  DeviceLABEL _deviceLABEL = new DeviceLABEL();
        private DispatcherTimer _timer = new  DispatcherTimer();
        private bool _isRunning = false;
        public LabelViewModel()
        {
            StartCommand = new RelayCommand(StartProcess, true);
            StopCommand = new RelayCommand(StopProcess, true);
            _timer.Interval = TimeSpan.FromMilliseconds(500);
            _timer.Tick += new EventHandler(Timer_Tick);

        }

        private ObservableCollection<bool?> ledStatus = new ObservableCollection<bool?>
            { false, false, false, false, false, false, false, false,
                false, false, false, false, false, false, false, false};

        public ObservableCollection<bool?> LedStatus
        {
            get { return ledStatus; }
            set {
                if (ledStatus == value) return;
                ledStatus = value;
                RaisePropertyChanged("LedStatus");

            }
        }

        private bool[] checkStatus = new bool[]
            { false, false, false, false, false, false, false, false,
                false, false, false, false, false, false, false, false};


        public bool this[int index]
        {
            get { return checkStatus[index]; }
            set
            {
                if (checkStatus[index] == value) return;

                checkStatus[index] = value;
                RaisePropertyChanged("CheckStatus");
            }
        }

        private Color coloreOff;
        public Color ColoreOff
        {
            get { return coloreOff; }
            set
            {
                coloreOff = value;
                this.RaisePropertyChanged("ColoreOff");
            }
        }

        private bool flash;
        public bool Flash
        {
            get { return flash; }
            set
            {
                flash = value;
                this.RaisePropertyChanged("Flash");
            }
        }

        private Color _colorOn = Colors.Green;
        public Color ColorOn
        {
            get { return _colorOn; }
        }

        private Color _colorOff = Colors.Red;
        public Color ColorOff
        {
            get { return _colorOff; }
        }


        private bool _isStartButtonEnabled = true;
        public bool IsStartButtonEnabled
        {
            get { return _isStartButtonEnabled; }
            set { _isStartButtonEnabled = value; RaisePropertyChanged("IsStartButtonEnabled"); }
        }
        private bool _isStopButtonEnabled = false;

        public bool IsStopButtonEnabled
        {
            get { return _isStopButtonEnabled; }
            set { _isStopButtonEnabled = value; RaisePropertyChanged("IsStopButtonEnabled"); }
        }
        public RelayCommand StartCommand { get; set; }
        void StartProcess()
        {
            IsStartButtonEnabled = false;
            IsStopButtonEnabled = true;

            if (_deviceLABEL.IsConnected)
            {
                _timer.Interval = TimeSpan.FromMilliseconds(100);
                _isRunning = true;
                _timer.Start();
            }
            else
            {
                Connect();
                _isRunning = true;
                _timer.Start();
            }

        }
        public RelayCommand StopCommand { get; set; }
        void StopProcess()
        {
            _timer.Stop();
            _isRunning = false;

            IsStartButtonEnabled = true;
            IsStopButtonEnabled = false;

            for (int i = 0; i < 16; i++)
            {
                LedStatus[i] = false;
            }

        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            //TODO 라벨에서 리턴되는 값이 있으면 이를 이용해서 주기적으로 연결성 확인

            if (_isRunning)
            {

            }
        }

        public void Connect()
        {
            _deviceLABEL.Connect(1);
            _timer.Interval = TimeSpan.FromMilliseconds(500);
            _timer.Tick += new EventHandler(Timer_Tick);
        }

    }

 
}
