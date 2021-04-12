using System.Xml;
using System.Data;
using CargoLampTest.Model;
using System.IO;
using System.Collections.ObjectModel;
using System.ComponentModel;
using GalaSoft.MvvmLight;

namespace AUA.AiS_FruiT
{
    public class TestField : ViewModelBase
    {

        public string no = "-";
        public string item = "-";
        public string detailItem = "-";
        public string specMin = "";
        public string specMax = "";
        public string measurement = "-";
        public string result = "-";


        #region property
        private byte _testNumber=0;
        public byte TestNumber
        {
            get { return _testNumber; }
            set
            {
                _testNumber = value;
                RaisePropertyChanged("TestNumber");
            }
        }

        private string _testItem = "-";
        public string TestItem
        {
            get { return _testItem; }
            set
            {
                _testItem = value;
                RaisePropertyChanged("TestItem");
            }
        }
        private string _detailItem = "-";
        public string DetailItem
        {
            get { return _detailItem; }
            set
            {
                _detailItem = value;
                RaisePropertyChanged("DetailItem");
            }
        }
        private string _low = "-";
        public string Low
        {
            get { return _low; }
            set
            {
                _low = value;
                RaisePropertyChanged("Low");
            }
        }
        private string _measureValue = "-";
        public string MeasureValue
        {
            get { return _measureValue; }
            set
            {
                _measureValue = value;
                RaisePropertyChanged("MeasureValue");
            }
        }
        private string _high = "-";
        public string High
        {
            get { return _high; }
            set
            {
            _high = value;
                RaisePropertyChanged("High");
            }
        }
        private string _result = "-";
        public string Result
        {
            get { return _result; }
            set
            {
                _result = value;
                RaisePropertyChanged("Result");
            }
        }
        #endregion


        public void Clear()
		{
			
		}
	}

    public class TestCondition
    {
        public string Param1 { get; set; } = "-";  //peak
        public string Param2 { get; set; } = "-";  //drop
        public string Param3 { get; set; } = "-";  //control
        public string Enable { get; set; } = "-"; // test enable = True,  disable =False

        public string param1 = "-";  //peak
        public string param2 = "-";  //drop
        public string param3 = "-";  //control
        public string enable = "-"; // test enable = True,  disable =False
    }

	public class TestList
	{

        public TestField TestField { get; set; }
        public TestCondition TestCondition { get; set; }


        public TestField[] testField;
        public TestCondition[] testCondition;
        public TestList()
        {
            //GetXML();
        }

        public void Clear()
        {
            for (int i = 0; i < Program.MaxRowCnt; i++)
            {
                testField[i].measurement = "-";
                testField[i].result = "";
            }
        }
       
    }


}
