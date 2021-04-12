using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using AUA_FCT_EDITOR.XmlControl;
using System.IO;
using log4net;

namespace AUA.AiS_FruiT
{
    public partial class DeviceLABEL
    {

        private SerialPort _serialPort = new SerialPort();
        public bool _isReceived = false;
        public string _receiveData = "";
        private string _receiveDataBuffer = "";
        #region  아스키 정의
        public const char ASCII_NUL = (char)0x00;
        public const char ASCII_SOH = (char)0x01;
        public const char ASCII_STX = (char)0x02;
        public const char ASCII_ETX = (char)0x03;
        public const char ASCII_EOT = (char)0x04;
        public const char ASCII_ENQ = (char)0x05;
        public const char ASCII_ACK = (char)0x06;
        public const char ASCII_BEL = (char)0x07;
        public const char ASCII_BS = (char)0x08;
        public const char ASCII_HT = (char)0x09;
        public const char ASCII_LF = (char)0x0A;
        public const char ASCII_VT = (char)0x0B;
        public const char ASCII_FF = (char)0x0C;
        public const char ASCII_CR = (char)0x0D;
        public const char ASCII_SO = (char)0x0E;
        public const char ASCII_SI = (char)0x0F;

        public const char ASCII_DLE = (char)0x10;
        public const char ASCII_DC1 = (char)0x11;
        public const char ASCII_DC2 = (char)0x12;
        public const char ASCII_DC3 = (char)0x13;
        public const char ASCII_DC4 = (char)0x14;
        public const char ASCII_NAK = (char)0x15;
        public const char ASCII_SYN = (char)0x16;
        public const char ASCII_ETB = (char)0x17;
        public const char ASCII_CAN = (char)0x18;
        public const char ASCII_EM = (char)0x19;
        public const char ASCII_SUB = (char)0x1A;
        public const char ASCII_ESC = (char)0x1B;
        public const char ASCII_FS = (char)0x1C;
        public const char ASCII_GS = (char)0x1D;
        public const char ASCII_RS = (char)0x1E;
        public const char ASCII_US = (char)0x1F;
        #endregion

        private static readonly Ini _ini = new Ini(Path.GetFullPath("env.ini"));
        private static readonly ILog pLogger = LogHelper.GetLoggerRollingFileAppender("Root");
        private static readonly ILog eLogger = LogHelper.GetLoggerRollingFileAppender("Exception");
        StringBuilder strPrinters = new StringBuilder(4096);


        //xml에서 가져올 데이타 담는 그릇
        string zplData = "";  //바코드에 보낼 전체 프로토콜 데이타.
        int startPrintNumber = 1;
        string labelData = "";  //라벨 텍스트 데이타
        int count = 1;  //라벨 출력 갯수 1부터 n까지..
        int fontSize = 0;
        int labelFontSize = 0;
        public bool IsConnected { get; set; } = false;
        EqSettingValue model = new EqSettingValue();
        

        public bool Connect(int portName, int baudRate = (int)9600, int DataBits = (int)8, Parity parity = Parity.None, StopBits stopBits = StopBits.One, Handshake handshake = Handshake.None, int readTimeout = 1000, int writeTimeout = 1000)
        {
            _serialPort.PortName = "COM" + portName.ToString();
            _serialPort.BaudRate = baudRate;
            _serialPort.DataBits = DataBits;
            _serialPort.Parity = parity;
            _serialPort.StopBits = stopBits;
            _serialPort.Handshake = handshake;
            _serialPort.ReadTimeout = readTimeout;
            _serialPort.WriteTimeout = writeTimeout;

            try
            {
                _serialPort.Open();
            }
            catch (Exception ex)
            {
                eLogger.Error(ex.ToString());
                IsConnected = false;
                return false;
            }

            _serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);

            if (_serialPort.IsOpen)
            {
                IsConnected = true;

                return true;
            }
            else
            {
                IsConnected = false;

                return false;
            }
        }

        public void Close()
        {
            IsConnected = false;
            _serialPort.Close();
        }

