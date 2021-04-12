using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using AUA.AiS_FruiT;

namespace CargoLampTest.ViewModel
{
    public partial class MainViewModel
    {
        private void Init()
        {
            MenuForegroundColor[0] = Brushes.Gray; // home
            MenuForegroundColor[1] = Brushes.Gray; // setting
            MenuForegroundColor[2] = Brushes.Gray; //current
            MenuForegroundColor[3] = Brushes.Gray; //digital
            MenuForegroundColor[4] = Brushes.Gray; //label

        }
        private void ConnectionChanged(object args)
        {
            IsConnectedVoltMeter = (bool)args;
            
        }

        public bool IsConnectedVoltMeter { get; set; }
        public bool IsConnectedHome { get; private set; }
        public bool IsConnectedSetting { get; private set; }
        public bool IsConnectedCurrentMeter { get; private set; }
        public bool IsConnectedDIO { get; private set; }
        public bool IsConnectedPTC04 { get; private set; }

        private void TimerStatus_Tick(object sender, EventArgs e)
        {
            CurrentTime = DateTime.Now.ToString("MM-dd HH:mm:ss");
            _timerStatus.Stop();
            MainProcess();

            IsConnectedHome = (_CurrentMeterViewModel._deviceMT4N.IsConnected) && (_USB7230ViewModel._deviceUSB7230.IsConnected) && (_PTC04ViewModel._devicePTC04.IsConnected);

            MenuForegroundColor[0] = (IsConnectedHome) ? Brushes.PaleGreen : Brushes.OrangeRed;

            MenuForegroundColor[1] = (IsConnectedSetting) ? Brushes.PaleGreen : Brushes.OrangeRed;

            MenuForegroundColor[2] = (_CurrentMeterViewModel._deviceMT4N.IsConnected) ? Brushes.PaleGreen : Brushes.OrangeRed;

            MenuForegroundColor[3] = (_USB7230ViewModel._deviceUSB7230.IsConnected) ? Brushes.PaleGreen : Brushes.OrangeRed;

            MenuForegroundColor[4] = (_PTC04ViewModel._devicePTC04.IsConnected) ? Brushes.PaleGreen : Brushes.OrangeRed;

            _timerStatus.Start();
        }

        private ObservableCollection<Brush> menuForegroundColor = new ObservableCollection<Brush>
            {
                Brushes.LightGray,Brushes.LightGray,Brushes.LightGray,Brushes.LightGray,Brushes.LightGray, //6개 메뉴가 있다. 

            };
        public ObservableCollection<Brush> MenuForegroundColor
        {
            get { return menuForegroundColor; }
            set
            {
                menuForegroundColor = value;
                RaisePropertyChanged("MenuForegroundColor");
            }
        }


    }
}
