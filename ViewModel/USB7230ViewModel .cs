using AUA.AiS_FruiT;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using log4net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Threading;
using CargoLampTest.Model;


namespace CargoLampTest.ViewModel
{
    public class USB7230ViewModel : ViewModelBase
    {
        private static Ini _ini = new Ini(Path.GetFullPath("env.ini"));
        private static readonly ILog pLogger = LogHelper.GetLoggerRollingFileAppender("Root");
        private static readonly ILog eLogger = LogHelper.GetLoggerRollingFileAppender("Exception");

        public DeviceUSB7230 _deviceUSB7230 = new DeviceUSB7230();
        private DispatcherTimer _timer = new  DispatcherTimer();
        private bool _isRunning = false;
        public USB7230ViewModel()
        {
            StartCommand = new RelayCommand(StartProcess, true);
            StopCommand = new RelayCommand(StopProcess, true);
            _timer.Interval = TimeSpan.FromMilliseconds(500);
            _timer.Tick += new EventHandler(Timer_Tick);

            try
            {
                string dioID = _ini.GetValue("NETWORK_INFO", "USB7230");
                short id = Convert.ToInt16(dioID);
                DIO_ID = (ushort)id;
            }
            catch (Exception ex)
            {
                eLogger.Error(ex.ToString());
            }

            Messenger.Default.Register<NotificationMessage>(this, NotifyFromSelectableViewModel);
        }
        //SelectableViewModel에서 USB7230ViewModel에 데이타 전달하기위해서 MvvmLight의 MessengerInstance를 사용함.
        public void NotifyFromSelectableViewModel(NotificationMessage obj)
        {
            string[] received = obj.Notification.Trim().Split('/');
            int index = Convert.ToInt16(received[0]);
            bool OnOff =Convert.ToBoolean(received[1]);

            WriteLine(index, OnOff);
        }

        #region Property
        private ObservableCollection<bool?> _ledStatus = new ObservableCollection<bool?>
            { false, false, false, false, false, false, false, false,
              false, false, false, false, false, false, false, false};
        public ObservableCollection<bool?> LedStatus
        {
            get { return _ledStatus; }
            set {
                if (_ledStatus == value) return;
                _ledStatus = value;
                RaisePropertyChanged("LedStatus");
            }
        }
        
        private ObservableCollection<SelectableViewModel> _items = new ObservableCollection<SelectableViewModel>
            {
                new SelectableViewModel{ Code = DO.CylinderUp, Name = "CylinderUp",},
                new SelectableViewModel{ Code = DO.CylinderDown, Name = "CylinderDown",},
                new SelectableViewModel{ Code = DO.Select_1, Name = "Select_1",},
                new SelectableViewModel{ Code = DO.Select_2, Name = "Select_2",},
                new SelectableViewModel{ Code = DO.Select_3,  Name = "Select_3",},
                new SelectableViewModel{ Code = DO.Select_4,  Name = "Select_4",},
                new SelectableViewModel{ Code = DO.Select_5,  Name = "Select_5",},
                new SelectableViewModel{ Code = DO.Select_6,  Name = "Select_6",},

                new SelectableViewModel{ Code = 8,  Name = "8번",},
                new SelectableViewModel{ Code = 9,  Name = "9번",},
                new SelectableViewModel{ Code = 10,  Name = "10번",},
                new SelectableViewModel{ Code = 11,  Name = "11번",},
                new SelectableViewModel{ Code = 12,  Name = "12번",},
                new SelectableViewModel{ Code = 13,  Name = "13번",},
                new SelectableViewModel{ Code = 14,  Name = "14번",},
                new SelectableViewModel{ Code = 15,  Name = "15번",},
            };
        public ObservableCollection<SelectableViewModel> Items
        {
            get { return _items; }
            set
            {
                if (_items == value) return;

                _items = value;
                RaisePropertyChanged("Items");
            }
        }



        private Color _ColoreOff;
        public Color ColoreOff
        {
            get { return _ColoreOff; }
            set
            {
                _ColoreOff = value;
                this.RaisePropertyChanged("ColoreOff");
            }
        }

        private bool _Flash;
        public bool Flash
        {
            get { return _Flash; }
            set
            {
                _Flash = value;
                this.RaisePropertyChanged("Flash");
            }
        }

        private Color _ColorOn = Colors.Green;
        public Color ColorOn
        {
            get { return _ColorOn; }
        }

        private Color _ColorOff = Colors.Red;
        public Color ColorOff
        {
            get { return _ColorOff; }
        }


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


        private ushort _DIO_ID = 0;
        public ushort DIO_ID
        {
            get { return _DIO_ID; }
            set
            {
                if (_DIO_ID == value) return;
                
                _ini.SetValue("NETWORK_INFO", "USB7230", value.ToString());
                _DIO_ID = value;
                RaisePropertyChanged("DIO_ID");
            }
        }

        #endregion

        #region Method
        public void Connect()
        {
            _deviceUSB7230.Connect(DIO_ID);
            _timer.Interval = TimeSpan.FromMilliseconds(500);
            _timer.Tick += new EventHandler(Timer_Tick);
        }
        public void ReadPort()
        {
            uint StatusByte = 0;
            StatusByte= _deviceUSB7230.ReadPort();

            for (int i = 0; i < 16; i++)
            {
                bool isOnLED= Convert.ToBoolean((StatusByte >> (i)) & 1);
                LedStatus[i] = isOnLED;
            }

        }

        public void WritePort(uint sendByte)
        {
            //uint sendByte = 0x8000;
            bool status = _deviceUSB7230.WritePort(sendByte);
        }

        public void WriteLine(int index, bool OnOff)
        {
            ushort line = (ushort)index;
            ushort value = 0;
            if (OnOff) value = 1;
            else value = 0;

            bool status = _deviceUSB7230.WriteLine(line, value);
        }

        #endregion

        #region RelayCommand
        public RelayCommand StartCommand { get; set; }
        void StartProcess()
        {
            IsStartButtonEnabled = false;
            IsStopButtonEnabled = true;

            if (_deviceUSB7230.IsConnected)
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
        #endregion

        private void Timer_Tick(object sender, EventArgs e)
        {
            if(_isRunning)
            {
                ReadPort();
            }
        }



    }

 
}