        void DataReceivedHandler(Object sender, SerialDataReceivedEventArgs e)
        {
            //ReadByte, ReadChar, ReadExisting, ReadLine 
            SerialPort serialPort = (SerialPort)sender;
            string receiveData = serialPort.ReadExisting();
            _receiveDataBuffer += receiveData;

            if (_receiveDataBuffer.IndexOf((char)0x0A) > -1)
            {
                _receiveData = _receiveDataBuffer;
                _receiveDataBuffer = "";

                ReceiveData(_receiveData);
                _isReceived = true;
            }
        }

        public void Clear()
        {
            _isReceived = false;
            _receiveData = "";
        }

        public int Send(string message)
        {
            try
            {
                if (_serialPort.IsOpen)
                {
                    _serialPort.Write(message);

                    return message.Length;
                }
                else
                {
                    return -1;
                }
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public virtual void ReceiveData(string receiveData)
        {

        }

        public int GetLabelSerial(string modelName)
        {
            string today = DateTime.Now.ToString("yyyyMMdd");
            string lastDate = _ini.GetValue("LABEL_SERIAL_INFO", "LAST_DATE");

            if (today != lastDate)
            {
                _ini.SetValue("LABEL_SERIAL_INFO", "LAST_DATE", today);
                List<string> keys = _ini.GetKeys("LABEL_SERIAL_INFO");
                foreach (string category in keys)
                {
                    if (category == "LAST_DATE") { }
                    else
                        _ini.SetValue("LABEL_SERIAL_INFO", category, "1");
                }
            }



            if (_ini.GetValue("LABEL_SERIAL_INFO", modelName).Equals(""))
            {
                _ini.SetValue("LABEL_SERIAL_INFO", modelName, "1");
            }
            int labelSerial = Convert.ToInt32(_ini.GetValue("LABEL_SERIAL_INFO", modelName));
            _ini.SetValue("LABEL_SERIAL_INFO", modelName, (labelSerial + 1).ToString());

            return labelSerial;
        }

        public string Zebra1D(string modelName, string printType)
        {

            labelData = "";

            //LabelType = "YD",
            //선택한 모델에 대한 이름을 화면에 보여준다.
            GetLabelInfo(modelName, printType);
            switch (model.SetLabelType)
            {
                case "YD":
                    labelData = Zebra1D_Common(modelName, printType);
                    break;

                case "RP":
                    labelData = Zebra1D_Common(modelName, printType);
                    break;

                case "QL":
                    labelData = Zebra1D_Common(modelName, printType);
                    break;
                case "HI":
                    labelData = Zebra1D_Common(modelName, printType);
                    break;
                case "IG":
                    labelData = Zebra1D_Common(modelName, printType);
                    break;

                default:
                    labelData = Zebra1D_Common(modelName, printType);
                    break;
            }
            return labelData;
        }

        public string Zebra2D(string modelName, string printType)
        {
            //TODO model = Program._model;

            labelData = "";

            //LabelType = "YD",
            //선택한 모델에 대한 이름을 화면에 보여준다.
            GetLabelInfo(modelName, printType);
            switch (model.SetLabelType)
            {

                case "YDm":
                    labelData = Zebra2D_Common(modelName, printType);
                    break;

                case "RP":
                    labelData = Zebra2D_Common(modelName, printType);
                    break;
                case "QL":
                    labelData = Zebra2D_Common(modelName, printType);
                    break;
                case "HI":
                    labelData = Zebra2D_Common(modelName, printType);
                    break;
                case "IG":
                    labelData = Zebra2D_Common(modelName, printType);
                    break;


                default:
                    labelData = Zebra2D_Common(modelName, printType);
                    break;
            }
            return labelData;
        }

        public string Zebra1D_Common(string modelName, string printType)
        {
            string SerialNumber = "";
            try
            {
                GetLabelInfo(modelName, printType);
                for (int i = 0; i < count; i++)
                {
                    labelData = "";
                    //라벨 작성
                    //1.HW버젼
                    labelData += model.HW;
                    labelData += " "; //QL만 space를 넣어준다. 
                    //2. S/W 버전
                    labelData += model.SW;
                    //3.차종  HIPSD
                    labelData += model.CAR_CODE;

                    //4.연 O=> 2015년
                    if (model.YEAR == "숫자")
                        labelData += DateTime.Now.Year.ToString("#00");
                    else
                        labelData += (char)('A' + DateTime.Now.Year - 2001);
                    //5. 월 E => 5월
                    if (model.MONTH == "숫자")
                        labelData += DateTime.Now.Month.ToString("#00");
                    else
                        labelData += (char)('A' + DateTime.Now.Month - 1);
                    //6. 일 
                    if (model.DAY == "숫자")
                    {
                        labelData += DateTime.Now.Day.ToString("#00");
                    }
                    else
                    {
                        if (DateTime.Now.Day <= 26)
                            labelData += (char)('A' + DateTime.Now.Day - 1);
                        else
                            labelData += (DateTime.Now.Day - 26).ToString();
                    }
                    //7. 제품 일련번호
                    if (printType == "sample")
                    {
                        SerialNumber = (startPrintNumber + i).ToString("#0000");
                        labelData += SerialNumber;
                        System.Threading.Thread.Sleep(50);

                    }
                    else
                    {
                        SerialNumber = GetLabelSerial(modelName).ToString("#0000");
                        labelData += SerialNumber;
                    }

                    /////////////////////////////////////////
                    /// 화면에 뿌려질 라벨 정리

                    labelFontSize = fontSize + 10;
                    if (model.DARKNESS == null || model.DARKNESS == "") { model.DARKNESS = "20"; }
                    //width Values: 1 to 10
                    if (model.WIDTH == null || model.WIDTH == "") { model.WIDTH = "2"; }

                    zplData = "";
                    zplData += "^XA";
                    zplData += "^PRA,A,A";  //프린트 속도
                    zplData += "~SD" + model.DARKNESS;  //프린트 Set Darkness



                    //품번-품명-LotNo-바코드문자-1D바코드-2D바코드-HWChar-SWChar-HW-SW순으로
                    //상단 P/N문자  제일 첫번째 ~ 가 index 0이고, 맞는것이 없으면 -1이 리턴된다. 
                    //품번
                    if ((model.LabelNumberPosX.IndexOf("~") == -1))
                    {
                        zplData += "^FO" + model.LabelNumberPosX + "," + model.LabelNumberPosY + "^A0N," + labelFontSize + "," + labelFontSize + "^FD" + model.LabelNumber + "^FS";
                    }

                    //품명
                    if ((model.LabelNamePosX.IndexOf("~") == -1))
                    {
                        zplData += "^FO" + model.LabelNamePosX + "," + model.LabelNamePosY + "^A0N," + labelFontSize + "," + labelFontSize + "^FD" + model.LabelName + "^FS";
                    }
                    //LotNo
                    if ((model.LotNoCharPosX.IndexOf("~") == -1))
                    {
                        zplData += "^FO" + model.LotNoCharPosX + "," + model.LotNoCharPosY + "^A0N," + labelFontSize + "," + labelFontSize + "^FD" + model.LotNoChar + "^FS";
                    }
                    //바코드문자
                    if ((model.BarcodeTextPosX.IndexOf("~") == -1))
                    {
                        zplData += "^FO" + model.BarcodeTextPosX + "," + model.BarcodeTextPosY + "^A0N," + fontSize + "," + fontSize + "^FD" + labelData + "^FS";
                    }

                    // HW , HW Char
                    if ((model.HW.IndexOf("~") == -1))
                    {
                        if ((model.HWChar.IndexOf("~") == -1))
                            zplData += "^FO" + model.HWPosX + "," + model.HWPosY + "^A0N," + model.FONT_SIZE + "," + model.FONT_SIZE + "^FD" + "H/W : " + model.HW + "^FS";
                        else
                            zplData += "^FO" + model.HWPosX + "," + model.HWPosY + "^A0N," + model.FONT_SIZE + "," + model.FONT_SIZE + "^FD" + model.HW + "^FS";
                    }
                    // SW, SW Char
                    if ((model.SW.IndexOf("~") == -1))
                    {
                        if ((model.SWChar.IndexOf("~") == -1))
                            zplData += "^FO" + model.SWPosX + "," + model.SWPosY + "^A0N," + model.FONT_SIZE + "," + model.FONT_SIZE + "^FD" + "S/W : " + model.SW + "^FS";
                        else
                            zplData += "^FO" + model.SWPosX + "," + model.SWPosY + "^A0N," + model.FONT_SIZE + "," + model.FONT_SIZE + "^FD" + model.SW + "^FS";
                    }


                    // Trace Description
                    if ((model.TracePosX.IndexOf("~") == -1))
                    {
                        if ((model.TRACE_CODE == "") || (model.TRACE_CODE.IndexOf("~") == -1))
                        {
                            zplData += "^FO" + model.TracePosX + "," + model.TracePosY + "^A0N," + "25" + "," + "25";
                            zplData += "^FD";
                            zplData += (DateTime.Now.Year - 2000).ToString("#00");
                            zplData += DateTime.Now.Month.ToString("#00");
                            zplData += DateTime.Now.Day.ToString("#00");
                            //zplData += SerialNumber;
                            zplData += "^FS";
                        }
                        else zplData += model.TRACE_CODE;
                    }
                    // Divisition Information
                    if ((model.DivisionPosX.IndexOf("~") == -1))
                    {
                        zplData += "^FO" + model.DivisionPosX + "," + model.DivisionPosY + "^A0N," + "25" + "," + "25" + "^FD" + model.CAR_CODE + "^FS";
                    }


                    //1D.바코드
                    zplData += "^FO" + model.D1PosX + "," + model.D1PosY + "^BY" + model.WIDTH + ",2.0,1^BCN,35,N,N,N,N,A^FD" + labelData + "^FS";
                    zplData += "^XZ";

                    Send(zplData);


                }

            }//try

            catch (Exception ex)
            {
                eLogger.Error(ex.ToString());
            }

            return labelData;
        }

        public string Zebra2D_Common(string modelName, string printType)
        {
            try
            {
                string SerialNumber = "";
                GetLabelInfo(modelName, printType);
                for (int i = 0; i < count; i++)
                {
                    //ZPL 작성
                    labelData = "";
                    labelData += model.CAR_CODE;

                    //4.연 O=> 2015년
                    if (model.YEAR == "숫자")
                        labelData += DateTime.Now.Year.ToString("#00");
                    else
                        labelData += (char)('A' + DateTime.Now.Year - 2001);
                    //5. 월 E => 5월
                    if (model.MONTH == "숫자")
                        labelData += DateTime.Now.Month.ToString("#00");
                    else
                        labelData += (char)('A' + DateTime.Now.Month - 1);
                    //6. 일 
                    if (model.DAY == "숫자")
                    {
                        labelData += DateTime.Now.Day.ToString("#00");
                    }
                    else
                    {
                        if (DateTime.Now.Day <= 26)
                            labelData += (char)('A' + DateTime.Now.Day - 1);
                        else
                            labelData += (DateTime.Now.Day - 26).ToString();
                    }
                    //7. 제품 일련번호
                    if (printType == "sample")
                    {
                        SerialNumber = (startPrintNumber + i).ToString("#0000");
                        labelData += SerialNumber;
                        System.Threading.Thread.Sleep(500);

                    }
                    else
                    {
                        SerialNumber = GetLabelSerial(modelName).ToString("#0000");
                        labelData += SerialNumber;
                    }

                    /////////////////////////////////////////
                    /// 화면에 뿌려질 라벨 정리

                    labelFontSize = fontSize + 10;
                    if (model.DARKNESS == null || model.DARKNESS == "") { model.DARKNESS = "20"; }
                    //width Values: 1 to 10
                    if (model.WIDTH == null || model.WIDTH == "") { model.DARKNESS = "2"; }

                    zplData = "";
                    zplData += "^XA";
                    zplData += "^PRA,A,A";  //프린트 속도
                    zplData += "~SD" + model.DARKNESS;  //프린트 Set Darkness



                    //품번-품명-LotNo-바코드문자-1D바코드-2D바코드-HWChar-SWChar-HW-SW순으로
                    //상단 P/N문자  제일 첫번째 ~ 가 index 0이고, 맞는것이 없으면 -1이 리턴된다. 
                    //품번
                    if ((model.LabelNumberPosX.IndexOf("~") == -1))
                    {
                        zplData += "^FO" + model.LabelNumberPosX + "," + model.LabelNumberPosY + "^A0N," + labelFontSize + "," + labelFontSize + "^FD" + model.SetModelNumber + "^FS";
                    }

                    //품명
                    if ((model.LabelNamePosX.IndexOf("~") == -1))
                    {
                        zplData += "^FO" + model.LabelNamePosX + "," + model.LabelNamePosY + "^A0N," + labelFontSize + "," + labelFontSize + "^FD" + model.LabelName + "^FS";
                    }
                    //LotNo
                    if ((model.LotNoCharPosX.IndexOf("~") == -1))
                    {
                        zplData += "^FO" + model.LotNoCharPosX + "," + model.LotNoCharPosY + "^A0N," + labelFontSize + "," + labelFontSize + "^FD" + model.LotNoChar + "^FS";
                    }
                    //바코드문자
                    if ((model.BarcodeTextPosX.IndexOf("~") == -1))
                    {
                        zplData += "^FO" + model.BarcodeTextPosX + "," + model.BarcodeTextPosY + "^A0N," + fontSize + "," + fontSize + "^FD" + labelData + "^FS";
                    }

                    // HW , HW Char
                    if ((model.HW.IndexOf("~") == -1))
                    {
                        if ((model.HWChar.IndexOf("~") == -1))
                            zplData += "^FO" + model.HWPosX + "," + model.HWPosY + "^A0N," + model.FONT_SIZE + "," + model.FONT_SIZE + "^FD" + "H/W : " + model.HW + "^FS";
                        else
                            zplData += "^FO" + model.HWPosX + "," + model.HWPosY + "^A0N," + model.FONT_SIZE + "," + model.FONT_SIZE + "^FD" + model.HW + "^FS";
                    }
                    // SW, SW Char
                    if ((model.SW.IndexOf("~") == -1))
                    {
                        if ((model.SWChar.IndexOf("~") == -1))
                            zplData += "^FO" + model.SWPosX + "," + model.SWPosY + "^A0N," + model.FONT_SIZE + "," + model.FONT_SIZE + "^FD" + "S/W : " + model.SW + "^FS";
                        else
                            zplData += "^FO" + model.SWPosX + "," + model.SWPosY + "^A0N," + model.FONT_SIZE + "," + model.FONT_SIZE + "^FD" + model.SW + "^FS";
                    }


                    // Trace Description  RJ에서 Trace는 LIN통신용으로 사용된다. 
                    //if ((model.TracePosX.IndexOf("~") == -1))
                    //{
                    //    if ((model.TRACE_CODE == "") || (model.TRACE_CODE.IndexOf("~") == -1))
                    //    {
                    //        zplData += "^FO" + model.TracePosX + "," + model.TracePosY + "^A0N," + "20" + "," + "20";
                    //        zplData += "^FD";
                    //        zplData += (DateTime.Now.Year - 2000).ToString("#00");
                    //        zplData += DateTime.Now.Month.ToString("#00");
                    //        zplData += DateTime.Now.Day.ToString("#00");
                    //        //zplData += SerialNumber;
                    //        zplData += "^FS";
                    //    }
                    //    else zplData += model.TRACE_CODE;
                    //}
                    // Divisition Information
                    if ((model.DivisionPosX.IndexOf("~") == -1))
                    {
                        zplData += "^FO" + model.DivisionPosX + "," + model.DivisionPosY + "^A0N," + "20" + "," + "20" + "^FD" + model.DIVISION + "^FS";
                    }
                    


                    //2D바코드
                    zplData += "^FO" + model.D2PosX + "," + model.D2PosY + "^BXN,5,200^FH\\^FD";
                    zplData += "[)>\\1E06";  //1E : RS  06
                    zplData += "\\1D" + model.COMPANY_CODE;  //1D : GS 각단락을 구분해주는 구분자. VSS7H
                    zplData += "\\1D" + "P" + model.LabelNumber;   //P88590D9000
                    zplData += "\\1D" + model.SEQ_CODE;  //S
                    zplData += "\\1D" + model.EONO; //EQLQ044
                    zplData += "\\1D";

                    if ((model.TRACE_CODE == "") || (model.TRACE_CODE.IndexOf("~") == -1))
                    {
                        zplData += "T";
                        zplData += (DateTime.Now.Year - 2000).ToString("#00");
                        zplData += DateTime.Now.Month.ToString("#00");
                        zplData += DateTime.Now.Day.ToString("#00");
                        zplData += "DY1SA";  //교대+공장+라인+4M
                    }
                    else zplData += model.TRACE_CODE;
                    //T151123DY1SA

                    zplData += labelData;

                    //특이정보에 HW/SW 가 들어감 => 2015.10.23 현대차 요구로 저거함.
                    // A1.00 1.00
                    //initial information
                    //zplData += "\\1DM";

                    zplData += "\\1D" + model.INITIAL + model.HW + " " + model.SW;

                    //업체정보
                    zplData += "\\1D" + model.COMPANY_INFO;
                    zplData += "\\1D\\1E\\04^FS";  //1D : GS, 1E : RS,  04:EOT


                    zplData += "^XZ";

                    Send(zplData);


                }

            }//try

            catch (Exception ex)
            {
                eLogger.Error(ex.ToString());
            }

            return labelData;
        }

        public string ZebraQR_Common(string modelName, string printType)
        {
            try
            {
                GetLabelInfo(modelName, printType);
                for (int i = 0; i < count; i++)
                {
                    //ZPL 작성
                    //차종  IG 통풍모터에 적용/ ICT에서 발행하는것과 같음.
                    labelData = "";
                    labelData += model.CAR_CODE;

                    //4.연 O=> 2015년
                    if (model.YEAR == "숫자")
                        labelData += DateTime.Now.Year.ToString("#00");
                    else
                        labelData += (char)('A' + DateTime.Now.Year - 2001);
                    //5. 월 E => 5월
                    if (model.MONTH == "숫자")
                        labelData += DateTime.Now.Month.ToString("#00");
                    else
                        labelData += (char)('A' + DateTime.Now.Month - 1);
                    //6. 일 
                    if (model.DAY == "숫자")
                    {
                        labelData += DateTime.Now.Day.ToString("#00");
                    }
                    else
                    {
                        if (DateTime.Now.Day <= 26)
                            labelData += (char)('A' + DateTime.Now.Day - 1);
                        else
                            labelData += (DateTime.Now.Day - 26).ToString();
                    }
                    //7. 제품 일련번호
                    if (printType == "sample") labelData += (startPrintNumber + i).ToString("#0000");
                    else labelData += GetLabelSerial(modelName).ToString("#0000");


                    /////////////////////////////////////////
                    /// 화면에 뿌려질 라벨 정리

                    labelFontSize = fontSize + 10;
                    if (model.DARKNESS == null || model.DARKNESS == "") { model.DARKNESS = "20"; }
                    //width Values: 1 to 10
                    if (model.WIDTH == null || model.WIDTH == "") { model.DARKNESS = "2"; }

                    zplData = "";
                    zplData += "^XA";
                    zplData += "^PRA,A,A";  //프린트 속도
                    zplData += "~SD" + model.DARKNESS;  //프린트 Set Darkness

                    //품번-품명-LotNo-바코드문자-1D바코드-2D바코드-HWChar-SWChar-HW-SW순으로
                    //상단 P/N문자  제일 첫번째 ~ 가 index 0이고, 맞는것이 없으면 -1이 리턴된다. 
                    //품번
                    if ((model.LabelNumberPosX.IndexOf("~") == -1))
                    {
                        zplData += "^FO" + model.LabelNumberPosX + "," + model.LabelNumberPosY + "^A0N," + labelFontSize + "," + labelFontSize + "^FD" + model.SetModelNumber + "^FS";
                    }

                    //품명
                    if ((model.LabelNamePosX.IndexOf("~") == -1))
                    {
                        zplData += "^FO" + model.LabelNamePosX + "," + model.LabelNamePosY + "^A0N," + labelFontSize + "," + labelFontSize + "^FD" + model.LabelName + "^FS";
                    }
                    //LotNo
                    if ((model.LotNoCharPosX.IndexOf("~") == -1))
                    {
                        zplData += "^FO" + model.LotNoCharPosX + "," + model.LotNoCharPosY + "^A0N," + labelFontSize + "," + labelFontSize + "^FD" + model.LotNoChar + "^FS";
                    }
                    //바코드문자
                    if ((model.BarcodeTextPosX.IndexOf("~") == -1))
                    {
                        zplData += "^FO" + model.BarcodeTextPosX + "," + model.BarcodeTextPosY + "^A0N," + fontSize + "," + fontSize + "^FD" + labelData + "^FS";
                    }

                    // HW , HW Char
                    if ((model.HW.IndexOf("~") == -1))
                    {
                        if ((model.HWChar.IndexOf("~") == -1))
                            zplData += "^FO" + model.HWPosX + "," + model.HWPosY + "^A0N," + model.FONT_SIZE + "," + model.FONT_SIZE + "^FD" + "H/W : " + model.HW + "^FS";
                        else
                            zplData += "^FO" + model.HWPosX + "," + model.HWPosY + "^A0N," + model.FONT_SIZE + "," + model.FONT_SIZE + "^FD" + model.HW + "^FS";
                    }
                    // SW, SW Char
                    if ((model.SW.IndexOf("~") == -1))
                    {
                        if ((model.SWChar.IndexOf("~") == -1))
                            zplData += "^FO" + model.SWPosX + "," + model.SWPosY + "^A0N," + model.FONT_SIZE + "," + model.FONT_SIZE + "^FD" + "S/W : " + model.SW + "^FS";
                        else
                            zplData += "^FO" + model.SWPosX + "," + model.SWPosY + "^A0N," + model.FONT_SIZE + "," + model.FONT_SIZE + "^FD" + model.SW + "^FS";
                    }

                    //QR바코드
                    /**
                    Format: ^BQa,b,c,d,e
                    Parameters Details
                        a = field orientation Values: normal (^FW has no effect on rotation)
                        b = model Values: 1 (original) and 2 (enhanced – recommended)  
                            Default: 2
                        c = magnification factor Values: 1 to 10
                            Default:
                            1 on 150 dpi printers
                            2 on 200 dpi printers
                            3 on 300 dpi printers
                            6 on 600 dpi printers
                        d = error correction Values:
                            H = ultra-high reliability level
                            Q = high reliability level
                            M = standard level
                            L = high density level
                            Default:
                            Q = if empty
                            M = invalid values
                        e = mask value Values: 0 - 7
                            Default: 7
                    ex) ^BQN,2,10
                     **/
                    //zplData = "^XA";

                    zplData += "^FO" + model.D2PosX + "," + model.D2PosY;
                    zplData += "^BQN,2,3";
                    zplData += "^MMT,N";  //Print Mode T, P, R, A C, 
                    zplData += "~JS90";  //프린트 Change Backfeed Sequence
                    //Send("^XB");




                    //1 Q = error correction level  2 A, = automatic setting
                    zplData += "^FDQA," + model.HW + model.SW + labelData + "^FS";
                    //SW,HW, CARCODE, Year, Month , day, SN
                    zplData += "^XZ";

                    Send(zplData);




                    //Send("^XA^HH^XZ");  //현재 정보를 리턴한다. 
                    //Send("~HS");


                }

            }//try

            catch (Exception ex)
            {
                eLogger.Error(ex.ToString());
            }

            return labelData;
        }

        void GetLabelInfo(string modelName, string printType)
        {
            //TODO var models = from Model in Program.arrModel
            //             select Model;


            //model = models.First(Model => Model.SetModelNumber == modelName);

            //xml에서 가져올 데이타 담는 그릇
            zplData = "";  //바코드에 보낼 전체 프로토콜 데이타.
            startPrintNumber = 1;
            labelData = "";  //라벨 텍스트 데이타
            count = 1;  //라벨 출력 갯수 1부터 n까지..
            fontSize = 0;
            labelFontSize = 0;

            startPrintNumber = Convert.ToInt32(model.SERIAL_START);
            count = (printType == "sample") ? Convert.ToInt32(model.COUNT) : 1;
            fontSize = Convert.ToInt32(model.FONT_SIZE);


        }

    }//class
}//namespace
