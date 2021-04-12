using log4net;
using System;
using System.IO.Ports;
using System.Threading.Tasks;

namespace AUA.AiS_FruiT
{
    class DeviceBCR
    {
        private static readonly ILog pLogger = LogHelper.GetLoggerRollingFileAppender("Root");
        //(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly ILog eLogger = LogHelper.GetLoggerRollingFileAppender("Exception");
        //private ILog pLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //private ILog eLogger = LogManager.GetLogger("Exception");

        public delegate void MessageChangedHandler(string barcodeReceived);
        public event MessageChangedHandler BarcodeReceived;
        private SerialPort _serialPort;
        private string _receiveData { get; set; }
        private string _receiveDataBuffer = "";

        public bool IsConnected { get; set; }
        public bool IsReceived { get; set; }
        public string ReceiveData { get; set; }

        public DeviceBCR()
        {
            _serialPort = new SerialPort();
        }

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

                if (_serialPort.IsOpen)
                {
                    IsConnected = true;
                    _serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                    pLogger.Info("Connection Success");
                    return true;
                }
                else
                {
                    IsConnected = false;
                    pLogger.Info("Connection Failed");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                eLogger.Error("Connection Error", ex);
                return false;
            }

        }
        public void Close()
        {
            IsConnected = false;
            _serialPort.Close();
            _serialPort = null;
        }
        public void Clear()
        {
            IsReceived = false;
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

        void DataReceivedHandler(Object sender, SerialDataReceivedEventArgs e)
        {
            //ReadByte, ReadChar, ReadExisting, ReadLine 
            //SerialPort serialPort = (SerialPort)sender;
            string receiveData = _serialPort.ReadExisting();
            _receiveDataBuffer += receiveData;

            if (_receiveDataBuffer.IndexOf((char)0x0A) > -1
                || _receiveDataBuffer.IndexOf((char)0x0D) > -1)
            {
                _receiveData = _receiveDataBuffer;
                _receiveDataBuffer = "";
                ReceiveData = _receiveData;
                if (BarcodeReceived != null && ReceiveData.Length > 3)
                {
                    BarcodeReceived(ReceiveData);
                }

                Clear();
                IsReceived = true;
                return;

            }
        }
    }
}
