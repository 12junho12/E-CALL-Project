using System;
using System.IO.Ports;
using Modbus.Device;
using System.Threading;
using System.Timers;
using log4net;

namespace AUA.AiS_FruiT
{
    public class DeviceMT4N
    {
        private static readonly ILog pLogger = LogHelper.GetLoggerRollingFileAppender("Root");
        private static readonly ILog eLogger = LogHelper.GetLoggerRollingFileAppender("Exception");
        public  double[] _measureValue = new double[2];
       
        System.Timers.Timer _timer = new System.Timers.Timer();
        private  string _portNumber = "";
        private object lockObject = new object();
        public  bool IsConnected { get; set; }
        public DeviceMT4N()
        {
            _timer.Interval = 100;
            _timer.Elapsed += new ElapsedEventHandler(Timer_Tick);
            //_timer.Enabled = true;
        }
        ~DeviceMT4N()
        {
            Stop();
        }
        public void Start()
        {
            _timer.Start();
        }
        public void Connect(string portNumber)
        {
            _portNumber = portNumber;
            try
            {
                if (ModbusSerialRTUMasterReadRegisters())
                {
                    IsConnected = true;
                }
                else
                {
                    IsConnected = false;
                }
            }
            catch (Exception ex)
            {
                IsConnected = false;
                eLogger.Error(ex.ToString());
            }

        }

        public void Stop()
        {
            _timer.Stop();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            //_timer.Stop();
            try
            {
                ModbusSerialRTUMasterReadRegisters();
            }
            catch (Exception ex)
            {
                eLogger.Error(ex.ToString());
            }
            
            //_timer.Start();
        }

        public void ModbusSerialRtuMasterWriteRegisters()
        {
            using (SerialPort port = new SerialPort("COM" + _portNumber.ToString()))
            {
                // configure serial port
                port.BaudRate = 9600;
                port.DataBits = 8;
                port.Parity = Parity.None;
                port.StopBits = StopBits.One;
                port.Open();

                //var adapter = new SerialPortAdapter(port);
                // create modbus master
                IModbusSerialMaster master = ModbusSerialMaster.CreateRtu(port);

                byte slaveId = 1;
                ushort startAddress = 100;
                ushort[] registers = new ushort[] { 1, 2, 3 };

                // write three registers
                master.WriteMultipleRegisters(slaveId, startAddress, registers);
            }
        }

        public  bool ModbusSerialRTUMasterReadRegisters()
        {
            lock (lockObject)
            {
                using (SerialPort port = new SerialPort("COM" + _portNumber.ToString()))
                {
                    // configure serial port
                    port.BaudRate = 9600;
                    port.DataBits = 8;
                    port.Parity = Parity.None;
                    port.StopBits = StopBits.One;
                    port.Open();

                    // create modbus master
                    ModbusSerialMaster master = ModbusSerialMaster.CreateRtu(port);
                    master.Transport.ReadTimeout = 1000;
                    try
                    {
                        //standard -> scale로 변경해야 자리수를 변경할 수 있다.
                        //master.WriteSingleRegister(3, 0x65, 1);
                        //master.WriteSingleRegister(5, 0x65, 1);
                      
                        // scale에서만 동작
                       //master.WriteSingleRegister(3, 0x66, 1);
                        //master.WriteSingleRegister(5, 0x66, 1);
                       
                        // 입력 레인지
                       // master.WriteSingleRegister(3, 0x67, 500);
                       //master.WriteSingleRegister(3, 0x67, 500);
                   

                        ushort[] register0 = master.ReadInputRegisters(1, 0x00, 1);
                        ushort[] register1 = master.ReadInputRegisters(2, 0x00, 2);
                        
                        ushort[] registers = { register0[0], register1[0] };

                        //MT4N Read Holding Register
                        //  ushort[] register3 = master.ReadHoldingRegisters(5, 0x64, 7);
                        for (int i = 0; i < 2; i++)
                        {
                            if (registers[i] > 300)
                            {
                            }
                            else
                            {
                                _measureValue[i] = (double)registers[i] / 1000.0;
                            }
                            _measureValue[i] = (double)registers[i] / 1000.0;
                        }
                         
                        //_measureValue[0] = (double)register0[0] / 1000.0;
                       // _measureValue[1] = (double)register1[0] / 1000.0;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        return false;
                    }
                    return true;
                }
            }
        }
    }
}
