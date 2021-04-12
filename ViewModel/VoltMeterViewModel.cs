using AUA.AiS_FruiT;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using log4net;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;

namespace CargoLampTest.ViewModel
{
    public class VoltMeterViewModel : ViewModelBase
    {
        public static Guid TheId { get; set; } = Guid.NewGuid();
        private static Ini _ini = new Ini(Path.GetFullPath("env.ini"));
        private static readonly ILog pLogger = LogHelper.GetLoggerRollingFileAppender("Root");
        private static readonly ILog eLogger = LogHelper.GetLoggerRollingFileAppender("Exception");
        public RelayCommand ConnectionCommand { get; set; }
        public DeviceMT4N _deviceMT4N = new DeviceMT4N();
        private bool _isRunning = false;
        private Timer _timer = new Timer();
        


        #region property

        #endregion

        public VoltMeterViewModel()
        {

            StartCommand = new RelayCommand(StartProcess, true);
            StopCommand = new RelayCommand(StopProcess, true);

            try
            {
                SerialPortNumber = _ini.GetValue("NETWORK_INFO", "MT4N");
             }
            catch (Exception ex)
            {
                eLogger.Error(ex.ToString());
            }

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
        private double _measureValue3;
        public double MeasureValue3
        {
            get { return _measureValue3; }
            set { _measureValue3 = value; RaisePropertyChanged("MeasureValue3"); }
        }

        private string _SerialPortNumber = "0";
        public string SerialPortNumber
        {
            get { return _SerialPortNumber; }
            set
            {
                if (_SerialPortNumber == value) return;

                _ini.SetValue("NETWORK_INFO", "MT4N", value.ToString());
                _SerialPortNumber = value;
                RaisePropertyChanged("SerialPortNumber");
            }
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

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (_isRunning)
            {
                try
                {
                    _deviceMT4N.ModbusSerialRTUMasterReadRegisters();
                }
                catch (Exception ex)
                {
                    eLogger.Error(ex.ToString());
                }


                MeasureValue1 = _deviceMT4N._measureValue[0];
                MeasureValue2 = _deviceMT4N._measureValue[1];
                MeasureValue3 = _deviceMT4N._measureValue[2];
            }

        }

        public RelayCommand StartCommand { get; set; }
        void StartProcess()
        {
            IsStartButtonEnabled = false;
            IsStopButtonEnabled = true;

            if (_deviceMT4N.IsConnected)
            {
                _timer.Start();
                _isRunning = true;
            }
            else
            {
                Connect();
                _isRunning = true;
            }

        }
        public RelayCommand StopCommand { get; set; }
        void StopProcess()
        {
            _timer.Stop();
            _isRunning = false;

            IsStartButtonEnabled = true;
            IsStopButtonEnabled = false;

            Task.Delay(1000).Wait();
            MeasureValue1 = 0;
            MeasureValue2 = 0;
            MeasureValue3 = 0;

        }

        public void Connect()
        {
            _deviceMT4N.Connect(SerialPortNumber);

            _timer.Interval = 500;
            _timer.Elapsed += new ElapsedEventHandler(Timer_Tick);
            _timer.Start();
        }




    }
}
