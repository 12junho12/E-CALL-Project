using AUA.AiS_FruiT;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Windows.Threading;
using System.Windows;
using System.Windows.Media;
using static AUA.AiS_FruiT.DeviceBCR;
using System.Threading.Tasks;
using System.IO.Ports;

using log4net;
using log4net.Config;
using System.IO;
using System.Collections.ObjectModel;
using System.Collections;
using System.Windows.Controls;
using System.Collections.Generic;
using CommonServiceLocator;
using GalaSoft.MvvmLight.Messaging;
// 프로그램 내 한번만 지정
[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace CargoLampTest.ViewModel
{

    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public partial class MainViewModel : ViewModelBase
    {
        private static readonly Ini _ini = new Ini(Path.GetFullPath("env.ini"));
        private static readonly ILog pLogger = LogHelper.GetLoggerRollingFileAppender("Root");
        private static readonly ILog eLogger = LogHelper.GetLoggerRollingFileAppender("Exception");
        private static readonly ILog rLogger = LogHelper.GetLoggerRollingFileAppender("Result");

        private SQLiteDataBase _db = new SQLiteDataBase();
        private DispatcherTimer _timerStatus = new DispatcherTimer();
        private ViewModelBase _currentViewModel;


        private  HomeViewModel _HomeViewModel;
        private  SettingViewModel _SettingViewModel;
        private  CurrentMeterViewModel _CurrentMeterViewModel;
        private static USB7230ViewModel _USB7230ViewModel;
        private PTC04ViewModel _PTC04ViewModel;
      
        string _resultFilePath = "";
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            pLogger.Info("Start MainViewModel");

            //MessengerInstance.Register<NotificationMessage>(this, NotifyFromHomeViewModel);
            Messenger.Default.Register<NotificationMessage>(this, NotifyFromHomeViewModel);

            ExitCommand = new RelayCommand(ExitProcess, true);
            MainWindowsLoaded = new RelayCommand(MainWindowsLoadedProcess, true);
            SelectedItemChangedCommand = new RelayCommand(SelectedItemChangedProcess, true);


            //아래 방식으로 하면, intance가 없으면 생성하고, 있으면 기존것을 가지고 사용한다. 클래스에서 바로 생성한것과 다름
            _HomeViewModel = ServiceLocator.Current.GetInstance<HomeViewModel>();
            _SettingViewModel = ServiceLocator.Current.GetInstance<SettingViewModel>();
            _CurrentMeterViewModel = ServiceLocator.Current.GetInstance<CurrentMeterViewModel>();
            _USB7230ViewModel = ServiceLocator.Current.GetInstance<USB7230ViewModel>();
            _PTC04ViewModel= ServiceLocator.Current.GetInstance<PTC04ViewModel>();

            CurrentViewModel = _HomeViewModel;



            _timerStatus.Interval = TimeSpan.FromMilliseconds(500);
            _timerStatus.Tick += new EventHandler(TimerStatus_Tick);
            _timerStatus.Start();

            Start(); //Thread start

            Init();

        }

        private void NotifyFromHomeViewModel(NotificationMessage obj)
        {
            MainTitle = obj.Notification;
        }

        #region Property
        private string _mainTitle;
        public string MainTitle
        {
            get { return _mainTitle; }
            set { _mainTitle = value; RaisePropertyChanged("MainTitle"); }
        }

        private int _selectedIndex = 0;
        public int SelectedIndex
        {
            get
            {
                return _selectedIndex;
            }

            set
            {
                if (_selectedIndex == value)
                {
                    return;
                }
                _selectedIndex = value;
                RaisePropertyChanged("SelectedIndex");


                //RaisePropertyChanged(() => SelectedIndex);
            }
        }
        private Thickness _gridCursorMargin = new Thickness(0, 10, 0, 0);
        public Thickness GridCursorMargin
        {
            get
            {
                return _gridCursorMargin;
            }
            set
            {
                _gridCursorMargin = value;
                RaisePropertyChanged("GridCursorMargin");
            }
        }

        public ViewModelBase CurrentViewModel
        {
            get
            {
                return _currentViewModel;
            }
            set
            {
                if (_currentViewModel == value)
                    return;
                _currentViewModel = value;
                RaisePropertyChanged("CurrentViewModel");

            }
        }
        private string _currentTime;
        public string CurrentTime
        {
            get { return _currentTime; }
            set { _currentTime = value; RaisePropertyChanged("CurrentTime"); }
        }

        //UI에 보여주기위해서
        private string _productBCR;
        public string ProductBCR
        {
            get { return _productBCR; }
            set { _productBCR = value; RaisePropertyChanged("ProductBCR1234567890"); }
        }
        //UI에 보여주기전 필터링
        private string _scannedBCR;
        public string ScannedBCR
        {
            get { return _scannedBCR; }
            set
            {
                _scannedBCR = value;
                RaisePropertyChanged("ScannedBCR");
                if ((_scannedBCR.Contains("OK")) || (_scannedBCR.Contains("NG")))
                {
                    //if (_deviceLeCroy.IsConnected && _deviceAFG.IsConnected && _deviceBCR.IsConnected)
                    {
                        ResultProcess();
                    }
                    //else
                    {
                        //TODO : 연결이 안되어 있으때 예외처리...
                    }
                }
                else if (_scannedBCR.Contains("Connected"))
                { }
                else
                {

                    ResultIsEnalbed = true;
                    ResultBackgroundColor = Brushes.PaleGoldenrod;
                    ProductBCR = _scannedBCR;
                    //_deviceAFG.OutputOn();
                    //_deviceLeCroy.LeCroyString("vbs 'app.Acquisition.TriggerMode = \"Auto\"'");
                }

            }
        }

        private bool _resultIsEnalbed = false;
        public bool ResultIsEnalbed
        {
            get { return _resultIsEnalbed; }
            set
            {
                _resultIsEnalbed = value;
                RaisePropertyChanged("ResultIsEnalbed");
            }
        }

        private Brush _resultBackgroundColor = Brushes.Gray;
        public Brush ResultBackgroundColor
        {
            get { return _resultBackgroundColor; }
            set
            {
                _resultBackgroundColor = value;
                RaisePropertyChanged("ResultBackgroundColor");
            }
        }



        #endregion

        #region RelayCommand
        public RelayCommand SelectedItemChangedCommand { get; set; }
        void SelectedItemChangedProcess()
        {
            //_VoltMeterViewModel = (new ViewModelLocator()).VoltMeter;
            

            GridCursorMargin = new Thickness(0, (10 + (80 * SelectedIndex)), 0, 0);
            switch (SelectedIndex)
            {
                case 0:
                    CurrentViewModel = _HomeViewModel;
                    break;
                case 1:
                    CurrentViewModel = _SettingViewModel;
                    break;
                
                case 2:
                    CurrentViewModel = _CurrentMeterViewModel;
                    break;
                case 3:
                    CurrentViewModel = _USB7230ViewModel;
                    break;
                default:
                    CurrentViewModel = _PTC04ViewModel;
                    break;
            }

        }

        public RelayCommand ExitCommand { get; set; }
        void ExitProcess()
        {
            //if (_deviceBCR != null)
            //{
            //    _deviceBCR.Close();
            //    EventClearHelper.RemoveAllEventHandlers(_deviceBCR);
            //}
            //if (_deviceAFG != null)
            //{
            //    _deviceAFG.Close();
            //}
            Stop();
            pLogger.Info("Exit Program");
            Application.Current.MainWindow.Close();
        }

        public RelayCommand MainWindowsLoaded { get; set; }
        void MainWindowsLoadedProcess()
        {
            //serial접속을 인스턴스 시작하는곳에 배치하면 접속 에러가 가끔 발생해서 여기로 이동함.
            //_scopeIP = _ini.GetValue("NETWORK_INFO", "SCOPE");
            //_serialPortNumberBCR = Convert.ToInt16(_ini.GetValue("NETWORK_INFO", "BCR"));
            //_serialPortNumberFGen = Convert.ToInt16(_ini.GetValue("NETWORK_INFO", "FGEN "));

            //_deviceBCR.Connect(_serialPortNumberBCR);
            //_deviceBCR.BarcodeReceived += new MessageChangedHandler(BarcodeProcess);

            //_deviceAFG.Connect(_serialPortNumberFGen);
            //_deviceAFG.ClearAFG();

            //_deviceLeCroy.Create(_scopeIP);
            //_deviceLeCroy.LeCroyRecall(1);

            _resultFilePath = _ini.GetValue("FOLDER_INFO", "TEST_RESULT_FILE");

        }
        #endregion

        #region Method

        #endregion

        private void ResultProcess()
        {
            string defaultPath = _resultFilePath;
            defaultPath += "\\" + DateTime.Now.Year.ToString("#0000") + "\\" + DateTime.Now.Month.ToString("#00") + "\\" + DateTime.Now.Day.ToString("#00") + "\\";
            defaultPath.Replace("\\\\", "\\");
            Directory.CreateDirectory(defaultPath);

            string dt = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string barcode = ProductBCR?.Trim();
            string fullPath = "";
            string fileName = "";

            //_deviceLeCroy.LeCroyString("vbs 'app.Acquisition.TriggerMode = \"Stop\"'");
            //_deviceAFG.OutputOff();
            double measureValueC1 = 0;
            double measureValueC2 = 0;
            double measureValueC3 = 0;
            double measureValueF1 = 0;

            string result = $" / Barcode : {barcode} / C1 : {measureValueC1} / C2 : {measureValueC2} / C3 : {measureValueC3} / F1 : { measureValueF1}";



            if (ScannedBCR.Contains("OK"))
            {
                //화면 변경
                fileName = $"{barcode}_{dt}_OK.JPG";
                fullPath = defaultPath + fileName;
                //_deviceLeCroy.SaveWaveImage(fullPath);



                ResultBackgroundColor = Brushes.PaleGreen;
                ResultSearch rs = new ResultSearch
                {
                    WORK_DATE = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    MODEL = "CargoLamp",
                    SCAN_BARCODE = barcode,
                    OUT_BARCODE = "-",
                    RESULT = "OK",
                    ID = "1",
                    NAME = "master",
                    MEAS1 = measureValueC1.ToString(),
                    MEAS2 = measureValueC2.ToString(),
                    MEAS3 = measureValueC3.ToString(),
                    MEAS4 = measureValueF1.ToString(),
                    MEAS5 = ""
                };
                SaveResultData(rs);
                pLogger.Info("Result OK");
                rLogger.Info("Result OK" + result);
            }
            else if (ScannedBCR.Contains("NG"))
            {
                fileName = $"{barcode}_{dt}_NG.JPG";
                fullPath = defaultPath + fileName;
                //_deviceLeCroy.SaveWaveImage(fullPath);


                ResultBackgroundColor = Brushes.OrangeRed;
                ResultSearch rs = new ResultSearch
                {
                    WORK_DATE = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    MODEL = "CargoLamp",
                    SCAN_BARCODE = barcode,
                    OUT_BARCODE = "-",
                    RESULT = "NG",
                    ID = "1",
                    NAME = "master",
                    MEAS1 = measureValueC1.ToString(),
                    MEAS2 = measureValueC2.ToString(),
                    MEAS3 = measureValueC3.ToString(),
                    MEAS4 = measureValueF1.ToString(),
                    MEAS5 = ""
                };
                SaveResultData(rs);
                pLogger.Info("Result NG");
                rLogger.Info("Result NG" + result);

            }
            else
            {
                ResultBackgroundColor = Brushes.Gray;
                pLogger.Info("Result Unknown");
                rLogger.Info(barcode + "Result Unknown");
            }
        }



    }
}