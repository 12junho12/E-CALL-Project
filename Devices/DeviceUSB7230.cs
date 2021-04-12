using System;
using System.IO.Ports;
using Modbus.Device;
using System.Threading;
using System.Timers;
using log4net;

namespace AUA.AiS_FruiT
{
    public class DeviceUSB7230
    {
        private static readonly ILog pLogger = LogHelper.GetLoggerRollingFileAppender("Root");
        private static readonly ILog eLogger = LogHelper.GetLoggerRollingFileAppender("Exception");

        public bool IsConnected { get; set; } = false;

        public static ushort _nCardId;
        public static ushort[] _CardID = new ushort[USBDASK.MAX_USB_DEVICE];
        private ushort handleCardID { get; set; } = 0;
        System.Timers.Timer _timer = new System.Timers.Timer();
        public DeviceUSB7230()
        {
            _timer.Interval = 100;
            _timer.Elapsed += new ElapsedEventHandler(Timer_Tick);
        }
        ~DeviceUSB7230()
        {
            Close();
        }
        public bool ScanDIO()
        {
            USBDAQ_DEVICE[] AvailModules = new USBDAQ_DEVICE[USBDASK.MAX_USB_DEVICE];
            ushort moduleNum;
            short error = USBDASK.UD_Device_Scan(out moduleNum, AvailModules);

            if (error != USBDASK.NoError)
            {
                eLogger.Error("UD_Device_Scan() failed, error code = {error}");
                return false;
            }

            int i, vi;

            for (i = 0, vi = 0; i < moduleNum; ++i)
            {
                if (AvailModules[i].wModuleType == USBDASK.USB_7230)
                {
                    _CardID[vi] = AvailModules[i].wCardID;

                    ++vi;
                }
            }

            if (vi != 0)
            {
                _nCardId = _CardID[0];

            }

            return true;
        }

        public void Connect(ushort cardId)
        {
            var rtn = USBDASK.UD_Register_Card(USBDASK.USB_7230, cardId);
            if (rtn < 0)
            {
                IsConnected = false;
                eLogger.Error("UD_Register_Card failed, error code = {rtn}");
                pLogger.Error("UD_Register_Card failed, error code = {rtn}");
            }
            else
            {
                IsConnected = true;
                handleCardID = (ushort)rtn;
            }


        }
        public void Start()
        {
            //_timer.Start();
        }
        public void Stop()
        {
            _timer.Stop();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {

        }
        public uint  ReadPort()
        {
            uint StatusByte = 0;
            short err;
            if (IsConnected)
            {
                err = USBDASK.UD_DI_ReadPort(handleCardID, 0, out StatusByte);
                if (err < 0)
                {
                    eLogger.Error("Error # : {err}");
                    IsConnected = false;
                }
                else
                {

                }
            }
            return StatusByte;

        }

        public bool ReadLine(ushort line)
        {
            short err;
            ushort getValue = 0;
            if (IsConnected)
            {
                err = USBDASK.UD_DI_ReadLine(handleCardID, 0, line, out getValue);
                if (err < 0)
                {
                    eLogger.Error("Error # : {err}");
                    IsConnected = false;
                    return false;
                }
                else
                {
                    if (getValue == 1) return true;
                }
            }
            return false;

        }

        public bool WritePort(uint sendByte)
        {
            short err;

            err = USBDASK.UD_DO_WritePort(handleCardID, 0, sendByte);
            if (err < 0)
            {
                eLogger.Error("Error # : {err}");
                IsConnected = false;
                return false;
            }
            return true;

        }

        public bool WriteLine(ushort line, ushort value)
        {
            short err;
            if (IsConnected)
            {
                err = USBDASK.UD_DO_WriteLine(handleCardID, 0, line, value);
                if (err < 0)
                {
                    eLogger.Error("Error # : {err}");
                    IsConnected = false;
                    return false;
                }
            }
            return true;
        }
        public void Close()
        {
            USBDASK.UD_Release_Card(handleCardID);
        }
    }
}
