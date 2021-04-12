using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using CargoLampTest.Model;
using System.Windows.Data;
using AUA.AiS_FruiT;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Xml;
using AUA_FCT_EDITOR.XmlControl;
using System.Linq;
using log4net;
using System.Windows.Media;
using GalaSoft.MvvmLight.Messaging;

namespace CargoLampTest.ViewModel
{
    /// <summary>
    /// https://www.codeproject.com/Articles/323187/MVVMLight-Using-Two-Views
    /// </summary>
    public partial class HomeViewModel : ViewModelBase
    {
        private static readonly ILog pLogger = LogHelper.GetLoggerRollingFileAppender("Root");
        private static readonly ILog eLogger = LogHelper.GetLoggerRollingFileAppender("Exception");
        static public Equipment equipmentItem = null;
        static public string xmlFileName = Path.GetFullPath(@"ModelData\AUA_Setting_E-CALL.AUX");
        static EqSettingValue[] arrModel =
{
        };
        static public EqSettingValue _model = new EqSettingValue();
        public HomeViewModel()
        {
            HomeViewLoaded = new RelayCommand(HomeViewLoadedProcess, true);
            StartCommand = new RelayCommand(StartProcess, true);
            StopCommand = new RelayCommand(StopProcess, true);


            Models = new ObservableCollection<TestModel>();
            ReadXMLGetSetting();
            ModelSetup();
            GetTestListXML();

            //GridViewInit();

            SelectedModelIndex = 0;
            SystemStatus = "Ready";

        }
        #region property
        //TODO Binding Models.PartNumber 동작 안한다. => SelectedPartNumber를 바인딩해서 사용
        private string _partNumber;

        public string PartNumber
        {
            get { return _partNumber; } //TODO get이 발생안한다. 
            set { _partNumber = value; RaisePropertyChanged("PartNumber"); }
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

        private ObservableCollection<TestList> _testList;
        public ObservableCollection<TestList> TestLists
        {
            get { return _testList; }
            set
            {
                _testList = value;
                RaisePropertyChanged("TestLists");
            }
        }

        private ObservableCollection<TestField> _testFields = new ObservableCollection<TestField>();
        public ObservableCollection<TestField> TestFields
        {
            get { return _testFields; }
            set
            {
                _testFields = value;
                RaisePropertyChanged("TestFields");
            }
        }




        // Model 가져오기
        private ObservableCollection<TestModel> _models { get; set; }
        public ObservableCollection<TestModel> Models
        {
            get { return _models; }
            set
            {
                _models = value;
                RaisePropertyChanged("Models");
            }
        }

        private TestModel _selectedModel = new TestModel();
        public TestModel SelectedModel
        {
            get { return _selectedModel; }
            set
            {
                _selectedModel = value;
                RaisePropertyChanged("SelectedModel");
            }
        }

        private string _selectedPartNumber;

        public string SelectedPartNumber
        {
            get { return _selectedPartNumber; }
            set { _selectedPartNumber = value; RaisePropertyChanged("SelectedPartNumber"); }
        }



        private int _SelectedModelIndex;

        public int SelectedModelIndex
        {
            get { return _SelectedModelIndex; }
            set
            {
                _SelectedModelIndex = value;
                RaisePropertyChanged("SelectedModelIndex");
                SelectedIndexChanged(_SelectedModelIndex);
            }
        }


        private string systemStatus;

        public string SystemStatus
        {
            get { return systemStatus; }
            set { systemStatus = value; RaisePropertyChanged("SystemStatus"); }
        }


        private string _station1;

        public string Station1
        {
            get { return _station1; }
            set { _station1 = value; RaisePropertyChanged("Station1"); }
        }

        private string _operator;

        public string Operator
        {
            get { return _operator; }
            set { _operator = value; RaisePropertyChanged("Operator"); }
        }

        private string _cycleTime;
        public string CycleTime
        {
            get { return _cycleTime; }
            set { _cycleTime = value; RaisePropertyChanged("CycleTime"); }
        }

        private string _totalCount;
        public string TotalCount
        {
            get { return _totalCount; }
            set { _totalCount = value; RaisePropertyChanged("TotalCount"); }
        }

        private string _OKCount;
        public string OKCount
        {
            get { return _OKCount; }
            set { _OKCount = value; RaisePropertyChanged("OKCount"); }
        }

        private string _NGCount;
        public string NGCount
        {
            get { return _NGCount; }
            set { _NGCount = value; RaisePropertyChanged("NGCount"); }
        }

        private string _ScanBarcode;
        public string ScanBarcode
        {
            get { return _ScanBarcode; }
            set { _ScanBarcode = value; RaisePropertyChanged("ScanBarcode"); }
        }

        private string _OutputBarcode;
        public string OutputBarcode
        {
            get { return _OutputBarcode; }
            set { _OutputBarcode = value; RaisePropertyChanged("OutputBarcode"); }
        }


        private string _FinalResult;
        public string FinalResult
        {
            get { return _FinalResult; }
            set { _FinalResult = value; RaisePropertyChanged("FinalResult"); }
        }
        private string _TotalOKCount;
        public string TotalOKCount
        {
            get { return _TotalOKCount; }
            set { _TotalOKCount = value; RaisePropertyChanged("TotalOKCount"); }
        }
        private string _TotalNGCount;
        public string TotalNGCount
        {
            get { return _TotalNGCount; }
            set { _TotalNGCount = value; RaisePropertyChanged("TotalNGCount"); }
        }

        private string _PinBlockCount;
        public string PinBlockCount
        {
            get { return _PinBlockCount; }
            set { _PinBlockCount = value; RaisePropertyChanged("PinBlockCount"); }
        }

        private Brush _labelColor;

        public Brush LabelColor
        {
            get { return _labelColor; }//TODO get호출이 많다. 
            set { _labelColor = value; RaisePropertyChanged("LabelColor"); }
        }

        #endregion

        #region RelayCommand
        public RelayCommand HomeViewLoaded { get; set; }
        void HomeViewLoadedProcess()
        {
            //화면선택할때마다 동작한다.
        }
        public RelayCommand StartCommand { get; set; }
        void StartProcess()
        {
            //MainViewModel에서 Thread동작할 수 있는 플래그 활성한다.
            Program.IsRunSW = true;
            IsStartButtonEnabled = false;
            IsStopButtonEnabled = true;
            SystemStatus = "AUTO";
        }
        public RelayCommand StopCommand { get; set; }
        void StopProcess()
        {
            Program.IsRunSW = false;
            IsStartButtonEnabled = true;
            IsStopButtonEnabled = false;
            SystemStatus = "STOP";
        }
        #endregion

        #region Methode


        public void GridViewInit()
        {
            TestList temp = new TestList();
            for (int i = 0; i < Program.MaxRowCnt; i++)
            {
                TestFields.Add(new TestField()
                {
                    TestNumber = Convert.ToByte(temp.testField[i].no),
                    TestItem = temp.testField[i].item,
                    DetailItem = temp.testField[i].detailItem,
                    Low = temp.testField[i].specMin,
                    High = temp.testField[i].specMax,
                    MeasureValue = "-",
                    Result = "-",
                });
            }

        }

        public void ReadXMLGetSetting()
        {
            try
            {
                equipmentItem = null;
                equipmentItem = new Equipment();

                XmlRead.ReadXmlFile(xmlFileName, equipmentItem);

                //-- 장비정보
                Program.MainTitle = equipmentItem.Name;
                Messenger.Default.Send<NotificationMessage, MainViewModel>(new NotificationMessage(this, Program.MainTitle));
                EqSettingValue[] eqSettingValue = new EqSettingValue[equipmentItem.SettingValue.Count];



                Array.Resize(ref arrModel, equipmentItem.SettingValue.Count);
                Array.Clear(arrModel, 0, equipmentItem.SettingValue.Count);

                for (int i = 0; i < equipmentItem.SettingValue.Count; i++)
                {
                    arrModel[i] = new EqSettingValue();
                    eqSettingValue[i] = equipmentItem.SettingValue[i];
                    arrModel[i] = equipmentItem.SettingValue[i];
                }


                // 빈 배열은 지워준다. 
                arrModel = arrModel.Where(c => c.SetName != null).ToArray();

                //_isSimulation = (GetIni("Simulation", Program.equipmentItem.IniValue.Setting.ToString()) == "YES") ? true : false;
            }
            catch (Exception ex)
            {
                eLogger.Error(ex.ToString());
            }
            finally
            {
            }

        }

        public void ModelSetup()
        {

            var models = from Model in arrModel
                         select Model;

            foreach (var Model in models)
            {
                Models.Add(new TestModel() { Name = Model.SetName, PartNumber = Model.SetModelNumber, });
            }
        }

        private void SelectedIndexChanged(int selectedModelIndex)
        {

            int selectedIndex = selectedModelIndex;

            //TODO xml을 using을 사용해야 아래에서 오픈할때 Exception이 발생하지 않는다. 
            //ReadXMLGetSetting();
            //ModelSetup();

            //if (CheckLicenseCount(selectedIndex))
            //{
            //}
            //else
            //{
            //    _SelectedModelIndex = 0;
            //}
            //선택한 모델에 대한 이름을 화면에 보여준다.
            try
            {
                var result2 = arrModel[selectedIndex];
                SelectedPartNumber = result2.SetModelNumber;
                //Program.modelName = result2.SetName;
                Program.ModelNo = result2.SetModelNumber;
                //Program._BCR_Check = result2.SetBCRCheck;
                var converter = new System.Windows.Media.BrushConverter();
                LabelColor = (Brush)converter.ConvertFromString(result2.SetLabelColor);
                for (int i = 0; i < Program.SiteCount; i++)
                {
                    GetTestListXML();
                }
            }
            catch (Exception ex)
            {

                eLogger.Error(ex.ToString());
            }

        }
        public void GetTestListXML()
        {
            XmlDocument xml = new XmlDocument();

            if (Program.ModelNo == "") xml.Load(Path.GetFullPath(@"ModelData\Sample.xml"));
            else xml.Load(Path.GetFullPath(@"ModelData\" + Program.ModelNo + ".xml"));

            XmlNodeList xnList = xml.GetElementsByTagName("TestItem");

            TestFields.Clear();
            foreach (XmlNode xnNode in xnList)
            {
                TestFields.Add(new TestField()
                {
                    TestNumber = Convert.ToByte(xnNode.ChildNodes[0].InnerText),
                    TestItem = xnNode.ChildNodes[1].InnerText,
                    DetailItem = xnNode.ChildNodes[2].InnerText,
                    Low = xnNode.ChildNodes[3].InnerText,
                    High = xnNode.ChildNodes[4].InnerText,
                    MeasureValue = xnNode.ChildNodes[5].InnerText,
                    Result = xnNode.ChildNodes[6].InnerText,
                });
            }


        }
        #endregion







    }
}
